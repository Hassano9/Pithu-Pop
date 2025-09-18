using UnityEngine;

public class dustcontroller : MonoBehaviour
{
    public ParticleSystem dustTrail;
    public Joystick joystick;   // Reference to your joystick script
    public float minInput = 0.1f; // Deadzone (so tiny movements don’t trigger dust)

    void Update()
    {
        float h = joystick.Horizontal;
        float v = joystick.Vertical;

        if (Mathf.Abs(h) > minInput || Mathf.Abs(v) > minInput)
        {
            if (!dustTrail.isPlaying)
                dustTrail.Play();
        }
        else
        {
            if (dustTrail.isPlaying)
                dustTrail.Stop();
        }
    }
}
