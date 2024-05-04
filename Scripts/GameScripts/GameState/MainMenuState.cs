using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuState : BaseState
{
    public event EventHandler OnEnterState;

    [SerializeField] private MainMenuGame game;
    [SerializeField] private Transform tutorialUI;
    [SerializeField] private Button startButton;

    private void Awake()
    {
        gameObject.SetActive(false);
        tutorialUI.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
    }

    public override void GetNextStates(string state)
    {

    }

    public override void EnterState()
    {
        gameObject.SetActive(true);

        OnEnterState?.Invoke(this, EventArgs.Empty);

        game.Show();

        Player.Instance.GetComponent<PlayerPauseControl>().PausePlayer();

        game.OnPlayClicked += Game_OnPlayClicked;
    }

    private void Game_OnPlayClicked(object sender, System.EventArgs e)
    {
        game.Hide();
        tutorialUI.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);
        startButton.onClick.AddListener(() =>
        {
            StateMachine.ChangeState(GENERATE_STATE);
        });
    }



    public override void ExitState()
    {
        tutorialUI.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        game.OnPlayClicked -= Game_OnPlayClicked;
        game.Hide();
        gameObject.SetActive(false);
    }

    public override void Update()
    {

    }

    public override void Render()
    {

    }
}
