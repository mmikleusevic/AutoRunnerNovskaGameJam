using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    // public GameObject pauseMenuPanel;
    // public GameObject gameOverPanel;
    // public GameObject creditsPanel;
    
    public Slider volumeSlider;

    private void Start()
    {
        ShowMainMenu();
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        SoundManager.Instance.SetVolume(volumeSlider.value);
    }
    public void StartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        //pauseMenuPanel.SetActive(false);
    }
    public void BackFromOptions()
    {
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void SetVolume(float volume)
    {
        volumeSlider.value = volume;
        PlayerPrefs.SetFloat("Volume", volume);
        SoundManager.Instance.SetVolume(volumeSlider.value);
    }

    public void TogglePauseMenu()
    {
        bool isPaused = Time.timeScale == 0f;
        if (isPaused)
        {
            Time.timeScale = 1f;
            //pauseMenuPanel.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            //pauseMenuPanel.SetActive(true);
            optionsPanel.SetActive(false);
        }
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        //pauseMenuPanel.SetActive(false);
    }
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
        //pauseMenuPanel.SetActive(false);
    }

    public void ShowEndGamePanel()
    {
        Time.timeScale = 0f;
        
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        //pauseMenuPanel.SetActive(false);
        //creditsPanel.SetActive(false);
        //gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        //gameOverPanel.SetActive(false);
        StartGame();
    }
}
