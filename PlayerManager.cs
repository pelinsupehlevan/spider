using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("References")]
    public GameObject deathEffect;
    public GameObject hitEffect;
    public GameObject crosshair;
    public GameObject wand;
    public PauseMenu pauseMenu;
    public GameObject gameOverMenu;

    [Header("Components")]
    private TeleportController teleportController;

    private bool isDead = false;

    [Header("Damage Settings")]
    public float damageCooldown = 1f;
    private float damageCooldownTimer = 0f;

    private void Start()
    {
        currentHealth = maxHealth;
        teleportController = GetComponent<TeleportController>();

        if (teleportController == null)
        {
            Debug.LogWarning("TeleportController not found on player!");
        }
    }

    private void Update()
    {
        if (damageCooldownTimer > 0f)
            damageCooldownTimer -= Time.deltaTime;
    }

    public void TakeDamage(float amount)
    {
        if (isDead || damageCooldownTimer > 0f)
            return;

        currentHealth -= amount;
        damageCooldownTimer = damageCooldown;

        Debug.Log($"Player took {amount} damage, health now: {currentHealth}");

        if (currentHealth <= 0f)
        {
            Die();
        }
        else
        {
            // Optional: Show hit effect
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }
        }
    }

    public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            pauseMenu.isGameOver = true;

            // Disable teleport when dead
            if (teleportController != null)
            {
                teleportController.DisableTeleport();
            }

            // Show death effect
            if (deathEffect != null)
            {
                Instantiate(deathEffect, transform.position + Vector3.up * 1f, Quaternion.identity);
            }

            Debug.Log("Player has died.");

            // Disable player components
            GetComponent<PlayerMovement>().enabled = false;
            wand.SetActive(false);
            crosshair.SetActive(false);

            // Show cursor and game over menu
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            gameOverMenu.SetActive(true);
        }
    }

    public void RefillHealth()
    {
        currentHealth = maxHealth;
        Debug.Log($"Player health refilled to {currentHealth}");
    }

    // Public method to get current health (useful for UI)
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    // Public method to get health percentage (useful for UI)
    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }

    // Public method to check if player is dead
    public bool IsPlayerDead()
    {
        return isDead;
    }

    // Method to revive player (if needed for respawn system)
    public void Revive()
    {
        if (isDead)
        {
            isDead = false;
            currentHealth = maxHealth;
            pauseMenu.isGameOver = false;

            // Re-enable teleport
            if (teleportController != null)
            {
                teleportController.EnableTeleport();
            }

            // Re-enable player components
            GetComponent<PlayerMovement>().enabled = true;
            wand.SetActive(true);
            crosshair.SetActive(true);

            // Hide cursor and game over menu
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            gameOverMenu.SetActive(false);

            Debug.Log("Player has been revived.");
        }
    }
}