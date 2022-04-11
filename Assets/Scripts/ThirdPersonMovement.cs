using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cinemachine; //Always Add when Using Cinemachine

public class ThirdPersonMovement : MonoBehaviour
{

    public CharacterController controller;
    public Transform cam;
    public BoxCollider drownCollider;

    //Gravity Parameters
    public float speed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;

    //Death Parameters
    public float health = 100.0f;

    //Turning
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

   //Animation Parameters
    private Animator animator;
    private string currentState;

    //Animation States
    const string Player_Idle = "Player_Idle";
    const string Player_Walking = "Player_Walking";
    const string Player_Jumping = "Player_Jumping";
    const string Player_Fire_Idle = "Player_Fire_Idle";
    const string Player_Firing_Forward = "Player_Firing_Forward";
    const string Player_Rifle_Run_Forward = "Player_Rifle_Run_Forward";
    const string Player_Rifle_Run_Backwards = "Player_Rifle_Run_Backwards";
    const string Player_Rifle_Run_Right = "Player_Rilfe_Run_Right";
    const string Player_Rifle_Run_Left = "Player_Rifle_Run_Left";
    const string Player_Death = "Player_Death";


    //bools
    bool isGrounded;
    bool jumping;
    bool weaponActivated;
    bool Dead;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        Move(); //Move

        isJumping(); //Jump

        isShooting(); //Shoot

        Death(); //Game Over
    }

    void Move()
    {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            //Animation and Movement
            if (isGrounded)
            { //If on The Ground
                if (direction.magnitude > 0.1f) //If Moving
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

                    if (!weaponActivated)                                   //If weapon Activated
                    {
                        ChangeAnimationState(Player_Walking);
                    }
                    else
                    {                                                  //If weapon not activted
                        ChangeAnimationState(Player_Rifle_Run_Forward);
                    }
                }
                else if (direction.magnitude <= 0f) //If not Moving
                {
                    if (weaponActivated)
                    {
                        ChangeAnimationState(Player_Fire_Idle);
                    }
                    else if (!weaponActivated)
                    {
                        ChangeAnimationState(Player_Idle);
                    }
                }
            }
            else if (jumping == true) //If not On Ground
            {
                ChangeAnimationState(Player_Jumping);
            }
    }

    //Detects if Jumping is Activated 
    void isJumping()
    {
        //Checks that the player is grounded
        //In Other Words
        /*
         * bool Physics.CheckSphere(Vector3 position, float radius, Layer Mask)
         */
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        jumping = !isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //This is how gravity will behave
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //Jump Settings
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            jumping = true;
        }
    }

    //Detects if Weapon is Activated
    void isShooting()
    {
        if (Input.GetMouseButtonDown(1)) //Left Click Down 
        {
            Debug.Log("Weapon Activated");
            weaponActivated = true;
        }
        if (Input.GetMouseButtonUp(1)) //Left Click Up
        {
            Debug.Log("Weapon Deactivated");
            weaponActivated = false;
        }
    }

    //Checks for Player Death
    void Death()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            Debug.Log("Player Is Dead");
        }
        else
        {
            Debug.Log("Player is Alive");
        }

    }

    //Changes Animation 
    void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting itself
        if (currentState == newState) return;

        //play the animation
        animator.Play(newState);

        //reassign the current state
        currentState = newState;
    }
}
