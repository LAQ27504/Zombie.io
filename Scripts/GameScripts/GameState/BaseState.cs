using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    protected const string GENERATE_STATE = "Generate";
    protected const string PLAY_STATE = "Play";
    protected const string BUY_STATE = "Buy";
    protected const string GAMEOVER_STATE = "GameOver";
    protected const string PAUSE_STATE = "Pause";
    protected const string MAIN_MENU = "MainMenu";
    public abstract void EnterState();

    public abstract void Update();

    public abstract void ExitState();

    public abstract void GetNextStates(string state);
    
    public abstract void Render();


}
