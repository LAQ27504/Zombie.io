using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class UpgradeGunUI : MonoBehaviour 
{ 

    [SerializeField] protected Image upgradedImage;

    [SerializeField] protected Button button;

    [SerializeField] protected TextMeshProUGUI costsText;

    protected int currentUpgrade;
    protected float costs;

    protected abstract void UpdradeGun();

    protected void UpdateCost()
    {
        costs = costs * 1.25f + currentUpgrade * 0.1f;
        costsText.text = ((int)Mathf.Ceil(costs)).ToString();
    }

    protected void UpdateImageVisual()
    {

        currentUpgrade += 1;

        upgradedImage.fillAmount = (float)currentUpgrade / 6f;
    }

    protected bool CanUpgrade()
    {
        if (currentUpgrade < 6)
        {
            return true;
        }
        return false;
    }
}
