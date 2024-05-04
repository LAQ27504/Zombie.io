using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public event EventHandler OnRestartButton;
    public event EventHandler OnHomeClicked;

    [SerializeField] private Button homeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private TextMeshProUGUI score;

    private void Awake()
    {
        homeButton.onClick.AddListener(() =>
        {
            OnHomeClicked?.Invoke(this, EventArgs.Empty);
        });
        restartButton.onClick.AddListener(() =>
        {
            OnRestartButton?.Invoke(this, EventArgs.Empty);
        });
        Hide();
    }

    public void ShowScore(int score)
    {
        this.score.text = "Your score: " + score.ToString();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject?.SetActive(false);
    }
}
