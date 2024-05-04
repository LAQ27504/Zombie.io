using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerPauseControl : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Player player;

    private void Awake()
    {
        playerMovement = GetComponentInChildren<PlayerMovement>();
        player = GetComponent<Player>();
    }

    public void PausePlayer()
    {
        playerMovement.SetDisable();
        player.canRotate = false;
    }

    public void UnPausePlayer()
    {
        playerMovement.SetEnable();
        player.canRotate = true;
    }
}
