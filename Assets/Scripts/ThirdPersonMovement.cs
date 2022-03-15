using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    private Animator animator;
    public Transform cam;

    //Gravity Parameters
    public float speed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    bool isGrounded;

    //Turning Paramters
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Checks that the player is grounded
        //In Other Words
        /*
         * bool Physics.CheckSphere(Vector3 position, float radius, Layer Mask)
         */
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Move();

        //This is how gravity will behave
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //Jump Settings
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

    }

    void Move()
    {
        //Finds and assigns the child of the player named "Lady Pirate"


        //Input Settings
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            //Atan 2 is a Mathematical Function that returns the Angle between the X Axis
            //And the Angle starting at Zero and ending at x,y

            //Atan function gives us our angle in radians, so we utilize
            //"Rad2Deg" to convert the angle from Radians to Degrees
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            //Quaternion.Euler allows us to input for x,y and z
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //We turn our Quaternion into a direction by multiplying by our vector
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

            //Animation
            animator.SetBool("isWalking",true);
        }
        else
        {
            //Animation
            animator.SetBool("isWalking",false);
        }
    }
}
