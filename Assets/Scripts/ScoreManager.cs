using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int ribbonsCollected = 0;

    public static ScoreManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddRibbons()
    {
        ribbonsCollected++;
        Debug.Log($"Ribbons collected: {ribbonsCollected}");
    }

    public void ResetScore()
    {
        ribbonsCollected = 0;
    }
}
