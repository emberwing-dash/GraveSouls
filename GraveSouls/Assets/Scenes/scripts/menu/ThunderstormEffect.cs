using UnityEngine;

public class ThunderstormEffect : MonoBehaviour
{
    [Header("Thunderstorm Settings")]
    public Color darkColor = Color.black;
    public Color flashColor = new Color(0.25f, 0.25f, 0.25f); // dark grey flash
    public float flashSpeed = 2f;      // how fast color changes (higher = faster flash)
    public float delayBetweenFlashes = 3f; // seconds between each flash

    private Camera mainCam;
    private bool flashing = false;
    private float timer = 0f;

    void Start()
    {
        mainCam = Camera.main;
        if (mainCam != null)
            mainCam.backgroundColor = darkColor;
    }

    void Update()
    {
        if (mainCam == null) return;

        timer += Time.deltaTime;

        // When it's time for a flash
        if (!flashing && timer >= delayBetweenFlashes)
        {
            timer = 0f;
            StartCoroutine(FlashRoutine());
        }
    }

    private System.Collections.IEnumerator FlashRoutine()
    {
        flashing = true;

        // fade to flash color
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * flashSpeed;
            mainCam.backgroundColor = Color.Lerp(darkColor, flashColor, t);
            yield return null;
        }

        // fade back to dark color
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * flashSpeed;
            mainCam.backgroundColor = Color.Lerp(flashColor, darkColor, t);
            yield return null;
        }

        flashing = false;
    }
}
