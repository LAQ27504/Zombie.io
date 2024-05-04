using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyingUI : MonoBehaviour
{
    public event EventHandler OnNextRoundButtonClicked;

    [SerializeField] private Button nextRoundButton;

    private void Awake()
    {
        gameObject.SetActive(false);

        nextRoundButton.onClick.AddListener(() =>
        {
            OnNextRoundButtonClicked?.Invoke(this, EventArgs.Empty);
        });
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
