using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject pauseMenuPanel;
    public bool isGameStarted = false;
    public Slider volumeSlider;

    private void Start()
    {
        ShowMainMenu();
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
    }

    private void Update()
    {
        if (isGameStarted && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        isGameStarted = true;
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log($"Game quit");
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
    }
    public void BackFromOptions()
    {
        optionsPanel.SetActive(false);
        if (isGameStarted)
            pauseMenuPanel.SetActive(true);
        else
            mainMenuPanel.SetActive(true);
    }

    public void SetVolume(float volume)
    {
        volumeSlider.value = volume;
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void TogglePauseMenu()
    {
        bool isPaused = Time.timeScale == 0f;
        if (isPaused)
        {
            Time.timeScale = 1f;
            pauseMenuPanel.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            pauseMenuPanel.SetActive(true);
            optionsPanel.SetActive(false);
        }
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);
    }
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        isGameStarted = false;
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
    }
}
