using UnityEngine;
using TMPro; // if using TextMeshPro

public class StartAnimationOnTouch : MonoBehaviour
{
    private Animator animator;
    private MobilePlayerController playerController;
    private bool hasPlayed = false;

    [Header("UI")]
    public GameObject tapToStartText;     // Assign in Inspector
    public GameObject floatingJoystickUI; // Assign your Floating Joystick UI here

    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<MobilePlayerController>();

        // Block movement at the start
        if (playerController != null)
            playerController.canMove = false;

        // Show the Tap to Start text
        if (tapToStartText != null)
            tapToStartText.SetActive(true);

        // Hide joystick until intro finishes
        if (floatingJoystickUI != null)
            floatingJoystickUI.SetActive(false);
    }

    void Update()
    {
        if (!hasPlayed && (Input.touchCount > 0 || Input.GetMouseButtonDown(0)))
        {
            PlayIntro();
        }
    }

    void PlayIntro()
    {
        hasPlayed = true;

        // Hide Tap to Start text
        if (tapToStartText != null)
            tapToStartText.SetActive(false);

        // Play intro animation
        animator.SetTrigger("PlayIntro");

        // Either detect length dynamically OR just hardcode your animation length
        float introLength = 2f; // Replace with your animation’s length in seconds
        Invoke(nameof(EnableGameplay), introLength);
    }

    void EnableGameplay()
    {
        if (playerController != null)
            playerController.canMove = true;

        if (floatingJoystickUI != null)
            floatingJoystickUI.SetActive(true);
    }
}
