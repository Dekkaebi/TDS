using UnityEngine;

public class characterAnimator : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isForwardHash;
    int isBackwardHash;
    int isLeftHash;
    int isRightHash;
    void Start()
    {
        animator = GetComponent<Animator>();
        isForwardHash = Animator.StringToHash("isForward");
        isBackwardHash = Animator.StringToHash("isBackward");
        isLeftHash = Animator.StringToHash("isLeft");
        isRightHash = Animator.StringToHash("isRight");
        isWalkingHash = Animator.StringToHash("isWalking");
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isForward = animator.GetBool(isForwardHash);
        bool isBackward = animator.GetBool(isBackwardHash);
        bool isLeft = animator.GetBool(isLeftHash);
        bool isRight = animator.GetBool(isRightHash);
        bool forwardPressed = Input.GetKey("w");
        bool backwardPressed = Input.GetKey("s");
        bool leftPressed = Input.GetKey("a");
        bool rightPressed = Input.GetKey("d");

        //Walk Forwad           
        if (!isForward && forwardPressed)
        {
            animator.SetBool(isForwardHash, true);
            animator.SetBool(isWalkingHash, true);
        }
        if (isForward && !forwardPressed)
        {
            animator.SetBool(isForwardHash, false);
            animator.SetBool(isWalkingHash, false);
        }
        //Walk Backward
        if (!isBackward && backwardPressed)
        {
            animator.SetBool(isBackwardHash, true);
            animator.SetBool(isWalkingHash, true);
        }
        if (isBackward && !backwardPressed)
        {
            animator.SetBool(isBackwardHash, false);
            animator.SetBool(isWalkingHash, false);
        }
        //Walk Left
        if (!isLeft && leftPressed)
        {
            animator.SetBool(isLeftHash, true);
            animator.SetBool(isWalkingHash, true);
        }
        if (isLeft && !leftPressed)
        {
            animator.SetBool(isLeftHash, false);
            animator.SetBool(isWalkingHash, false);
        }
        //Right Left
        if (!isRight && rightPressed)
        {
            animator.SetBool(isRightHash, true);
            animator.SetBool(isWalkingHash, true);
        }
        if (isRight && !rightPressed)
        {
            animator.SetBool(isRightHash, false);
            animator.SetBool(isWalkingHash, false);
        }
    }
}
