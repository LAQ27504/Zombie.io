using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private Image Loadingbar;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Loading(float percentage)
    {
        Loadingbar.fillAmount = percentage;
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
