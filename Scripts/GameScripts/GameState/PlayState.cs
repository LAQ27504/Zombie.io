using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;

public class PlayState : BaseState
{
    public static PlayState Instance;

    public event EventHandler OnEnterRound;
    public event EventHandler OnDoneSpawnEnemy;

    [SerializeField] private PlayScreenUI screenUI;
    [SerializeField] private EnemySpawn enemySpawn;
    [SerializeField] private Transform enemyTransform;
    [SerializeField] private TimerCountdownUI timerCountdownUI;
    [SerializeField] private Transform enemyHolder;
 
    private int enemyCount;

    private Player player;

    private bool isDoneCountdown;

    private bool isWave;

    private int enemyCountMax = 0;
    private void Awake()
    {
        enemyCount = 0;

        Instance = this;
        gameObject.SetActive(false);
    }

    private void Instance_OnKilled(object sender, Enemy.OnKilledArgs e)
    {
        enemyCount--;
    }

    public override void GetNextStates(string state)
    {

    }

    public override void EnterState()
    {
        timerCountdownUI.OnDoneCountdown += TimerCountdownUI_OnDoneCountdown;

        Time.timeScale = 1f;

        gameObject.SetActive(true);

        player = Player.Instance;


        player.OnDead += Player_OnDead;

        screenUI.gameObject.SetActive(true);

        timerCountdownUI.SetUp(0);

        CountdownUI.Instance.OnDoneCountdown += Instance_OnDoneCountdown;

        if (enemyHolder.childCount <= 0)
        {
            enemyCount = 0;
            isWave = false;
        }

        StartCountdown();
    }

    private void TimerCountdownUI_OnDoneCountdown(object sender, System.EventArgs e)
    {
        DeleteAllEnemies();
        StateMachine.ChangeState(GAMEOVER_STATE);
    }

    private void Player_OnDead(object sender, System.EventArgs e)
    {
        DeleteAllEnemies();
        StateMachine.ChangeState(GAMEOVER_STATE);
    }

    private void Instance_OnDoneCountdown(object sender, System.EventArgs e)
    {
        player.GetComponent<PlayerPauseControl>().UnPausePlayer();
        if (Enemy.Instance != null)
        {   
            Enemy.Instance.UnPauseEnemy();
        }
        isDoneCountdown = true;
    }

    public override void ExitState()
    {
        timerCountdownUI.OnDoneCountdown -= TimerCountdownUI_OnDoneCountdown;

        CountdownUI.Instance.OnDoneCountdown -= Instance_OnDoneCountdown;

        gameObject.SetActive(false);
    }

    public override void Update()
    {

        if (Input.GetKey(KeyCode.Escape))
        {
            
            StateMachine.ChangeState(PAUSE_STATE);
        }

        if (isDoneCountdown)
        {
            isDoneCountdown = false;
            if (!isWave)
            {
                SpawnEnemyWave();
            }
        }
        IsDoneWave();
    }

    public override void Render()
    {

    }

    private void SpawnEnemyWave()
    {
        isWave = true;

        foreach (Vector3 spawnLocation in enemySpawn.spawnPointList)
        {
            Transform enemyNewTransform = Instantiate(enemyTransform, spawnLocation, Quaternion.identity, enemyHolder);

            enemyCount++;
        }

        OnEnterRound?.Invoke(this, EventArgs.Empty);

        OnDoneSpawnEnemy?.Invoke(this, EventArgs.Empty);

        enemyCountMax = enemyCount;

        float timerForEachEnemy = 15;

        timerCountdownUI.SetUp(enemyCountMax * timerForEachEnemy);

        ScoreTextUI.Instance.SetUp();

        BonesCountUI.Instance.SetUp();

        Enemy.OnKilled += Instance_OnKilled;
    }

    private void DeleteAllEnemies()
    {
        foreach (Enemy enemy in enemyHolder.GetComponentsInChildren<Enemy>())
        {
            Debug.Log(enemy);
            Destroy(enemy.gameObject);
        }
    }

    private void IsDoneWave()
    {
        if (enemyCount <= 0)
        {
            if (isWave)
            {
                timerCountdownUI.StopTimer();

                StateMachine.ChangeState(BUY_STATE);

                SetDown();
            }
        }
    }

    private void SetDown()
    {
        ScoreTextUI.Instance.SetDown();

        BonesCountUI.Instance.SetDown();

        Enemy.OnKilled -= Instance_OnKilled;

    }

    private void StartCountdown()
    {
        isDoneCountdown = false;
        CountdownUI.Instance.StartCountdown();
    }

}
