using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WalkerGenerator : MonoBehaviour
{
    public static WalkerGenerator Instance;

    public event EventHandler<OnDoneCountWallMaxArgs> OnDoneCountWallMax;
    public class OnDoneCountWallMaxArgs : EventArgs
    {
        public int countWallMax;
    }

    public event EventHandler OnWallChange;

    public enum Grid
    {
        FLOOR,
        WALL,
        NONE,
        COUNT,
        EMPTY
    }

    //Variables
    [SerializeField]
    private EnemySpawn enemySpawn;
    [SerializeField]
    private Grid[,] gridHandler;
    [SerializeField]
    private List<WalkerObject> walkers;
    [SerializeField]
    private LoadingUI loadingUI;
    
    [SerializeField] 
    private Tilemap tileMap;
    [SerializeField]
    private Tilemap collision;
    [SerializeField]
    private Tile noneTile;
    [SerializeField]
    private Tile spawn;
    [SerializeField] 
    private Tile floor;
    [SerializeField]
    private Tile wall;

    [SerializeField] 
    private int mapWidth = 40;
    [SerializeField] 
    private int maximumWalkers = 10;
    [SerializeField] 
    private int tileCount = default;
    [SerializeField] 
    private int mapHeight = 40;
    [SerializeField]
    private int saveZone = 8;

    [SerializeField]
    private float chanceToSpawn = 0.999f;
    [SerializeField] 
    private float fillPercentage = 0.7f;
    [SerializeField] 
    private float waitTime = 0.05f;

    private int wallCount;
    private int wallCountMax;

    private Vector3Int highestPoint;
    private Vector3Int lowestPoint;

    private void Awake()
    {
        Instance = this;

        gridHandler = new Grid[mapWidth, mapHeight];
    }

    public void ClearMap()
    {
        int updatedCount = walkers.Count;

        walkers.Clear();

        for (int x = 0; x < gridHandler.GetLength(0); x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1); y++)
            {
                tileMap.SetTile(new Vector3Int(x, y, 0), noneTile);
                collision.SetTile(new Vector3Int(x, y, 0), noneTile);
                gridHandler[x, y] = Grid.EMPTY;
            }
        }
        enemySpawn.ClearList();
        wallCount = 0;
        wallCountMax = 0;
        tileCount = 0;
    }

    public void ResetStat(float spawnChance, int mapInfo)
    {
        chanceToSpawn = spawnChance;
        mapHeight = mapInfo;
        mapWidth = mapInfo;
    }

    public void ChangeLevelStat(float spawnChance, int roundlevel)
    {
        this.chanceToSpawn = spawnChance;
        mapWidth += roundlevel * 5;
        mapHeight += roundlevel * 5;
    }

    public void InitializeGrid()
    {
        Debug.Log(wallCount + " + " + wallCountMax + " + " + tileCount);
        for (int x = 0; x < gridHandler.GetLength(0); x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1); y++)
            {
                gridHandler[x, y] = Grid.EMPTY;
            }
        }

        walkers = new List<WalkerObject>();

        Vector3Int TileCenter = new Vector3Int(gridHandler.GetLength(0) / 2, gridHandler.GetLength(1) / 2, 0);

        highestPoint = CenteMap();
        lowestPoint = CenteMap();
        
        WalkerObject curWalker = new WalkerObject(new Vector2(TileCenter.x, TileCenter.y), GetDirection(), 0.5f);
        gridHandler[TileCenter.x, TileCenter.y] = Grid.FLOOR;
        tileMap.SetTile(TileCenter, floor);
        walkers.Add(curWalker);

        tileCount++;

        StartCoroutine(CreateFloors());
    }

    public Vector3Int CenteMap()
    {
        Vector3Int TileCenter = new Vector3Int(gridHandler.GetLength(0) / 2, gridHandler.GetLength(1) / 2, 0);
        return TileCenter;
    }

    private Vector2 GetDirection()
    {
        int choice = Mathf.FloorToInt(UnityEngine.Random.value * 3.99f);

        switch (choice)
        {
            case 0:
                return Vector2.down;
            case 1:
                return Vector2.left;
            case 2:
                return Vector2.up;
            case 3:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }

    private IEnumerator CreateFloors()
    {
        while ((float)tileCount / (float)gridHandler.Length < fillPercentage)
        {
            loadingUI.Loading(FloorPercentage() * 0.6f);

            bool hasCreatedFloor = false;
            foreach (WalkerObject curWalker in walkers)
            {
                Vector3Int curPos = new Vector3Int((int)curWalker.Position.x, (int)curWalker.Position.y, 0);

                
                if (gridHandler[curPos.x, curPos.y] != Grid.FLOOR)
                {
                    tileMap.SetTile(curPos, floor);
                    tileCount++;
                    gridHandler[curPos.x, curPos.y] = Grid.FLOOR;
                    hasCreatedFloor = true;
                    
                }
            }

            //Walker Methods
            ChanceToSpawn();
            ChanceToRemove();
            ChanceToRedirect();
            ChanceToCreate();
            UpdatePosition();

            if (hasCreatedFloor)
            {
                yield return new WaitForSeconds(waitTime);
            }
        }
        AddSpawnLocationIfThereIsNoSpawn();
        WallCount();
        StartCoroutine(CreateWalls());
    }

    private void AddSpawnLocationIfThereIsNoSpawn()
    {
        if (enemySpawn.spawnPointList.Count == 0) 
        {
            if (gridHandler[CenteMap().x, CenteMap().y + 8] == Grid.FLOOR)
            {
                highestPoint = new Vector3Int(CenteMap().x, CenteMap().y + 8);
                tileMap.SetTile(highestPoint, spawn);
                enemySpawn.ImportSpawmPoint(highestPoint);
            }
            else if (gridHandler[CenteMap().x, CenteMap().y - 8] == Grid.FLOOR)
            {
                highestPoint = new Vector3Int(CenteMap().x, CenteMap().y - 8);
                tileMap.SetTile(highestPoint, spawn);
                enemySpawn.ImportSpawmPoint(highestPoint);
            }
            else if (gridHandler[CenteMap().x + 8, CenteMap().y] == Grid.FLOOR)
            {
                highestPoint = new Vector3Int(CenteMap().x + 8, CenteMap().y);
                tileMap.SetTile(highestPoint, spawn);
                enemySpawn.ImportSpawmPoint(highestPoint);
            }
            else if (gridHandler[CenteMap().x - 8, CenteMap().y] == Grid.FLOOR)
            {
                highestPoint = new Vector3Int(CenteMap().x - 8, CenteMap().y);
                tileMap.SetTile(highestPoint, spawn);
                enemySpawn.ImportSpawmPoint(highestPoint);
            }
        }
    }

    private void ChanceToSpawn()
    {
        foreach (WalkerObject curWalker in walkers)
        {
            Vector3Int curPos = new Vector3Int((int)curWalker.Position.x, (int)curWalker.Position.y, 0);
            float roll = UnityEngine.Random.value;
            if ( roll > chanceToSpawn && gridHandler[curPos.x, curPos.y] == Grid.FLOOR && !IsInPlayerZone(curPos))
            {
                tileMap.SetTile(curPos, spawn);
                enemySpawn.ImportSpawmPoint(curPos);
                break;
            }
        }
    }
    private void ChanceToRemove()
    {
        int updatedCount = walkers.Count;
        for (int i = 0; i < updatedCount; i++)
        {
            if (UnityEngine.Random.value < walkers[i].ChanceToChange && walkers.Count > 1)
            {
                walkers.RemoveAt(i);
                break;
            }
        }
    }

  
    private void ChanceToRedirect()
    {
        for (int i = 0; i < walkers.Count; i++)
        {
            if (UnityEngine.Random.value < walkers[i].ChanceToChange)
            {
                WalkerObject curWalker = walkers[i];
                curWalker.Direction = GetDirection();
                walkers[i] = curWalker;
            }
        }
    }

    private void ChanceToCreate()
    {
        int updatedCount = walkers.Count;
        for (int i = 0; i < updatedCount; i++)
        {
            if (UnityEngine.Random.value < walkers[i].ChanceToChange && walkers.Count < maximumWalkers)
            {

                Vector2 newDirection = GetDirection();
                Vector2 newPosition = walkers[i].Position;

                WalkerObject newWalker = new WalkerObject(newPosition, newDirection, 0.5f);
                walkers.Add(newWalker);
            }
        }
    }

    private void UpdatePosition()
    {
        for (int i = 0; i < walkers.Count; i++)
        {
            WalkerObject FoundWalker = walkers[i];
            FoundWalker.Position += FoundWalker.Direction;
            FoundWalker.Position.x = Mathf.Clamp(FoundWalker.Position.x, 1, gridHandler.GetLength(0) - 2);
            FoundWalker.Position.y = Mathf.Clamp(FoundWalker.Position.y, 1, gridHandler.GetLength(1) - 2);
            walkers[i] = FoundWalker;
        }
    }

    private void WallCount()
    {
        for ( int i = 0; i < gridHandler.GetLength(0) - 1; i++)
        {
            for (int j = 0; j < gridHandler.GetLength(1) - 1; j++)
            {
                if (gridHandler[i, j] == Grid.FLOOR)
                {
                    if (gridHandler[i + 1, j] == Grid.EMPTY)
                    {
                        gridHandler[i + 1, j] = Grid.COUNT;
                        wallCount += 1;
                    }
                    if (gridHandler[i - 1, j] == Grid.EMPTY)
                    {
                        gridHandler[i - 1, j] = Grid.COUNT;
                        wallCount += 1;
                    }
                    if (gridHandler[i, j + 1] == Grid.EMPTY)
                    {
                        gridHandler[i, j + 1] = Grid.COUNT;
                        wallCount += 1;
                    }
                    if (gridHandler[i, j - 1] == Grid.EMPTY)
                    {
                        gridHandler[i, j - 1] = Grid.COUNT;
                        wallCount += 1;
                    }
                }
            }
        }

        OnDoneCountWallMax?.Invoke(this, new OnDoneCountWallMaxArgs
        {
            countWallMax = wallCount,
        });
        wallCountMax = wallCount;
    }


    private IEnumerator CreateWalls()
    {
        wallCount = 0;

        for (int x = 0; x < gridHandler.GetLength(0) - 1; x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1) - 1; y++)
            { 
                if (gridHandler[x, y] == Grid.FLOOR)
                {
                    bool hasCreatedWall = false;

                    if (gridHandler[x + 1, y] == Grid.COUNT)
                    {
                        collision.SetTile(new Vector3Int(x + 1, y, 0), wall);
                        gridHandler[x + 1, y] = Grid.WALL;
                        hasCreatedWall = true;
                        wallCount++;
                        OnWallChange?.Invoke(this, EventArgs.Empty);
                    }
                    if (gridHandler[x - 1, y] == Grid.COUNT)
                    {
                        collision.SetTile(new Vector3Int(x - 1, y, 0), wall);
                        gridHandler[x - 1, y] = Grid.WALL;
                        hasCreatedWall = true;
                        wallCount++;
                        OnWallChange?.Invoke(this, EventArgs.Empty);
                    }
                    if (gridHandler[x, y + 1] == Grid.COUNT)
                    {
                        collision.SetTile(new Vector3Int(x, y + 1, 0), wall);
                        gridHandler[x, y + 1] = Grid.WALL;
                        hasCreatedWall = true;
                        wallCount++; 
                        OnWallChange?.Invoke(this, EventArgs.Empty);
                    }
                    if (gridHandler[x, y - 1] == Grid.COUNT)
                    {
                        collision.SetTile(new Vector3Int(x, y - 1, 0), wall);
                        gridHandler[x, y - 1] = Grid.WALL;
                        hasCreatedWall = true;
                        wallCount++;
                        OnWallChange?.Invoke(this, EventArgs.Empty);
                    }
                    loadingUI.Loading(0.6f + WallPercentage() * 0.4f);

                    if (hasCreatedWall)
                    {
                        yield return new WaitForSeconds(waitTime);
                    }

                }
            }
        }
    }


    private bool IsInPlayerZone(Vector3 position)
    {
        float up = CenteMap().y + saveZone;
        float down = CenteMap().y - saveZone;
        float right = CenteMap().x + saveZone;
        float left = CenteMap().x - saveZone;

        return position.x <= right && position.x >= left && position.y <= up && position.y >= down;

    }
    private float FloorPercentage()
    {
        return ((float)tileCount / (float)gridHandler.Length) / fillPercentage;
    }

    private float WallPercentage()
    {
        return (float)wallCount / (float)wallCountMax;
    }

}