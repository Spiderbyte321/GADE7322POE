using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private GameObject TileParent;
    [SerializeField] private GameObject TilePrefab;
    [SerializeField] private int[] GridXRange=new int[ArraySizes];
    [SerializeField] private int[] GridYRange = new int[ArraySizes];
    [SerializeField] private TileSet PlayerTower;
    [SerializeField] private TileSet EnemyTower;
    private List<Tile> CollapsedTiles = new List<Tile>();
    private const int ArraySizes = 2;
    private int GridBreadth;
    private int GridHeight;
    private Tile[,] GridTiles;
    private Tile CurrentTile;

    private void Start()
    {
        InitialiseGrid();
        PlacePlayerTower();
        PlaceEnemyTower();
        ChooseLowestEntropyTile();
        do
        {
          BuildGrid();
        } while(CollapsedTiles.Count<GridTiles.Length);
    }
    

    private void InitialiseGrid()//creates the grid and populates it with prefab instances
    {
        GridBreadth = UnityEngine.Random.Range(GridXRange[0], GridXRange[1]);
        GridHeight = UnityEngine.Random.Range(GridYRange[0], GridYRange[1]);
        GridTiles = new Tile[GridBreadth, GridHeight];
        Tile TempTile;
        
        for(int i = 0; i < GridBreadth; i++){
            for (int j = 0; j < GridHeight; j++)
            {
                Vector3 SpawnVector = new Vector3(i, 0,j);
                GameObject SpawnObject = Instantiate(TilePrefab, SpawnVector, quaternion.identity);
                SpawnObject.TryGetComponent(out TempTile);
                SpawnObject.transform.SetParent(TileParent.transform);
                GridTiles[i,j] = TempTile;
            }
        }
    }

    private void PlacePlayerTower()
    {
        int RandomX = UnityEngine.Random.Range(0, GridBreadth-1);
        int RandomY = UnityEngine.Random.Range(0, GridHeight-1);
        CurrentTile = GridTiles[RandomX,RandomY];
        CurrentTile.Collapse(PlayerTower);
        CollapsedTiles.Add(CurrentTile);
    }

    private void PlaceEnemyTower()
    {
        int RandomX = UnityEngine.Random.Range(0, GridBreadth);
        int RandomY = UnityEngine.Random.Range(0, GridHeight);

        if(GridTiles[RandomX, RandomY].Collapsed)
        {
          PlaceEnemyTower();  
        }

        CurrentTile = GridTiles[RandomX, RandomY];
        CurrentTile.Collapse(EnemyTower);
        CollapsedTiles.Add(CurrentTile);
    }

    private void ChooseLowestEntropyTile()
    {
        foreach (Tile EntropicTile in GridTiles)
        {
            if(!EntropicTile.Collapsed&&EntropicTile.Entropy <= CurrentTile.Entropy)
            {
                CurrentTile = EntropicTile;
            }
        }
    }
    
    
    
    private void BuildGrid()
    {
       
        CurrentTile.Collapse();
        CollapsedTiles.Add(CurrentTile);
        PropogateCollapse();
        ChooseLowestEntropyTile();
       
    }


    private void PropogateCollapse()
    {
        for(int x = 0; x < GridBreadth; x++)
        {
            for(int y = 0; y < GridHeight; y++)
            {
                if(!GridTiles[x, y].Collapsed) 
                    continue;
                
                if(x+1>0&&x+1<GridBreadth-1) 
                    GridTiles[x+1,y].AmendTile(GridTiles[x,y].CollapseInfo.LeftConstraint,EDirection.East);
                if(x-1>0&&x-1<GridBreadth-1)
                    GridTiles[x-1,y].AmendTile(GridTiles[x,y].CollapseInfo.RightConstraint,EDirection.West);
                if(y+1>0&&y+1<GridHeight-1)
                    GridTiles[x,y + 1].AmendTile(GridTiles[x,y].CollapseInfo.BottomConstraint,EDirection.North);
                if(y-1>0&&y-1<GridHeight-1)
                    GridTiles[x,y-1].AmendTile(GridTiles[x,y].CollapseInfo.TopConstraint,EDirection.South);
            }
        }
    }


    private void OnValidate()
    {
        if(GridXRange.Length != ArraySizes)
        {
          Array.Resize(ref GridXRange,ArraySizes);
          Debug.Log("Resized GridX to "+ArraySizes);
        }

        if (GridYRange.Length != ArraySizes)
        {
            Array.Resize(ref GridYRange,ArraySizes);
            Debug.Log("Resized GridY to "+ArraySizes);
        }
        
    }
}
