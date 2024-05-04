using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const string GENERATE_STATE = "Generate";
    private const string PLAY_STATE = "Play";
    private const string BUY_STATE = "Buy";
    private const string GAMEOVER_STATE = "GameOver";
    private const string PAUSE_STATE = "Pause";
    private const string MAIN_MENU = "MainMenu";
    [SerializeField] private GenerateState generateState;    
    [SerializeField] private PauseState pasueState;    
    [SerializeField] private PlayState playState;    
    [SerializeField] private BuyState buyState;    
    [SerializeField] private GameOverState gameoverState;
    [SerializeField] private MainMenuState mainMenuState;

    private int roundLevel;

    private float chanceToSpawn;


    private Dictionary<String, BaseState> stateDictionary;

    public enum State
    {
        GENERATE,
        PLAY,
        BUY,
        PAUSE,
        GAME_OVER,  
    }

    private void Awake()
    {
        roundLevel = 1;

        chanceToSpawn = 0.9995f;

        stateDictionary = new Dictionary<String, BaseState>();
        stateDictionary.Add(GENERATE_STATE, generateState);
        stateDictionary.Add(PLAY_STATE, playState);
        stateDictionary.Add(BUY_STATE, buyState);
        stateDictionary.Add(GAMEOVER_STATE, gameoverState);
        stateDictionary.Add(PAUSE_STATE, pasueState);
        stateDictionary.Add(MAIN_MENU, mainMenuState);

        playState.OnEnterRound += PlayState_OnEnterRound;

        mainMenuState.OnEnterState += MainMenuState_OnEnterState;

        gameoverState.OnRestartClick += GameoverState_OnRestartClick;
    }

    private void GameoverState_OnRestartClick(object sender, EventArgs e)
    {
        ResetGameInfo();
    }

    private void MainMenuState_OnEnterState(object sender, EventArgs e)
    {
        ResetGameInfo();
    }

    private void PlayState_OnEnterRound(object sender, EventArgs e)
    {
        if (roundLevel == 1)
        {
            Enemy.Instance.SetStat(50f, 10f, 2.5f, 0.75f, 2f);
        }
        else
        {
            float statChangePerecentage = 1 + roundLevel * 0.01f;

            Enemy.Instance.AddStat(statChangePerecentage,statChangePerecentage, 1, 1, statChangePerecentage);

        }
        roundLevel += 1;
        chanceToSpawn -= 0.0004f;

        WalkerGenerator.Instance.ChangeLevelStat(chanceToSpawn, roundLevel);
    }



    public void Start()
    {
        StateMachine.SetStateMachine(stateDictionary);
        StateMachine.ChangeState(MAIN_MENU);
    }

    private void Update()
    {
        
    }

    public void ResetGameInfo()
    {
        roundLevel = 1;

        chanceToSpawn = 0.9995f;

        int mapInfo = 30;

        WalkerGenerator.Instance.ResetStat(chanceToSpawn, mapInfo);
    }
    
    

}
