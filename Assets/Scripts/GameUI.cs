using System;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;

    private void Start()
    {
        timerText.text = "00:00";
    }

    private void OnEnable()
    {
        GameManager.Instance.OnScoreIncreased += SetScore;
        GameManager.Instance.OnTimeChanged += SetTime;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnScoreIncreased -= SetScore;
        GameManager.Instance.OnTimeChanged -= SetTime;
    }

    private void SetScore(float score)
    {
        scoreText.text = score.ToString();
    }
    
    private void SetTime(float time)
    {
        string timeText = FormatTime(time);
        timerText.text = timeText;
    }
    
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
}
