using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

public class TerrainGenerator : Pathfinder
{
    [SerializeField] private GameObject TileParent;
    [SerializeField] private GameObject TilePrefab;
    [SerializeField] private int[] GridXRange=new int[ArraySizes];
    [SerializeField] private int[] GridYRange = new int[ArraySizes];
    [SerializeField] private TileSet PlayerTower;
    [SerializeField] private TileSet EnemyTower;
    [SerializeField] private int EnemyShrinesAmount;
    private List<Tile> CollapsedTiles = new List<Tile>();
    private const int ArraySizes = 2;
    private int GridBreadth;
    private int GridHeight;
    private Tile[,] GridTiles;
    private Tile CurrentTile;
    private Tile PlayerBase;
    private List<List<Tile>> Paths = new List<List<Tile>>();

    private void Start()
    {
        InitialiseGrid();
        PlacePlayerTower();
        
        for(int i=0;i<EnemyShrinesAmount;i++)
            PlaceEnemyTower();
        
        CreatePathways();
        
        /*CurrentTile=ChooseLowestEntropyTile();
        do
        {
          BuildGrid();
          CurrentTile=ChooseLowestEntropyTile();
        } while(CollapsedTiles.Count<GridTiles.Length);*/
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
                GridTiles[i,j].InitialiseTile();
            }
        }
    }

    private void PlacePlayerTower()
    {
        int RandomX = UnityEngine.Random.Range(0, GridBreadth-1);
        int RandomY = UnityEngine.Random.Range(0, GridHeight-1);
        CurrentTile = GridTiles[RandomX,RandomY];
        CurrentTile.Collapse(PlayerTower);
        PlayerBase = CurrentTile;
        PropogateCollapse(CurrentTile);
        CollapsedTiles.Add(CurrentTile);
        
        Debug.Log("Player position"+CurrentTile.TilePosition);
    }

    private void PlaceEnemyTower()
    {
        int RandomX = UnityEngine.Random.Range(0, GridBreadth);
        int RandomY = UnityEngine.Random.Range(0, GridHeight);
        
        
        CurrentTile = GridTiles[RandomX, RandomY];
        CurrentTile.Collapse(EnemyTower);
        PropogateCollapse(CurrentTile);
        CreateFrontier(CurrentTile.TilePosition,GridTiles);
        List<Tile> Path =GetPath(PlayerBase,CurrentTile);

        Paths.Add(Path);
        
        
        CollapsedTiles.Add(CurrentTile);
    }

    private void CreatePathways()
    {

        foreach (List<Tile> path in Paths)
        {
            foreach (Tile pathpoint in path)
            {
                pathpoint.Collapse();
                PropogateCollapse(pathpoint);
                CollapsedTiles.Add(pathpoint);
            }
        }
        
    }
        

    private Tile ChooseLowestEntropyTile()
    {
        //returns lowest entropy tile
        //get all the tiles into a 1d array
        //sort by entropy
        //return the first element
        
        CollapsedTiles.Clear();
        CollapsedTiles = new List<Tile>();
        
        List<Tile> EntroipicTiles = new List<Tile>();
        for (int x = 0;  x<GridBreadth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                if(!GridTiles[x, y].Collapsed)
                {
                  EntroipicTiles.Add(GridTiles[x,y]);  
                }
                else
                {
                   CollapsedTiles.Add(GridTiles[x,y]); 
                }
            }
        }

        Tile[] EntropicTilesArray = EntroipicTiles.ToArray();
        Array.Sort(EntropicTilesArray,(a,b)=>a.Entropy.CompareTo(b.Entropy));

        if(EntropicTilesArray.Length!=0)
        {
           return EntropicTilesArray[0]; 
        }
        
        return CurrentTile;
        
    }
    
    
    
    private void BuildGrid()
    {
        if(!CurrentTile.Collapsed)
        {
          CurrentTile.Collapse();
          CollapsedTiles.Add(CurrentTile);
          PropogateCollapse(CurrentTile);
        }
    }


    private void PropogateCollapse(Tile CollapsedTile)
    {
        Queue<Tile> PropogationQueue = new Queue<Tile>();
        HashSet<Tile> EnquedTiles = new HashSet<Tile>();
        Tile[] Neighbours = ReturnNeighbours(CollapsedTile);
        foreach (Tile neighbour in Neighbours)
        {
            PropogationQueue.Enqueue(neighbour);
            EnquedTiles.Add(neighbour);
        }
        
        
        do
        {
            
            
            Tile AmendNeighbour = PropogationQueue.Dequeue();
            EnquedTiles.Remove(AmendNeighbour);
            int OriginalCount = AmendNeighbour.Entropy;
            if(AmendNeighbour.Collapsed)
            {
              continue;  
            }

            if(AmendNeighbour.TilePosition.x+1<GridBreadth&&GridTiles[AmendNeighbour.TilePosition.x + 1, AmendNeighbour.TilePosition.y].Collapsed)
            {

                
                EDirection Direction = DirectionUtilities.ReturnTileDirection(AmendNeighbour,
                    GridTiles[AmendNeighbour.TilePosition.x + 1, 
                        AmendNeighbour.TilePosition.y]);
                AmendNeighbour.AmendTile(GridTiles[AmendNeighbour.TilePosition.x+1,
                    AmendNeighbour.TilePosition.y].CollapseInfo.LeftConstraint,Direction);
            }//double check this will work cause ammend tile assumes the origin constraint is on origin
            
            if(AmendNeighbour.TilePosition.x-1>0&&GridTiles[AmendNeighbour.TilePosition.x - 1, AmendNeighbour.TilePosition.y].Collapsed)
            {

                
                
                EDirection Direction = DirectionUtilities.ReturnTileDirection(AmendNeighbour,
                    GridTiles[AmendNeighbour.TilePosition.x - 1, 
                        AmendNeighbour.TilePosition.y]);
                AmendNeighbour.AmendTile(GridTiles[AmendNeighbour.TilePosition.x-1,
                    AmendNeighbour.TilePosition.y].CollapseInfo.RightConstraint,Direction);
            }
            
            if(AmendNeighbour.TilePosition.y+1<GridHeight&&GridTiles[AmendNeighbour.TilePosition.x, AmendNeighbour.TilePosition.y+1].Collapsed)
            {
                
                EDirection Direction = DirectionUtilities.ReturnTileDirection(AmendNeighbour,
                    GridTiles[AmendNeighbour.TilePosition.x, 
                        AmendNeighbour.TilePosition.y+1]);
                AmendNeighbour.AmendTile(GridTiles[AmendNeighbour.TilePosition.x,
                    AmendNeighbour.TilePosition.y+1].CollapseInfo.BottomConstraint,Direction);
            }
            
            if (AmendNeighbour.TilePosition.y-1>0&&GridTiles[AmendNeighbour.TilePosition.x , AmendNeighbour.TilePosition.y-1].Collapsed)
            {
                
                EDirection Direction = DirectionUtilities.ReturnTileDirection(AmendNeighbour,
                    GridTiles[AmendNeighbour.TilePosition.x, 
                        AmendNeighbour.TilePosition.y-1]);
                AmendNeighbour.AmendTile(GridTiles[AmendNeighbour.TilePosition.x,
                    AmendNeighbour.TilePosition.y-1].CollapseInfo.TopConstraint,Direction);
            }
            
            if(AmendNeighbour.Entropy < OriginalCount)
            {
                foreach(Tile neighbour in ReturnNeighbours(AmendNeighbour))
                {
                    if(!EnquedTiles.Contains(neighbour))
                    {
                      PropogationQueue.Enqueue(neighbour);
                      EnquedTiles.Add(neighbour);
                    }
                } 
            }
        } while (PropogationQueue.Count>0);
        
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
    
    private Tile[] ReturnNeighbours(Tile TileOrigin)
    {
        List<Tile> ReturnArray = new List<Tile>();
        
        if(TileOrigin.TilePosition.x+1<GridBreadth) 
            ReturnArray.Add(GridTiles[TileOrigin.TilePosition.x+1,TileOrigin.TilePosition.y]);
        
        if(TileOrigin.TilePosition.x-1>0) 
            ReturnArray.Add(GridTiles[TileOrigin.TilePosition.x-1,TileOrigin.TilePosition.y]);
        
        if(TileOrigin.TilePosition.y+1<GridHeight) 
            ReturnArray.Add(GridTiles[TileOrigin.TilePosition.x,TileOrigin.TilePosition.y+1]);
        
        if(TileOrigin.TilePosition.y-1>0)
            ReturnArray.Add(GridTiles[TileOrigin.TilePosition.x,TileOrigin.TilePosition.y-1]);

        return ReturnArray.ToArray();
    }
}
