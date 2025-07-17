using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public GameObject deathEffect;
    public GameObject hitEffect;
    public GameObject crosshair;
    public GameObject wand;
    public PauseMenu pauseMenu;
    public GameObject gameOverMenu;

    private bool isDead = false;

    // Optional: damage cooldown to prevent rapid hits
    public float damageCooldown = 1f;
    private float damageCooldownTimer = 0f;

    private void Start()
    {
        currentHealth = maxHealth;
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
        damageCooldownTimer = damageCooldown; // reset cooldown timer

        Debug.Log($"Player took {amount} damage, health now: {currentHealth}");

        if (currentHealth <= 0f)
        {
            Die();
        }
        //else
        //{
        //    Instantiate(hitEffect, transform.position,Quaternion.identity);
        //}
    }

    public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            pauseMenu.isGameOver = true;
            Instantiate(deathEffect, transform.position + Vector3.up * 1f, Quaternion.identity);
            Debug.Log("Player has died.");

            GetComponent<PlayerMovement>().enabled = false;
            wand.SetActive(false);
            crosshair.SetActive(false);
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

}
