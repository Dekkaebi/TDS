using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using UnityEngine.Animations;
using System.Xml.Serialization;

public class Player : MonoBehaviour
{
    [Header("References")] 
    public PlayerMovementStats MoveStats;
    [SerializeField] private Collider _feetColl;
    [SerializeField] private Collider _bodyColl;
    [SerializeField] private Gun _gun;

    [Header("FX")] 
    public GameObject JumpParticles;
    public GameObject SecondJumpParticles;
    public GameObject LandParticles;
    public Position ParticleSpawnTransform;
    public TrailRenderer TrailRenderer;
    public ParticleSystem SpeedParticles;
    public GameObject DashParticles;
    public ParticleSystem WallSlideParticles;

    [Header("Debug")]
    public bool ShowDebugStats;
    private Rigidbody _rb;
    private Transform tf;

    //movement vars
    private Vector3 _moveVelocity;

    //collision check vars
    private RaycastHit _groundHit;
    private RaycastHit _headHit;
    private bool _isGrounded;
    private bool _bumpedHead;

    //jump vars
    public float VerticalVelocity { get; private set; }
    private bool _isJumping;
    private bool _isFastFalling;
    private bool _isFaling;
    private float _fastFallTime;
    private float _fastFallReleaseSpeed;
    private int _numberOfJumpsUsed;

    //apex vars
    private float _apexPoint;
    private float _timePastApexThreshold;
    private bool _isPastApexThreshold;

    //jump buffer vars
    private float _jumpBufferTimer;
    private bool _jumpReleaseDuringBuffer;

    //coyote time vars
    private float _coyoteTimer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        tf = GetComponent<Transform>();
    }

    private void Update()
    {
        //CountTimers();
        //JumpChecks();
        Rotation();
    }

    private void FixedUpdate()
    {
        //CollisionChecks();
        //Jump();

        if(_isGrounded)
        {
            Move(MoveStats.GroundAcceleration, MoveStats.GroundDeceleration, InputManager.Movement);
        }
        else
        {
            Move(MoveStats.AirAcceleration, MoveStats.AirDeceleration, InputManager.Movement);
        }

        // Shooting
        if (InputManager.ShootIsHeld)
        {
            Shoot();
        }
    }

    public bool IsJumping() { return _isJumping; }
    public bool IsGround() { return _isGrounded; }

    public bool IsFalling() { return _isFaling; }

    public bool IsFastFalling() { return _isFastFalling; }

    #region Movement

    private void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        if(moveInput != Vector2.zero)
        {
            // Реализовать поворот

            // Vector2 targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxWalkSpeed;

            Vector3 targetVelocity = new Vector3(moveInput.x, 0f, moveInput.y) * MoveStats.MaxWalkSpeed;

            // Это если будет бег но он будет по умолчанию мб кста можно сделать чтобы персонаж ходил в городах ну или опредеенных секциях, но перед этим стоит обнулить таргет
            // if(InputManager.RunIsHeld)
            // {
            //     targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxWalkSpeed;
            // }
            // else { targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxWalkSpeed; }

            // _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            _moveVelocity = Vector3.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            _rb.linearVelocity = new Vector3(_moveVelocity.x, _rb.linearVelocity.y, _moveVelocity.z);
            // _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocityY);
        }
        else if(moveInput == Vector2.zero)
        {
            // _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            _moveVelocity = Vector3.Lerp(_moveVelocity, Vector3.zero, deceleration * Time.fixedDeltaTime);
            _rb.linearVelocity = new Vector3(_moveVelocity.x, _rb.linearVelocity.y, _moveVelocity.z);
            // _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocityY);
        }
    }
    
    // Поворот в сторону мыши

    #endregion

    /*
    #region Jump
    // Проверка прыжка
    private void JumpChecks()
    {
        //WHEN WE PRESS THE JUMP BUTTON - нажал кнопку прыжжка
        if(InputManager.JumpWasPressed)
        {
            _jumpBufferTimer = MoveStats.JumpBufferTime;
            _jumpReleaseDuringBuffer = false;
        }

        //WHEN WE RELEASE THE JUMP BUTTON - отпустил кнопку прыжжка
        if(InputManager.JumpWasReleased)
        {
            if(_jumpBufferTimer > 0f)
            {
                _jumpReleaseDuringBuffer = true;
            }

            if(_isJumping && VerticalVelocity > 0f)
            {
                if(_isPastApexThreshold)
                {
                    _isPastApexThreshold = false;
                    _isFastFalling = true;
                    _fastFallTime = MoveStats.TimeForUpwardsCancel;
                    VerticalVelocity = 0f;
                }
                else
                {
                    _isFastFalling = true;
                    _fastFallReleaseSpeed = VerticalVelocity;
                }
            }
        }

        //INITIATE JUMP WITH JUMP BUFFERING AND COYOTE TIME
        // Инициализируем прыжок если он на земле и не в прыжке или все ещё может совершить прыжок после не нахождении на земле (_coyoteTimer)
        if(_jumpBufferTimer > 0f && !_isJumping && (_isGrounded || _coyoteTimer > 0f))
        {
            // Инициализирует что прыжок должен произойти
            InitiateJump(1);

            // если отпустил кнопку то должен падать
            if(_jumpReleaseDuringBuffer)
            {
                _isFastFalling = true;
                _fastFallReleaseSpeed = VerticalVelocity;
            }
        }

        //DOUBLE JUMP
        else if(_jumpBufferTimer > 0f && _isJumping && _numberOfJumpsUsed < MoveStats.NumberOfJumpsAllowed)
        {
            _isFastFalling = false;
            // Инициализирует что прыжок должен произойти
            InitiateJump(1);
        }

        //AIR JUMP AFTER COYOTE TIME LAPSED
        else if(_jumpBufferTimer > 0f && _isFaling && _numberOfJumpsUsed < MoveStats.NumberOfJumpsAllowed - 1)
        {
            // Инициализирует что прыжок должен произойти
            InitiateJump(2);
            _isFastFalling = false;
        }

        //LANDED
        // Приземление
        if((_isJumping || _isFaling) && _isGrounded && VerticalVelocity <= 0f)
        {
            _isJumping = false;
            _isFaling = false;
            _isFastFalling = false;
            _fastFallTime = 0f;
            _isPastApexThreshold = false;
            _numberOfJumpsUsed = 0;

            VerticalVelocity = Physics2D.gravity.y;
        }
    }

    private void InitiateJump(int numberOfJumpsUsed)
    {
        if(!_isJumping)
        {
            _isJumping = true;
        }

        _jumpBufferTimer = 0f;
        _numberOfJumpsUsed += numberOfJumpsUsed;
        VerticalVelocity = MoveStats.InitialJumpVelocity;
    }

    private void Jump()
    {
        //APPLY GRAVITY WHILE JUMPING
        if(_isJumping)
        {
            //CHECK FOR HEAD BUMP
            if(_bumpedHead)
            {
                _isFastFalling = true;
            }

            //GRAVITY ON ASCENDING
            if(VerticalVelocity >= 0f)
            {
                //APEX CONTROLS
                _apexPoint = Mathf.InverseLerp(MoveStats.InitialJumpVelocity, 0f, VerticalVelocity);

                if(_apexPoint > MoveStats.ApexThreshold)
                {
                    if(!_isPastApexThreshold)
                    {
                        _isPastApexThreshold = true;
                        _timePastApexThreshold = 0f;
                    }

                    if(_isPastApexThreshold)
                    {
                        _timePastApexThreshold += Time.fixedDeltaTime;
                        if(_timePastApexThreshold < MoveStats.ApexHangTime)
                        {
                            VerticalVelocity = 0f;
                        }
                        else
                        {
                            VerticalVelocity -= 0.01f;
                        }
                    }
                }

                //GRAVITY ON ASCENDING BUT NOT PAST APEX THRESHOLD
                else
                {
                    VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
                    if(_isPastApexThreshold)
                    {
                        _isPastApexThreshold = false;
                    }
                }
            }

            //GRAVITY ON DESCENDING
            else if(!_isFastFalling)
            {
                VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }

            else if(VerticalVelocity < 0f)
            {
                if(!_isFaling)
                {
                    _isFaling = true;
                }
            }
        }

        //JUMP CUT
        if(_isFastFalling)
        {
            if(_fastFallTime >= MoveStats.TimeForUpwardsCancel)
            {
                VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else if(_fastFallTime < MoveStats.TimeForUpwardsCancel)
            {
                VerticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / MoveStats.TimeForUpwardsCancel));
            }

            _fastFallTime += Time.fixedDeltaTime;
        }

        //NORMAL GRAVITY WHILE FALLING
        if(!_isGrounded && !_isJumping)
        {
            if(!_isFaling)
            {
                _isFaling = true;
            }

            VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
        }

        //CLAMP FALL SPEED
        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -MoveStats.MaxFallSpeed, 50f);

        _rb.linearVelocity = new Vector2(_rb.linearVelocityX, VerticalVelocity);
    }

    #endregion

    #region Draw Jump Arc

    private void DrawJumpArc(float moveSpeed, Color gizmoColor)
    {
        Vector2 startPosition = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
        Vector2 previousPosition = startPosition;
        float speed = 0f;
        if(MoveStats.DrawRight)
        {
            speed = moveSpeed;
        }
        else { speed = -moveSpeed; }
        Vector2 velocity = new Vector2(speed, MoveStats.InitialJumpVelocity);

        Gizmos.color = gizmoColor;

        float timeStep = 2 * MoveStats.TimeTillJumpApex / MoveStats.ArcResolution; //time step for the simulation
        //float totalTime = (2 * MoveStats.TimeTillJumpApex) + MoveStats.ApexHangTime; // total time of the arc including hang time

        for(int i = 0; i < MoveStats.VisualizationSteps; i++)
        {
            float simulationTime = i * timeStep;
            Vector2 displacement;
            Vector2 drawPoint;

            if(simulationTime < MoveStats.TimeTillJumpApex) //Ascending
            {
                displacement = velocity * simulationTime + 0.5f * new Vector2(0, MoveStats.Gravity) * simulationTime * simulationTime;
            }
            else if(simulationTime < MoveStats.TimeTillJumpApex + MoveStats.ApexHangTime) //apex hang time
            {
                float apexTime = simulationTime - MoveStats.TimeTillJumpApex;
                displacement = velocity * MoveStats.TimeTillJumpApex + 0.5f * new Vector2(0, MoveStats.Gravity) * MoveStats.TimeTillJumpApex * MoveStats.TimeTillJumpApex;
                displacement += new Vector2(speed, 0) * apexTime; //No vertical movement during hang time
            }
            else //Descending
            {
                float descendTime = simulationTime - (MoveStats.TimeTillJumpApex + MoveStats.ApexHangTime);
                displacement = velocity * MoveStats.TimeTillJumpApex + 0.5f * new Vector2(0, MoveStats.Gravity) * MoveStats.TimeTillJumpApex * MoveStats.TimeTillJumpApex;
                displacement += new Vector2(speed, 0) * MoveStats.ApexHangTime; //horizontal movement during hang time
                displacement += new Vector2(speed, 0) * descendTime + 0.5f * new Vector2(0, MoveStats.Gravity) * descendTime * descendTime;
            }

            drawPoint = startPosition + displacement;

            if(MoveStats.StopOnCollision)
            {
                RaycastHit2D hit = Physics2D.Raycast(previousPosition, drawPoint - previousPosition, Vector2.Distance(previousPosition, drawPoint), MoveStats.GroundLayer);
                if(hit.collider != null)
                {
                    //If a hit is detected, stop drawing the arc at the hit point
                    Gizmos.DrawLine(previousPosition, hit.point);
                    break;
                }
            }

            Gizmos.DrawLine(previousPosition, drawPoint);
            previousPosition = drawPoint;
        }
    }

    #endregion

    private void OnDrawGizmos()
    {
        if(MoveStats.ShowWalkJumpArc)
        {
            DrawJumpArc(MoveStats.MaxWalkSpeed, Color.white);
        }
    }
    *

    #region Collision Checks

    private void IsGrounded()
    {
        // Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
        // Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, MoveStats.GroundDetectionRayLength);

        Vector3 boxCastOrigin = new Vector3(_feetColl.bounds.center.x, _feetColl.bounds.min.y, _feetColl.bounds.min.z);
        Vector2 boxCastSize = new Vector3(_feetColl.bounds.size.x, MoveStats.GroundDetectionRayLength);
        Quaternion boxQaternion = new Quaternion(1, 1, 0, 0);

        _groundHit = Physics.CapsuleCast(boxCastOrigin, boxCastSize, Vector3.down, boxQaternion, MoveStats.GroundDetectionRayLength, MoveStats.GroundLayer);
        if(_groundHit.collider != null)
        {
            _isGrounded = true;
        }
        else { _isGrounded = false; }

        #region Debug Visualization
        if(MoveStats.DebugShowIsGroundedBox)
        {
            Color rayColor;
            if(_isGrounded)
            {
                rayColor = Color.green;
            }
            else { rayColor = Color.red; }

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.GroundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.GroundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - MoveStats.GroundDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);
        }
        #endregion
    }

    private void BumpedHead()
    {
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _bodyColl.bounds.max.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x * MoveStats.HeadWith, MoveStats.HeadDetectionRayLength);

        _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, MoveStats.HeadDetectionRayLength, MoveStats.GroundLayer);
        if(_headHit.collider != null)
        {
            _bumpedHead = true;
        }
        else { _bumpedHead = false; }

        #region Debug Visualization
        if(MoveStats.DebugShowHeadBumpBox)
        {
            float headWith = MoveStats.HeadWith;

            Color rayColor;
            if(_bumpedHead)
            {
                rayColor = Color.green;
            }
            else { rayColor = Color.red; }

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWith, boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + (boxCastSize.x / 2) * headWith, boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWith, boxCastOrigin.y + MoveStats.HeadDetectionRayLength), Vector2.right * boxCastSize.x * headWith, rayColor);
        }
        #endregion
    }

    private void CollisionChecks()
    {
        IsGrounded();
        BumpedHead();
    }

    #endregion

    #region Timers

    private void CountTimers()
    {
        _jumpBufferTimer -= Time.deltaTime;

        if(!_isGrounded)
        {
            _coyoteTimer -= Time.deltaTime;
        }
        else { _coyoteTimer = MoveStats.JumpCoyoteTime; }
    }

    #endregion

    */
    
    #region Rotate

    void Rotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Aim);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            LookAt(point);
        }
    }

    private void LookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, tf.position.y, lookPoint.z);
        //tf.LookAt(heightCorrectedPoint);

        Vector3 direction = (heightCorrectedPoint - tf.position).normalized;

        // Вычисляем желаемый поворот
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Плавно поворачиваем персонажа
        tf.rotation = Quaternion.Slerp(tf.rotation, targetRotation, MoveStats.AimAcceleration * Time.fixedDeltaTime);
    }

    #endregion

    #region Shoot
    
    private void Shoot()
    {   
        _gun.Shoot();

        if (MoveStats.ShootDebug)
        {
            _gun.DebugShoot();
        }
    }

    #endregion
}