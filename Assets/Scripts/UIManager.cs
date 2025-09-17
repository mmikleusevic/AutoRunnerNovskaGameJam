using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject pauseMenuPanel;
    public GameObject gameOverPanel;
    public GameObject creditsPanel;
    
    public bool isGameStarted = false;
    
    public Slider volumeSlider;
    
    public TMP_Text endScoreText;
    public TMP_Text endTimeText;

    public Timer timer;
    public ScoreManager scoreManager;
    
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
        gameOverPanel.SetActive(false);
        creditsPanel.SetActive(false);
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

    public void ShowEndGamePanel()
    {
        Time.timeScale = 0f;
        isGameStarted = false;
        
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        creditsPanel.SetActive(false);
        
        gameOverPanel.SetActive(true);

        if (timer != null)
        {
            float time = timer.elapsedTime;
            int minutes= Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            endTimeText.text = $"Your time: {minutes:0}:{seconds:00}";
        }
        else
        {
            endTimeText.text = "N/A";
        }

        if (scoreManager != null)
        {
            endScoreText.text = $"Ribbons collected: {scoreManager.ribbonsCollected}";
        }
        else
        {
            endScoreText.text = "0";
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        gameOverPanel.SetActive(false);
        StartGame();

        if (timer != null)
        {
            timer.RestartTimer();
        }

        if (scoreManager != null)
        {
            scoreManager.ResetScore();
        }
    }

    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    public void BackFromCredits()
    {
        creditsPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }
}
