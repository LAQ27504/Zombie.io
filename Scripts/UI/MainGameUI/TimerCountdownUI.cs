using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerCountdownUI : MonoBehaviour
{
    public event EventHandler OnDoneCountdown;

    [SerializeField] private Image countdownImage;

    private float timer;
    private float timerMax;

    private bool timerRunning;

    private void Awake()
    {
        timerRunning = false;
        countdownImage.fillAmount = 0f;
    }

    public void SetUp(float timer)
    {
        timerRunning = true;
        this.timer = 0f;
        timerMax = timer;
    }

    public void StopTimer()
    {
        timerRunning = !timerRunning;
    }

    private void Update()
    {
        if (timer < timerMax && timerMax != 0 && timerRunning)
        {
            timer += Time.deltaTime;
            countdownImage.fillAmount = timer / timerMax;

        }
        else if (timer >= timerMax && timerMax != 0 && timerRunning)
        {
            OnDoneCountdown?.Invoke(this, EventArgs.Empty);
        }
    }

}
