using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public GameObject crosshair;
    public GameObject player;
    public GameObject wand;
    public bool isGameOver = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
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
        player.GetComponent<PlayerMovement>().enabled = false;
        wand.GetComponent<WandControl>().enabled = false;
        pauseMenuUI.SetActive(true);
        crosshair.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

     public void Resume()
        {
            Time.timeScale = 1f;
            isPaused = false;
            player.GetComponent<PlayerMovement>().enabled = true;
            wand.GetComponent<WandControl>().enabled = true;
            pauseMenuUI.SetActive(false);
            crosshair.SetActive(true);

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
