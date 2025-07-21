using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("References")]
    public GameObject deathEffect;
    public GameObject hitEffect;
    public GameObject crosshair;
    public GameObject wand;
    public PauseMenu pauseMenu;
    public GameObject gameOverMenu;
    public GameObject healthBar;

    private TeleportController teleportController;
    private bool isDead = false;

    [Header("Damage Settings")]
    public float damageCooldown = 1f;
    private float damageCooldownTimer = 0f;

    private HealthSystemForDummies healthSystem;

    private void Start()
    {
        teleportController = GetComponent<TeleportController>();
        healthSystem = GetComponent<HealthSystemForDummies>();

        if (teleportController == null)
            Debug.LogWarning("TeleportController not found!");
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

        healthSystem.AddToCurrentHealth(-amount);
        damageCooldownTimer = damageCooldown;

        if (healthSystem.CurrentHealth <= 0f)
        {
            Die();
        }
        else if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        pauseMenu.isGameOver = true;

        teleportController?.DisableTeleport();

        if (deathEffect != null)
            Instantiate(deathEffect, transform.position + Vector3.up * 1f, Quaternion.identity);

        GetComponent<PlayerMovement>().enabled = false;
        wand.SetActive(false);
        crosshair.SetActive(false);
        healthBar.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameOverMenu.SetActive(true);
    }

    public void RefillHealth()
    {
        healthSystem.ReviveWithMaximumHealth(); 
    }

    public float GetCurrentHealth() => healthSystem.CurrentHealth;
    public float GetHealthPercentage() => healthSystem.CurrentHealthPercentage;
    public bool IsPlayerDead() => isDead;

    public void Revive()
    {
        if (!isDead) return;
        isDead = false;
        healthSystem.ReviveWithMaximumHealth();
        pauseMenu.isGameOver = false;

        teleportController?.EnableTeleport();
        GetComponent<PlayerMovement>().enabled = true;
        wand.SetActive(true);
        crosshair.SetActive(true);
        healthBar.SetActive(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        gameOverMenu.SetActive(false);

    }
}
