using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Hover Settings")]
    public float popScale = 1.15f;
    public float speed = 8f;
    public float glowIntensity = 1.5f; // how much brighter on hover

    [Header("Button Actions")]
    public bool isStartButton = false;
    public bool isTryAgainButton = false;
    public bool isBackButton = false;
    public bool isQuitButton = false;
    public string backSceneName = "MainMenu";   // Scene for Back button
    public string startSceneName = "main";      // Scene for Start button

    private Vector3 originalScale;
    private bool isHovered = false;
    private Image buttonImage;
    private Color originalColor;

    void Awake()
    {
        originalScale = transform.localScale;
        buttonImage = GetComponent<Image>();

        if (buttonImage != null)
            originalColor = buttonImage.color;
    }

    void Update()
    {
        // Smooth scale effect
        Vector3 targetScale = isHovered ? originalScale * popScale : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * speed);

        // Smooth color glow effect
        if (buttonImage != null)
        {
            Color targetColor = isHovered ? originalColor * glowIntensity : originalColor;
            buttonImage.color = Color.Lerp(buttonImage.color, targetColor, Time.deltaTime * speed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }

    // Attach this to the Button's OnClick() event
    public void OnButtonClick()
    {
        // Unlock and show cursor when changing scenes or quitting
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (isStartButton)
        {
            SceneManager.LoadScene(startSceneName);
        }
        else if (isTryAgainButton)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (isBackButton)
        {
            SceneManager.LoadScene(backSceneName);
        }
        else if (isQuitButton)
        {
            Application.Quit();
            Debug.Log("Game Quit!");
        }
    }
}
