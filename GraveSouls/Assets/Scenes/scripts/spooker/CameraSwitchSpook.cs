using UnityEngine;

public class CameraTriggerSwitch : MonoBehaviour
{
    [Header("Camera Settings")]
    public GameObject newCameraObject; // assign the new camera GameObject (not just the Camera component)
    public string triggerTag = "Player"; // tag that triggers the event

    [Header("Animator Settings")]
    public Animator currentAnimator; // assign the animator you want to stop

    [Header("Result Reference")]
    public ResultManager resultManager; // assign your ResultManager in inspector

    [Header("Audio Settings")]
    public AudioSource currentAudioSource; // audio currently playing
    public AudioSource nextAudioSource;    // audio to play when triggered

    private void Start()
    {
        // Make sure new camera is off initially
        if (newCameraObject != null)
            newCameraObject.SetActive(false);

        // Ensure next audio does not play at start
        if (nextAudioSource != null)
        {
            nextAudioSource.Stop();
            nextAudioSource.playOnAwake = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            // Disable the currently active camera
            Camera currentCam = Camera.main;
            if (currentCam != null)
                currentCam.gameObject.SetActive(false);

            // Stop current animation if animator is assigned
            if (currentAnimator != null)
                currentAnimator.enabled = false; // stops animation immediately

            // Enable the new camera
            if (newCameraObject != null)
                newCameraObject.SetActive(true);

            // 🎵 Handle audio switch (only play new audio now)
            HandleAudioSwitch();

            // Trigger ResultManager -> Bad Ending
            if (resultManager != null)
            {
                resultManager.ShowBadEnding();
            }
        }
    }

    private void HandleAudioSwitch()
    {
        if (currentAudioSource != null && currentAudioSource.isPlaying)
            currentAudioSource.Stop();

        if (nextAudioSource != null)
            nextAudioSource.Play(); // starts only when triggered
    }
}
