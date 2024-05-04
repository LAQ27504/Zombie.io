using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public static StateMachine Instance { get; private set; }

    private static Dictionary<string, BaseState> states = new Dictionary<string, BaseState>();

    private static BaseState currentState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }

    public static void SetStateMachine(Dictionary<string, BaseState> stateDict)
    {
        states = stateDict;
        currentState = null;
    }

    public static void ChangeState(string stateName)
    {
        if (states.ContainsKey(stateName) && currentState != null)
        {
            currentState.GetNextStates(stateName);

            currentState.ExitState();

            currentState = states[stateName];

            currentState.EnterState();

            currentState.Render();
        }
        else if (states.ContainsKey(stateName))
        {
            currentState = states[stateName];

            currentState.EnterState();

            currentState.Render();
        }
    }

    private static void Update()
    {
        currentState?.Update();
    }

}
