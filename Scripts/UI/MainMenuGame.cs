using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuGame : MonoBehaviour
{
    public event EventHandler OnPlayClicked;

    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            OnPlayClicked?.Invoke(this, EventArgs.Empty);
        });
        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        Hide();
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
