using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedTreeMovement : MonoBehaviour
{
    Animator animator;
    float velocityZ = 0.0f;
    float velocityX = 0.0f;
    public float acceleration = 2.0f;
    public float deceleration = 2.0f;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool forwardPressed = Input.GetKey("w");
        bool backwardPressed = Input.GetKey("s");
        bool leftPressed = Input.GetKey("a");
        bool rightPressed = Input.GetKey("d");
        
        // Движение вперёд
        if (forwardPressed && velocityZ < 0.5f)
        {
            velocityZ += Time.deltaTime * acceleration;
        }
        // Движение вниз
        if (backwardPressed && velocityZ > -0.5f)
        {
            velocityZ -= Time.deltaTime * acceleration;
        }
        // Движение влево
        if (leftPressed && velocityX > -0.5f)
        {
            velocityX -= Time.deltaTime * acceleration;
        }
        // Движение направо 
        if (rightPressed && velocityX < 0.5f)
        {
            velocityX += Time.deltaTime * acceleration;
        }
        //Уменьшение значения velocityZ при отсутствии движения вперёд
        if (!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ = -Time.deltaTime * deceleration;
        }
        //Увеличение значения velocityZ при отсутствии движения назад
        if (!backwardPressed && velocityZ < 0.0f)
        {
            velocityZ = +Time.deltaTime * deceleration;
        }
        //Обнуление velocityZ при отсутствии движения
        if (!forwardPressed && !backwardPressed && velocityZ != 0.0f && (velocityZ > -0.05f && velocityZ < 0.05f))
        {
            velocityZ = 0.0f;
        }
        //Увеличение значения velocityX при отсутствии движения вперёд
        if (!leftPressed && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }
        //Уменьшение значения velocityZ при отсутствии движения назад
        if (!rightPressed && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * deceleration;
        }
        //Обнуление velocityX при отсутствии движения
        if (!leftPressed && !rightPressed && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0.0f;
        }
        //Задаём значения переменныйх в аниматоре
        animator.SetFloat("velocity X", velocityX);
        animator.SetFloat("velocity Z", velocityZ);
    }
}
