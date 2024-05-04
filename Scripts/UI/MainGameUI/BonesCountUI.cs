using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BonesCountUI : MonoBehaviour
{
    public static BonesCountUI Instance;

    [SerializeField] private TextMeshProUGUI boneCountText;
    
    [SerializeField] private int count = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        boneCountText.text = count.ToString();
    }

    public void SetUp()
    {
        Enemy.OnKilled += Enemy_OnKilled;

        boneCountText.text = count.ToString();
    }

    public void SetDown()
    {
        Enemy.OnKilled -= Enemy_OnKilled;
    }

    public void ResetBones()
    {
        count = 0;
        boneCountText.text = count.ToString();
    }

    public bool CanDecreaseBone(int boneCosts, bool canUpgrade)
    {
        
        if (count < boneCosts || !canUpgrade)
        {
            Debug.Log("Can not do thí action");
            return false;
        }
        Debug.Log("successfull");
        count -= boneCosts;
        boneCountText.text = count.ToString();
        return true;
    }

    private void Enemy_OnKilled(object sender, Enemy.OnKilledArgs e)
    {
        count += e.bone;
        boneCountText.text = count.ToString();
    }
}
