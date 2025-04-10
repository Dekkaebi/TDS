using UnityEngine;
using System.Collections;
using System;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

[RequireComponent (typeof (CharacterController))]
public class PlayerController : MonoBehaviour {

    public float rotationSpeed = 360;
    public float walkSpeed = 5;
    public float runSpeed = 8;
    private float acceleration = 5;
    public float Speed;

    private Quaternion targetRotation;
    private Vector3 Velocity;

    public Gun gun;
    private CharacterController controller;
    private Animator animator;
    private Camera cam;

    void Start() {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cam = Camera.main;
    }

    void Update() {
        ControlMouse();
        if (Input.GetButton("Shot")) {
            gun.Shoot();
        }
    }
    void ControlMouse() //���������� ����� �������� ���� + WASD
    {
        // �������� ����, �������� ���������
        Vector3 mousePos = Input.mousePosition;
        mousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.transform.position.y - transform.position.y));                                                   
        targetRotation = Quaternion.LookRotation(mousePos - new Vector3(transform.position.x, 0, transform.position.z));                                    
        transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
        

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Velocity = Vector3.MoveTowards(Velocity, input, acceleration * Time.deltaTime);
        Vector3 motion = Velocity;
        motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? .7f : 1; // ���������� ��������� ��� �������� ��� ����
        motion *= (Input.GetButton("Run")) ? runSpeed : walkSpeed;
        motion += Vector3.up * -8;
        //animator.SetFloat("Speed", Mathf.Sqrt(motion.x * motion.x + motion.z * motion.z));
        controller.Move(motion * Time.deltaTime);

    }

}
