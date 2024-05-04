using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public event EventHandler OnUnpauseClicked;
    public event EventHandler OnHomeClicked;

    [SerializeField] private Button homeButton;
    [SerializeField] private Button unpauseButton;

    private void Awake()
    {
        homeButton.onClick.AddListener(() =>
        {
            OnHomeClicked?.Invoke(this, EventArgs.Empty);
        });
        unpauseButton.onClick.AddListener(() =>
        {
            OnUnpauseClicked?.Invoke(this, EventArgs.Empty);
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
