using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayScreenUI: MonoBehaviour
{
    [SerializeField] private AmmoUI ammoUI;

    [SerializeField] private Image healthBar;

    [SerializeField] private Image energyBar;

    private Gun playerGun;

    private Player player;

    public void SetUp(Player player, Gun gun)
    {
        this.player = player;
        this.playerGun = gun;

        ammoUI.GetGun(gun);
    }

    public void UpdateVisual()
    {
        if (player != null)
        {
            energyBar.fillAmount = player.EnergyPercentage();
            healthBar.fillAmount = player.HealthPercentage();
        }
    }

    private void Update()
    {
        UpdateVisual();
    }

}
