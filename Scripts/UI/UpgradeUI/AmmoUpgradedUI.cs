using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoUpgradeUI : UpgradeGunUI
{
    public static AmmoUpgradeUI Instance;

    public event EventHandler OnAmmoUpgraded;
    private void Awake()
    {

        Instance = this;

        currentUpgrade = 0;

        upgradedImage.fillAmount = 0;

        button.onClick.AddListener(UpdradeGun);
        costs = 1;
        costsText.text = ((int)costs).ToString();
    }
    protected override void UpdradeGun()
    {
        if (BonesCountUI.Instance.CanDecreaseBone((int)Mathf.Ceil(costs), CanUpgrade()))
        {

            UpdateCost();
            UpdateImageVisual();
            OnAmmoUpgraded?.Invoke(this, EventArgs.Empty);
            if (!CanUpgrade())
            {
                costsText.text = "MAX";
            }
        }
        else if (!CanUpgrade())
        {
            costsText.text = "MAX";
        }
    }

  
}
