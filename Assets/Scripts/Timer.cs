using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private float elapsedTime = 0f;
    public TMP_Text timerText;
    public GameObject timerUI;
    public bool isRunning = false;

    private void Start()
    {
        if (timerUI != null)
        {
            timerUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            timerText.text = FormatTime(elapsedTime);
        }
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        float seconds = time % 60f;
        return string.Format("{0:0}:{1:0.0}", minutes, seconds);
    }

    public void StartTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
        
        if (timerUI != null)
        {
            timerUI.SetActive(true);
        }
    }

    public void StopTimer()
    {
        elapsedTime = 0f;
        isRunning = false;
        
        if (timerUI != null)
        {
            timerUI.SetActive(false);
        }
    }
}
