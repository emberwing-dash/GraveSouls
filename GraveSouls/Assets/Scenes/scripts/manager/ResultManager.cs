using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [Header("References")]
    public GameManager gameManager;        // Reference to GameManager
    public RawImage goodEndingUI;          // RawImage for good ending
    public RawImage badEndingUI;           // RawImage for bad ending
    public Text timerText;                 // UI Text to show countdown

    [Header("Buttons")]
    public Button tryAgainButton;
    public Button backButton;

    [Header("Timer Settings")]
    public int startMinutes = 2;           // 2 minutes
    private float timerSeconds;
    private bool countdownActive = false;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


        // Hide ending UIs at start
        if (goodEndingUI != null) goodEndingUI.enabled = false;
        if (badEndingUI != null) badEndingUI.enabled = false;

        // Hide timer text initially
        if (timerText != null) timerText.gameObject.SetActive(false);

        // Hide buttons initially
        if (tryAgainButton != null) tryAgainButton.gameObject.SetActive(false);
        if (backButton != null) backButton.gameObject.SetActive(false);

        // Initialize timer
        timerSeconds = startMinutes * 60f;
    }

    /// <summary>
    /// Call this from the EnemyEncounterTrigger when the trigger happens
    /// </summary>
    public void StartTimer()
    {
        if (timerText != null)
            timerText.gameObject.SetActive(true);

        countdownActive = true;
    }

    private void Update()
    {
        if (!countdownActive) return;

        // Countdown timer
        if (timerSeconds > 0)
        {
            timerSeconds -= Time.deltaTime;

            // Update UI text if assigned
            if (timerText != null)
            {
                int minutes = Mathf.FloorToInt(timerSeconds / 60f);
                int seconds = Mathf.FloorToInt(timerSeconds % 60f);
                timerText.text = string.Format("Time {0:00}:{1:00}", minutes, seconds);
            }
        }
        else
        {
            // Timer reached zero -> good ending
            countdownActive = false;
            ShowGoodEnding();
        }

        // Check player health
        if (gameManager != null && gameManager.currentHealth <= 0)
        {
            countdownActive = false;
            ShowBadEnding();
        }
    }

    public void ShowGoodEnding()
    {
        if (goodEndingUI != null) goodEndingUI.enabled = true;
        if (badEndingUI != null) badEndingUI.enabled = false;

        // Show buttons
        if (tryAgainButton != null) tryAgainButton.gameObject.SetActive(true);
        if (backButton != null) backButton.gameObject.SetActive(true);

        // Make cursor visible
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("Good Ending!");
    }

    public void ShowBadEnding()
    {
        if (badEndingUI != null) badEndingUI.enabled = true;
        if (goodEndingUI != null) goodEndingUI.enabled = false;

        // Show buttons
        if (tryAgainButton != null) tryAgainButton.gameObject.SetActive(true);
        if (backButton != null) backButton.gameObject.SetActive(true);

        // Make cursor visible
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("Bad Ending!");
    }

}
