using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public event Action<float> OnTimeChanged;
    public event Action<float> OnScoreIncreased;
    
    public static GameManager Instance { get; private set; }

    private int score;
    private int highScore;
    private bool isGameOver;
    private float timer;
    
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isGameOver) return;
        
        timer += Time.deltaTime;
        OnTimeChanged?.Invoke(timer);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; 
        FollowPlayer.OnPlayerCaught += GameOver;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; 
        FollowPlayer.OnPlayerCaught -= GameOver;
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
    
    //TODO do something with this
    private void GameOver()
    {
        isGameOver = true;
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
}