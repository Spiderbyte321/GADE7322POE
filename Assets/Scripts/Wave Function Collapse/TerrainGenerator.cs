using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Unity.Mathematics;
using Random = System.Random;

public class TerrainGenerator : Pathfinder
{
    [SerializeField] private GameObject TileParent;
    [SerializeField] private GameObject TilePrefab;
    [SerializeField] private int[] GridXRange=new int[ArraySizes];
    [SerializeField] private int[] GridYRange = new int[ArraySizes];
    [SerializeField] private TileSet PlayerTower;
    [SerializeField] private GameObject PlayerTowerPrefab;
    [SerializeField] private TileSet EnemyTower;
    [SerializeField] private GameObject EnemyTowerPrefab;
    [SerializeField] private TileSet[] TransitionTileSets;//ohhhh dear I need to rework this
    //no wait I don't I just need to adjust it...maybe
    [SerializeField] private TileSet[] PathTileSets;
    [SerializeField] private TileSet PlayerDefenderSet;
    [SerializeField] private GameObject PayerDefenderPrefab;
    [SerializeField] private int EnemyShrinesAmount;
    [SerializeField] private int ShrineOffset;
    [SerializeField] private int PlayerTowerDensity;
    [SerializeField] private int minimumTransitionDistanceFromTower;
    [SerializeField] [Range(0, 100)] private int SimilarPathsThreshold;
    private List<Tile> CollapsedTiles = new List<Tile>();
    private const int ArraySizes = 2;
    private int GridBreadth;
    private int GridHeight;
    private Tile[,] GridTiles;
    private Tile CurrentTile;
    private Tile PlayerBase;
    private List<List<Tile>> Paths = new List<List<Tile>>();
    private Dictionary<List<Tile>, Tile> TransitionTiles = new Dictionary<List<Tile>, Tile>();
    private Dictionary<Tile, List<Tile>> enemyPaths = new Dictionary<Tile, List<Tile>>();


    public IReadOnlyDictionary<Tile, List<Tile>> EnemyPaths => enemyPaths;
    public static TerrainGenerator Instance;

    public delegate void OnMapGenerated(Tile playerBase);

    public static event OnMapGenerated MapGeneratedAction;
    
    //idea for cleanup rework this script so that spawning a tile is a method that takes in what tile to spawn


    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
            Instance = this;
        }
    }

    private void Start()
    {
        
        
        //ClearGrid();
        InitialiseGrid();
        PlacePlayerTower();
        

        
        
        for(int i=0;i<EnemyShrinesAmount;i++)
            PlaceEnemyTower();
        
        
        CreatePathways();
        PlaceAllyTowerSpots();
              
        
        CurrentTile=ChooseLowestEntropyTile();
        
        do
        {
          BuildGrid();
          CurrentTile=ChooseLowestEntropyTile();
        } while(CollapsedTiles.Count<GridTiles.Length);
        
        MapGeneratedAction?.Invoke(PlayerBase);
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

    private void ClearGrid()
    {
       Tile[] createdTiles= TileParent.GetComponentsInChildren<Tile>();

       foreach (Tile createdTile in createdTiles)
       {
           Destroy(createdTile.gameObject);
       }
       
       CollapsedTiles.Clear();
       Paths.Clear();
       TransitionTiles.Clear();
    }

    private void PlacePlayerTower()
    {
        int RandomX = UnityEngine.Random.Range(0, GridBreadth-1);
        int RandomY = UnityEngine.Random.Range(0, GridHeight-1);
        
        Destroy(GridTiles[RandomX,RandomY].gameObject);
        Vector3 PlacementPosition = new Vector3(RandomX, 0, RandomY);
        GameObject SpawnedObject =Instantiate(PlayerTowerPrefab, PlacementPosition, quaternion.identity);
        SpawnedObject.transform.SetParent(TileParent.transform);
        Tile SpawnedTile;
        SpawnedObject.TryGetComponent(out SpawnedTile);
        GridTiles[RandomX, RandomY] = SpawnedTile;
        
        
        CurrentTile = GridTiles[RandomX,RandomY];
        CurrentTile.Collapse(PlayerTower);
        CurrentTile.InitialiseTile();
        PlayerBase = CurrentTile;
        PropogateCollapse(CurrentTile);
        CollapsedTiles.Add(CurrentTile);
        
    }

    private void PlaceEnemyTower()
    {
        
        int RandomX;
        int RandomY;

        int enemyTowerXOffset;
        int playerTowerXOffset;
        
        do// while the point chosen is too close to player reroll
        {
            RandomX = UnityEngine.Random.Range(0, GridBreadth);
            RandomY = UnityEngine.Random.Range(0, GridHeight);

            enemyTowerXOffset = GridBreadth - RandomX;
            playerTowerXOffset = GridBreadth - PlayerBase.TilePosition.x;

        } while (Math.Abs(enemyTowerXOffset-playerTowerXOffset)<ShrineOffset);
        
        
        
        Destroy(GridTiles[RandomX,RandomY].gameObject);
        Vector3 PlacementPosition = new Vector3(RandomX, 0, RandomY);
        GameObject SpawnedObject =Instantiate(EnemyTowerPrefab, PlacementPosition, quaternion.identity);
        SpawnedObject.transform.SetParent(TileParent.transform);
        Tile SpawnedTile;
        SpawnedObject.TryGetComponent(out SpawnedTile);
        GridTiles[RandomX, RandomY] = SpawnedTile;
        
        
        CurrentTile = GridTiles[RandomX, RandomY];
        CurrentTile.Collapse(EnemyTower);
        CurrentTile.InitialiseTile();
        PropogateCollapse(CurrentTile);
        CreateFrontier(CurrentTile.TilePosition,GridTiles);
        List<Tile> Path =GetPath(PlayerBase,CurrentTile);


        foreach (Tile tileToMove in Path)//fix this later
        {
            Vector3 MoveVector = tileToMove.transform.position;
            MoveVector.y -= 0.15f;

            tileToMove.transform.position = MoveVector;
        }
        
        Paths.Add(Path);
        enemyPaths.Add(CurrentTile,Path);
        CollapsedTiles.Add(CurrentTile);
    }

    private bool ValidateGrid()
    {
        Dictionary<Vector2,Tile> UniqueTiles = new Dictionary<Vector2, Tile>();
        
        foreach (Tile collapse in CollapsedTiles)
        {
            UniqueTiles.TryAdd(collapse.TilePosition, collapse);
        }
        
        if (UniqueTiles.Count / CollapsedTiles.Count * 100 < SimilarPathsThreshold)
        {
            return true;
        }
        
        return false;
    }
    
    

    private void CreatePathways()//Should be marking these as paths rather than just collapsing
    {//Just give them onlypaths and let the WFC handle the rest Sweet!
        foreach (List<Tile> path in Paths)
        {
            foreach (Tile pathPoint in path)
            {
                pathPoint.AmendTileInfo(PathTileSets);
            }
            
            Tile TransitionTile;
            int testint = UnityEngine.Random.Range(minimumTransitionDistanceFromTower, path.Count / 2);
            TransitionTile = path[testint];//chat to erin about this
            TransitionTile.AmendTileInfo(TransitionTileSets);
            TransitionTiles.Add(path,TransitionTile);
        }
    }

    private void PlaceAllyTowerSpots()
    {
        foreach (List<Tile> path in Paths)
        {
            List<Tile> TotalPath = new List<Tile>();
            path.Reverse();
            TotalPath = path;

            List<Tile> PlayerPath = new List<Tile>();
            Vector2 PathTransition = TransitionTiles[path].TilePosition;
            
            foreach(Tile PathPoint in TotalPath)
            {
                if(PathPoint.TilePosition==PathTransition)
                    break;
                
                PlayerPath.Add(PathPoint);
                
            }

            for(int i = 0; i < PlayerTowerDensity; i++)
            {
                int RandomTileIndex = UnityEngine.Random.Range(0, PlayerPath.Count);

                Tile[] TilePointNeighbours = ReturnNeighbours(PlayerPath[RandomTileIndex]);
                List<Tile> PossiblePlacements = new List<Tile>();
                
                
                foreach (Tile neighbour in TilePointNeighbours)
                {
                    if (neighbour.Reserved)//need to indicate if path... I know how
                        continue;

                    
                    PossiblePlacements.Add(neighbour);
                }

                Dictionary<EDirection, EdgeConstraint> PLayerTowerConstraints =
                                        new Dictionary<EDirection, EdgeConstraint>();
                //Index out of bounds here must fix
                int TileInt = UnityEngine.Random.Range(0, PossiblePlacements.Count - 1);
                if(PossiblePlacements.Count==0)
                    continue;
                Debug.Log(TileInt);
                Tile ChosenPlacement = PossiblePlacements[TileInt];
                EDirection DirectionToRoad =
                    DirectionUtilities.ReturnTileDirection(ChosenPlacement, PlayerPath[RandomTileIndex]);

                PLayerTowerConstraints[DirectionToRoad] = ScriptableObject.CreateInstance<WallConstraint>();

                foreach (EDirection direction in Enum.GetValues(typeof(EDirection)))
                {
                    PLayerTowerConstraints.TryAdd(direction, ScriptableObject.CreateInstance<WallConstraint>());
                }
                
                
                PlayerDefenderSet.InitialiseTileSet(PLayerTowerConstraints);
                
                Vector3 PlacementPosition = new Vector3(ChosenPlacement.TilePosition.x, 0,ChosenPlacement.TilePosition.y );
                Destroy(ChosenPlacement.gameObject);
                
                GameObject SpawnedObject =Instantiate(PayerDefenderPrefab, PlacementPosition, quaternion.identity);
                SpawnedObject.transform.SetParent(TileParent.transform);
                Tile SpawnedTile;
                SpawnedObject.TryGetComponent(out SpawnedTile);
                ChosenPlacement = SpawnedTile;
                GridTiles[ChosenPlacement.TilePosition.x, ChosenPlacement.TilePosition.y] = ChosenPlacement;
                ChosenPlacement.Collapse(PlayerDefenderSet);
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


    public void ReplaceSpot(Transform ReplacementPoint)
    {
        GameObject SpawnedObject =Instantiate(PayerDefenderPrefab, ReplacementPoint.position, quaternion.identity);
        SpawnedObject.transform.SetParent(TileParent.transform);
        Tile SpawnedTile;
        SpawnedObject.TryGetComponent(out SpawnedTile);
        SpawnedTile.Collapse(PlayerDefenderSet);
    }
}
