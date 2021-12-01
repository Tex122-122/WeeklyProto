using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerCountdown : MonoBehaviour
{
    public string LevelToLoad;
    private float timer = 60;
    private Text timerSeconds;

    void Start()
    {
        timerSeconds = GetComponent<Text>();  
    }

    void Update()
    {
        timer -= Time.deltaTime;
        timerSeconds.text = timer.ToString("f2");
        if (timer <= 0)
        {
            Application.LoadLevel(LevelToLoad);
        }
    }
}
