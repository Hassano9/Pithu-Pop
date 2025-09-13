using UnityEngine;

public class MobilePlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 1f;
    public float runSpeed = 3f;
    public float rotationSpeed = 7f;

    [Header("References")]
    public Joystick joystick;   // Drag JoystickBG here
    public Animator anim;       // Drag your Animator here (the one with Idle/Walk/Run)

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
        Vector3 input = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        float mag = input.magnitude;

        // Debug joystick magnitude
        Debug.Log("🎮 Joystick magnitude = " + mag);

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

        // Update Animator parameter
        anim.SetFloat("Speed", mag);
        Debug.Log("🎬 Animator Speed set to: " + mag);
    }
}
