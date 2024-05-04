using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : BaseState
{
    [SerializeField] private PauseUI pauseUI;
    [SerializeField] private Player player;
    [SerializeField] private Transform enemyHolder;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public override void GetNextStates(string state)
    {

    }

    public override void EnterState()
    {
        gameObject.SetActive (true);

        Time.timeScale = 0f;

        pauseUI.Show();

        player.GetComponent<PlayerPauseControl>().PausePlayer();

        if (Enemy.Instance != null)
        {
            Enemy.Instance.PauseEnemy();
        }

        pauseUI.OnUnpauseClicked += PauseUI_OnUnpauseClicked;
        pauseUI.OnHomeClicked += PauseUI_OnHomeClicked;
    }

    private void PauseUI_OnHomeClicked(object sender, System.EventArgs e)
    {
        Time.timeScale = 1f;

        DeleteAllEnemies();
        WalkerGenerator.Instance.ClearMap();
        StateMachine.ChangeState(MAIN_MENU);
    }

    private void PauseUI_OnUnpauseClicked(object sender, System.EventArgs e)
    {
        StateMachine.ChangeState(PLAY_STATE);
    }

    public override void ExitState()
    {
        pauseUI.OnUnpauseClicked -= PauseUI_OnUnpauseClicked;
        pauseUI.Hide();
        gameObject.SetActive(false );
    }

    private void DeleteAllEnemies()
    {
        foreach (Enemy enemy in enemyHolder.GetComponentsInChildren<Enemy>())
        {
            Debug.Log(enemy);
            Destroy(enemy.gameObject);
        }
    }

    public override void Update()
    {

    }

    public override void Render()
    {

    }
}
