using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    
    [SerializeField] private GameManager gameManager;

    [SerializeField] private Transform enemy;

    public List<Vector3> spawnPointList;
    
    public void ImportSpawmPoint(Vector3 spawnLocation)
    {
        Vector3 spawnPoint = new Vector3(spawnLocation.x + 0.5f, spawnLocation.y + 0.5f, 0);
        if (!spawnPointList.Contains(spawnPoint)) 
        {
            spawnPointList.Add(spawnPoint);

        }
    }

    public void ClearList()
    {
        spawnPointList.Clear(); 
    }


}
