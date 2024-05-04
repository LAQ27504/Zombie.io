using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyState : BaseState
{
    [SerializeField] private BuyingUI buyingUi;

    [SerializeField] private UpgradeManager upgradeManager;

    [SerializeField] private Player player;

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
        buyingUi.Show();
        upgradeManager.SetUp();
        buyingUi.OnNextRoundButtonClicked += BuyingUi_OnNextRoundButtonClicked;
        player.GetComponent<PlayerPauseControl>().PausePlayer();
    }

    private void BuyingUi_OnNextRoundButtonClicked(object sender, System.EventArgs e)
    {
        player.ResetStat();
        WalkerGenerator.Instance.ClearMap();
        StateMachine.ChangeState(GENERATE_STATE);
    }

    public override void ExitState()
    {
        buyingUi.OnNextRoundButtonClicked -= BuyingUi_OnNextRoundButtonClicked;
        buyingUi.Hide();
        gameObject.SetActive(false);
    }

    public override void Update()
    {

    }

    public override void Render()
    {

    }
}
