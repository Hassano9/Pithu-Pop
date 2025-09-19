using UnityEngine;

public class MobilePlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 1f;
    public float runSpeed = 3f;
    public float rotationSpeed = 6f;

    [Header("References")]
    public Joystick joystick;  // Works for FloatingJoystick too
    public Animator anim;               // Drag your Animator here (Idle/Walk/Run)

    void Update()
    {
        if (joystick == null)
        {
            Debug.LogWarning("⚠️ Joystick is not assigned in the Inspector!");
            return;
        }

        if (anim == null)
        {
            Debug.LogWarning("⚠️ Animator is not assigned in the Inspector!");
            return;
        }

        // Get joystick input
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;
        Vector3 input = new Vector3(horizontal, 0, vertical);
        float mag = input.magnitude;

        // Move if input is detected
        if (mag > 0.1f)
        {
            bool running = mag > 0.6f;
            float speed = running ? runSpeed : walkSpeed;

            // Move character using transform
            transform.Translate(input.normalized * speed * Time.deltaTime, Space.World);

            // Rotate smoothly towards movement direction
            Quaternion targetRot = Quaternion.LookRotation(input.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        // Update Animator parameter (0 = Idle, 0.1–0.6 = Walk, >0.6 = Run)
        anim.SetFloat("Speed", mag);
    }
}
