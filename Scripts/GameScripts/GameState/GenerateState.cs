using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateState : BaseState
{
    [SerializeField] private LoadingUI loadingUI;

    [SerializeField] private WalkerGenerator walkerGenerator;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [SerializeField] private Player player;

    [SerializeField] private PlayScreenUI screenUI;

    private int wallMax;
    private int wallCurrent;

    private bool destroyState;

    private void Awake()
    {
        gameObject.SetActive(false);

        screenUI.gameObject.SetActive(false);

        destroyState = false;

        wallMax = 0;

        wallCurrent = 0;


        walkerGenerator.OnDoneCountWallMax += WalkerGenerator_OnDoneCountWallMax;
        walkerGenerator.OnWallChange += WalkerGenerator_OnWallChange;
    }

    private void WalkerGenerator_OnWallChange(object sender, System.EventArgs e)
    {
        wallCurrent++;
    }

    private void WalkerGenerator_OnDoneCountWallMax(object sender, WalkerGenerator.OnDoneCountWallMaxArgs e)
    {
        wallMax = e.countWallMax;
    }

    public override void GetNextStates(string state)
    {
        if (state == PLAY_STATE || state == PAUSE_STATE)
        {
            destroyState = false;
        }
        else
        {
            destroyState = true;
        }
    }

    public override void EnterState()
    {
        gameObject.SetActive(true);

        wallMax = 0;

        wallCurrent = 0;

        walkerGenerator.InitializeGrid();

        player.transform.position = walkerGenerator.CenteMap();

        player.GetComponent<PlayerPauseControl>().PausePlayer();

        loadingUI.Show();

        player.transform.GetChild(1).GetComponent<Gun>().SetUpGun();

        screenUI.SetUp(player, player.transform.GetChild(1).GetComponent<Gun>());
    }

    public override void ExitState()
    {
        if (destroyState)
        {

        }
        gameObject.SetActive(false);
    }

    public override void Update()
    {
        if (isDoneMapGenerate())
        {
            StateMachine.ChangeState(PLAY_STATE);
        }
    }

    public override void Render()
    {
        
    }

    private bool isDoneMapGenerate()
    {
        if (wallCurrent < wallMax || wallMax == 0)
        {
            return false;
        }

        loadingUI.Hide();

        return true;
    }
}
