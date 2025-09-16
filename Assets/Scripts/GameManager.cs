using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
        FollowPlayer.OnPlayerCaught += GameOver;
    }

    private void OnDisable()
    {
        FollowPlayer.OnPlayerCaught -= GameOver;
    }

    
    //TODO do something with this
    private void GameOver()
    {
        
    }
}