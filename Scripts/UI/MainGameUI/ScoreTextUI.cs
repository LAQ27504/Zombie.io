using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TMPro;
using UnityEngine;

public class ScoreTextUI : MonoBehaviour
{
    public static ScoreTextUI Instance;

    [SerializeField] private TextMeshProUGUI score;


    private int scoreCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SetUp()
    {
        Enemy.OnKilled += Enemy_OnKilled;

        score.text = "Score: " + scoreCount.ToString();
    }

    public void ResetScore()
    {
        scoreCount = 0;
        score.text = "Score: " + scoreCount.ToString();
    }

    public int GetScore()
    {
        return scoreCount;
    }

    public void SetDown()
    {
        Enemy.OnKilled -= Enemy_OnKilled;
    }

    private void Enemy_OnKilled(object sender, Enemy.OnKilledArgs e)
    {
        scoreCount += e.score;

        score.text = "Score: " + scoreCount.ToString();
    }

}
