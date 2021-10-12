/*
Copyright.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    public enum UpOrDown { up, down }
    public UnityEngine.UI.Text timerText;
    public UpOrDown TimerUpOrDown = UpOrDown.down;
    public int secondsWhenGameEnds = 0;
    System.DateTime startTime;
    System.DateTime stopTime;
    bool timerRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        startTime = System.DateTime.Now;
        stopTime = System.DateTime.Now;
        timerRunning = true;

    }

    public void TimeIsUp()
    {

    }

    public void StartTimer()
    {
        startTime = System.DateTime.Now;
        timerRunning = true;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public void ResetTimer()
    {
        startTime = System.DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        System.TimeSpan gameRunTimeSpan = stopTime - startTime;
        double gameRunSeconds = (stopTime - startTime).TotalSeconds;
        if (timerRunning)
        {
            if(TimerUpOrDown == UpOrDown.down && gameRunSeconds != 0 && secondsWhenGameEnds - gameRunSeconds < 0)
            {
                gameRunSeconds = secondsWhenGameEnds;
                timerRunning = false;
                TimeIsUp();
            }
            if (TimerUpOrDown == UpOrDown.up && gameRunSeconds != 0 && secondsWhenGameEnds - gameRunSeconds < 0)
            {
                gameRunSeconds = secondsWhenGameEnds;
                timerRunning = false;
                TimeIsUp();
            }
            else
                stopTime = System.DateTime.Now;
        }

        if (timerText != null)
        {
            
            if (TimerUpOrDown== UpOrDown.up)
                timerText.text = "Clock : "+ gameRunTimeSpan.ToString("mm\\:ss\\.ff");
            else
            {
                int sec = (secondsWhenGameEnds - (int)gameRunSeconds);
                int minutes = (int)((secondsWhenGameEnds - (int)gameRunSeconds)/60);
                int ms = 1000 * sec;
                string tsOut = System.String.Format("{0}:{1:D2}", minutes, sec, ms);
                timerText.text = "Timer : " + tsOut;
                //timerText.text = "Timer : "+(secondsWhenGameEnds - gameRunSeconds).ToString("mm\\:ss\\.ff");
            }
        }
    }
}
