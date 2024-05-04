using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "isWalking";
    private const string IS_RUNNING = "isRunning";
    private const string GRAB_ITEM = "GrabItem";
    private const string EQUIP_GUN = "EquipGun";
    private const string UNEQUIPE_GUN = "UnequipGun";
    private const string IS_EQUIPED = "isEquiped";

    [SerializeField] private PlayerMovement playerMovement;

    private Player player;

    [SerializeField]
    private Animator animator;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerMovement.OnInteraction += PlayerMovement_OnInteraction;
        playerMovement.OnEquipedWeapon += PlayerMovement_OnEquipedWeapon;
        
    }

    private void PlayerMovement_OnEquipedWeapon(object sender, PlayerMovement.OnEquipedWeaponArgs e)
    {
        if (e.isEquiped)
        {
            animator.Play(EQUIP_GUN);
            animator.SetBool(IS_EQUIPED, e.isEquiped);
        }
        else
        {
            animator.SetBool(IS_EQUIPED, e.isEquiped);
        }
    }

    private void PlayerMovement_OnInteraction(object sender, System.EventArgs e)
    {
        animator.Play(GRAB_ITEM);
    }

    private void Update()
    {
        animator.SetBool(IS_WALKING, player.IsWalking());
        animator.SetBool(IS_RUNNING, player.IsRunning());
    }

    

}
