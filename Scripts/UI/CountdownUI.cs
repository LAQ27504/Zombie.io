using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountdownUI : MonoBehaviour
{
    public static CountdownUI Instance;

    public event EventHandler OnDoneCountdown;

    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI countdownText;

    

    private static float countdownTimer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }

    private void Update()
    {
        Countdown();   
    }

    private void Countdown()
    {
        if (countdownTimer > 0)
        {
            countdownTimer -= Time.deltaTime;
            countdownText.text = Mathf.Ceil(countdownTimer).ToString();
        }
        else
        {
            EndCountdown();
        }
    }

    public void StartCountdown()
    {
        gameObject.SetActive(true);
        countdownTimer = 3f;
    }

    public void EndCountdown()
    {
        OnDoneCountdown?.Invoke(this, EventArgs.Empty);
        gameObject.SetActive(false);
    }

}
