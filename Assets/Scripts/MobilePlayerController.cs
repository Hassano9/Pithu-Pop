using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MobilePlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 1f;
    public float runSpeed = 3f;
    public float rotationSpeed = 6f;

    [Header("References")]
    public Joystick joystick;  
    public Animator anim;      

    [Header("Control")]
    public bool canMove = true;  

    private CharacterController controller;
    private float gravity = -9.81f;
    private float verticalVelocity = 0f; // Y movement storage

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Make sure controller won't climb things
        controller.stepOffset = 0f;
        controller.slopeLimit = 0f;
    }

    void Update()
    {
        if (joystick == null || anim == null) return;

        if (!canMove)
        {
            anim.SetFloat("Speed", 0);
            return;
        }

        // Joystick input
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;
        Vector3 input = new Vector3(horizontal, 0, vertical);
        float mag = input.magnitude;

        // Movement vector (horizontal only)
        Vector3 move = Vector3.zero;

        if (mag > 0.1f)
        {
            bool running = mag > 0.6f;
            float speed = running ? runSpeed : walkSpeed;

            move = input.normalized * speed;

            // Smooth rotation
            Quaternion targetRot = Quaternion.LookRotation(input.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        // ✅ Gravity handling
        if (controller.isGrounded)
        {
            verticalVelocity = -0.1f; // keep player "stuck" to ground
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        // Apply gravity
        move.y = verticalVelocity;

        // ✅ Final move
        controller.Move(move * Time.deltaTime);

        // Animator update
        anim.SetFloat("Speed", mag);
    }
}
