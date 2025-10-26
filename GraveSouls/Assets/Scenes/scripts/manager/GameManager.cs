using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Player Health Settings")]
    public int maxHealth = 5;
    public int currentHealth;

    [Header("UI Hearts")]
    public RawImage[] hearts; // Assign your 5 RawImages in the inspector

    private void Start()
    {
        // Initialize player health
        currentHealth = maxHealth;
        UpdateHeartsUI();
    }

    /// <summary>
    /// Call this method to reduce player health by 1
    /// </summary>
    public void TakeDamage(int amount = 1)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHeartsUI();

        if (currentHealth <= 0)
        {
            Debug.Log("You lose!");
        }
    }

    /// <summary>
    /// Updates the hearts UI based on current health
    /// </summary>
    private void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
                hearts[i].enabled = true;  // Heart visible
            else
                hearts[i].enabled = false; // Heart removed
        }
    }

    /// <summary>
    /// Call this method to restore health (optional)
    /// </summary>
    public void Heal(int amount = 1)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHeartsUI();
    }
}
