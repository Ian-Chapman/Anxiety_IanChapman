using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseButton;
    public GameObject itemPanel;
    private bool isPaused = false;
    public AudioSource audioSource;

    public void Start()
    {
        pauseMenu.SetActive(false);
    }

    public void OnPlayButtonPressed()
    {
        SceneManager.LoadScene("GameJam");
    }

    public void OnInstructionsButtonPressed()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }

    public void OnPauseButtonPressed()
    {

        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        itemPanel.SetActive(false);
        pauseButton.SetActive(false);
        audioSource.Pause();
    }

    public void OnRestartButtonPressed()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GameJam");
        pauseButton.SetActive(true);
        itemPanel.SetActive(true);
        pauseMenu.SetActive(false);
    }


    public void OnResumeButtonPressed()
    {
        Time.timeScale = 1;
        pauseButton.SetActive(true);
        itemPanel.SetActive(true);
        pauseMenu.SetActive(false);
        audioSource.Play();
    }

    public void OnQuitToMainMenuButtonPressed()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
