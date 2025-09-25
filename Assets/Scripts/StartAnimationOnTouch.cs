using UnityEngine;

public class StartAnimationOnTouch : MonoBehaviour
{
    private Animator animator;
    private MobilePlayerController playerController;
    private bool hasPlayed = false;

    [Header("UI")]
    public GameObject tapToStartText;
    public GameObject floatingJoystickUI;

    [Header("Stones")]
    public GameObject stoneParent;
    public float scatterRadius = 3f;   // how far they scatter
    public float scatterDuration = 1f; // how long it takes
    public float bounceHeight = 1f;    // upward bounce strength

    private bool scatterStarted = false;
    private float scatterTimer = 0f;
    private Vector3[] startPositions;
    private Vector3[] targetPositions;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<MobilePlayerController>();

        if (playerController != null)
            playerController.canMove = false;

        if (tapToStartText != null)
            tapToStartText.SetActive(true);

        if (floatingJoystickUI != null)
            floatingJoystickUI.SetActive(false);
    }

    void Update()
    {
        if (!hasPlayed && (Input.touchCount > 0 || Input.GetMouseButtonDown(0)))
        {
            PlayIntro();
        }

        if (scatterStarted)
        {
            scatterTimer += Time.deltaTime;
            float t = scatterTimer / scatterDuration;

            // Ease out for smoother scatter
            float easedT = 1f - Mathf.Pow(1f - t, 3);

            for (int i = 0; i < stoneParent.transform.childCount; i++)
            {
                Transform stone = stoneParent.transform.GetChild(i);

                // Basic scatter position
                Vector3 flatPos = Vector3.Lerp(startPositions[i], targetPositions[i], easedT);

                // Add bounce (parabola: up then down)
                float yOffset = Mathf.Sin(Mathf.Clamp01(t) * Mathf.PI) * bounceHeight;

                stone.position = new Vector3(flatPos.x, flatPos.y + yOffset, flatPos.z);
            }

            if (t >= 1f) scatterStarted = false;
        }
    }

    void PlayIntro()
    {
        hasPlayed = true;

        if (tapToStartText != null)
            tapToStartText.SetActive(false);

        animator.SetTrigger("PlayIntro");

        float introLength = 2f; 
        Invoke(nameof(EnableGameplay), introLength);
    }

    void EnableGameplay()
    {
        if (playerController != null)
            playerController.canMove = true;

        if (floatingJoystickUI != null)
            floatingJoystickUI.SetActive(true);

        // Scatter randomly
        ScatterStonesRandomCircle();
    }

    void ScatterStonesRandomCircle()
    {
        int count = stoneParent.transform.childCount;
        startPositions = new Vector3[count];
        targetPositions = new Vector3[count];

        for (int i = 0; i < count; i++)
        {
            Transform stone = stoneParent.transform.GetChild(i);
            startPositions[i] = stone.position;

            // Random direction on XZ plane
            float angle = Random.Range(0f, Mathf.PI * 2f);
            Vector3 dir = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

            targetPositions[i] = stone.position + dir * scatterRadius;
        }

        scatterStarted = true;
        scatterTimer = 0f;
    }
}
