using UnityEngine;

public class Enemy1: MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] GameObject[] heart;  // Array to hold heart GameObjects
    public int maxHealth = 3;    // Maximum health

    private int currentHealth;   // Current health of the enemy

    void Start()
    {
        currentHealth = maxHealth; // Set initial health to maximum
        UpdateHealthUI();          // Update the hearts in the UI
    }

    // Call this method to reduce enemy health
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Clamp the health to avoid negative values
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die(); // Trigger death logic
        }

        UpdateHealthUI(); // Update the UI
    }

    // Updates the hearts based on current health
    void UpdateHealthUI()
    {
        for (int i = 0; i < heart.Length; i++)
        {
            if (i < currentHealth)
            {
                heart[i].SetActive(true); // Show hearts for remaining health
            }
            else
            {
                heart[i].SetActive(false); // Hide hearts for lost health
            }
        }
    }

    // Logic for when the enemy dies
    void Die()
    {
        Debug.Log("Enemy has died!");
        Destroy(gameObject); // Destroy the enemy GameObject
    }
}