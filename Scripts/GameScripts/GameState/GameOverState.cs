using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : BaseState
{
    public event EventHandler OnRestartClick;

    [SerializeField] private PlayScreenUI playScreenUI;
    [SerializeField] private GameOverUI gameOverUI;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public override void GetNextStates(string state)
    {

    }

    public override void EnterState()
    {
        gameObject.SetActive(true);


        gameOverUI.Show();

        gameOverUI.OnRestartButton += GameOverUI_OnRestartButton;

        gameOverUI.OnHomeClicked += GameOverUI_OnHomeClicked;

        gameOverUI.ShowScore(ScoreTextUI.Instance.GetScore());

        playScreenUI.gameObject.SetActive(false);

        Gun.Instance.SetBack();

        Player.Instance.RecoverStat();



        BonesCountUI.Instance.ResetBones();
    }

    private void GameOverUI_OnHomeClicked(object sender, System.EventArgs e)
    {
        ScoreTextUI.Instance.ResetScore();
        WalkerGenerator.Instance.ClearMap();
        StateMachine.ChangeState(MAIN_MENU);
    }

    private void GameOverUI_OnRestartButton(object sender, System.EventArgs e)
    {
        OnRestartClick?.Invoke(this, EventArgs.Empty);
        ScoreTextUI.Instance.ResetScore();
        WalkerGenerator.Instance.ClearMap();
        StateMachine.ChangeState(GENERATE_STATE);
    }


    public override void ExitState()
    {
        gameOverUI.OnRestartButton -= GameOverUI_OnRestartButton;
        gameOverUI.Hide();
        gameObject.SetActive(false);
    }

    public override void Update()
    {
        
    }

    public override void Render()
    {

    }
}
