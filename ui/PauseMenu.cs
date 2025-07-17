using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Settings")]
    private bool isPaused = false;
    public bool isGameOver = false;

    [Header("UI References")]
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public GameObject crosshair;

    [Header("Player References")]
    public GameObject player;
    public GameObject wand;

    private PlayerMovement playerMovement;
    private WandControl wandControl;
    private TeleportController teleportController;

    private void Awake()
    {
        // Cache component references
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
            teleportController = player.GetComponent<TeleportController>();
        }

        if (wand != null)
        {
            wandControl = wand.GetComponent<WandControl>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            // Don't pause if settings menu is open
            if (settingsMenuUI.activeSelf)
            {
                return;
            }

            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;

        // Disable player controls
        if (playerMovement != null)
            playerMovement.enabled = false;
        if (wandControl != null)
            wandControl.enabled = false;
        if (teleportController != null)
            teleportController.DisableTeleport();

        // Show pause UI
        pauseMenuUI.SetActive(true);
        crosshair.SetActive(false);

        // Show cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;

        // Re-enable player controls
        if (playerMovement != null)
            playerMovement.enabled = true;
        if (wandControl != null)
            wandControl.enabled = true;
        if (teleportController != null)
            teleportController.EnableTeleport();

        // Hide pause UI
        pauseMenuUI.SetActive(false);
        crosshair.SetActive(true);

        // Hide cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("ui main");
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
    }
}