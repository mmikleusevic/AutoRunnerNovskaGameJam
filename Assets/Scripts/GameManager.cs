using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public event Action<float> OnTimeChanged;
    public event Action<float> OnScoreIncreased;
    
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI scoreTextLose;
    [SerializeField] private TextMeshProUGUI timeTextLose;
    [SerializeField] private TextMeshProUGUI scoreTextWin;
    [SerializeField] private TextMeshProUGUI timeTextWin;
    
    private int score;
    private int highScore;
    private bool isGameOver;
    private float timer;
    private bool isGamePaused;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        SoundManager.Instance.SetVolume(volumeSlider.value);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        
        if (isGameOver) return;
        
        timer += Time.deltaTime;
        OnTimeChanged?.Invoke(timer);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; 
        FollowPlayer.OnPlayerCaught += GameOver;
        FinishLine.OnFinish += Win;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; 
        FollowPlayer.OnPlayerCaught -= GameOver;
        FinishLine.OnFinish -= Win;
    }

    public void TogglePause()
    {
        Time.timeScale = isGamePaused ? 1 : 0;
        isGamePaused = !isGamePaused;
        pausePanel.SetActive(isGamePaused);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Game") return;
        
        score = 0;
        timer = 0;
        isGameOver = false;
        
        CallOnScoreIncreased();
        
        highScore = PlayerPrefs.GetInt(GameEvents.HIGH_SCORE, 0);
    }
    
    private void GameOver()
    {
        Time.timeScale = 0;

        SetLoseText();
        
        isGameOver = true;
        losePanel.SetActive(true);
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0;

        SetLoseText();
        
        isGameOver = true;
        losePanel.SetActive(true);
    }

    private void SetLoseText()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        
        scoreTextLose.text = "Skupljene vrpce: " + score;
        timeTextLose.text = "Tvoje vrijeme: " + $"{minutes:00}:{seconds:00}";
    }

    private void SetWinText()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        
        scoreTextWin.text = "Skupljene vrpce: " + score;
        timeTextWin.text = "Tvoje vrijeme: " + $"{minutes:00}:{seconds:00}";
    }

    private void Win()
    {
        StartCoroutine(WinCoroutine());
    }

    private IEnumerator WinCoroutine()
    {
        yield return new WaitForSeconds(3);
        
        isGameOver = true;
        winPanel.SetActive(true);
        SetWinText();
    }

    public void IncreaseCollectibleScore()
    {
        score++;

        CallOnScoreIncreased();

        if (score <= highScore) return;
        
        highScore = score;
        PlayerPrefs.SetInt(GameEvents.HIGH_SCORE, highScore);
    }
    
    private void CallOnScoreIncreased()
    {
        OnScoreIncreased?.Invoke(score);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    
    public void SetVolume(float volume)
    {
        volumeSlider.value = volume;
        PlayerPrefs.SetFloat("Volume", volume);
        SoundManager.Instance.SetVolume(volumeSlider.value);
    }
    
    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
        winPanel.SetActive(false);
    }
    
    public void BackFromCredits()
    {
        creditsPanel.SetActive(false);
        winPanel.SetActive(true);
    }
}