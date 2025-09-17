using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int score;
    private int highScore;
    
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
        highScore = PlayerPrefs.GetInt(GameEvents.HIGH_SCORE, 0);
    }
    
    //TODO do something with this
    private void GameOver()
    {
        
    }

    public void IncreaseCollectibleScore()
    {
        score++;

        if (score <= highScore) return;
        
        highScore = score;
        PlayerPrefs.SetInt(GameEvents.HIGH_SCORE, highScore);
    }
}