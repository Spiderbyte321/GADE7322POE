using UnityEngine;
using System.Collections.Generic;

public abstract class Pathfinder : MonoBehaviour//cleanup this whole class
{
    private List<Tile> Frontier = new List<Tile>();
    private Queue<Tile> OpenSet = new Queue<Tile>();
    private Dictionary<Tile, TileData> TilesData = new Dictionary<Tile, TileData>();
    private Vector2[] ExplorationDirection ={ Vector2.up, Vector2.right, Vector2.down, Vector2.left };

    private Tile[,] GridCopy;
    private Dictionary<Vector2, Tile> tiles = new Dictionary<Vector2, Tile>();

    protected void CreateFrontier(Vector2 StartCoordinates,Tile[,]gridCopy)
    { 
        ClearFrontier();
        GridCopy = gridCopy;
        foreach (Tile chosenTile in GridCopy)
        {
            tiles.TryAdd(chosenTile.TilePosition,chosenTile);
        }
        
        OpenSet.Enqueue(GridCopy[(int)StartCoordinates.x,(int)StartCoordinates.y]);
        while(OpenSet.Count > 0)
        {
            Tile CurrentTile = OpenSet.Dequeue();
            TileData CurrentData = new TileData(null,true,0);
            //Change Tile Mesh
            if(!Frontier.Contains(CurrentTile))
            {
                Frontier.Add(CurrentTile);
            }

            TilesData.TryAdd(CurrentTile, CurrentData);
            
            List<Tile> Neighbours = GetNeighbour(CurrentTile);
            
            foreach(Tile AdjacentTile in Neighbours)
            {
                
                TilesData.TryAdd(AdjacentTile, new TileData());  

                if(TilesData[AdjacentTile].Visited)
                { 
                  continue;  
                }
                

                TilesData[AdjacentTile] = new TileData(CurrentTile, true, TilesData[CurrentTile].Cost + 1);
                OpenSet.Enqueue(AdjacentTile); 
            }
        }
        
    }

    protected List<Tile> GetPath(Tile Destination,Tile UnitOrigin)
    {
        List<Tile> Path = new List<Tile>();
        Tile CurrentTile = Destination;
        
        
        while(Destination != UnitOrigin)
        {
            Path.Add(CurrentTile);
            
            Debug.Log(CurrentTile.TilePosition);
            if(TilesData[CurrentTile].Parent is not null)
            {
                CurrentTile = TilesData[CurrentTile].Parent;
            }
            else
            {
                break;
            }
        }
        
        Path.Reverse();
        return Path;

    }

    protected void ClearFrontier()
    {
        Frontier = new List<Tile>();
        OpenSet = new Queue<Tile>();
        TilesData = new Dictionary<Tile, TileData>();
    }
    
    private List<Tile> GetNeighbour(Tile CurrentTile)
    {
        List<Tile> ReturnedNeighbours = new List<Tile>();
        foreach(Vector2 Direction in ExplorationDirection)
        {
            Vector2 NeighbourCoordinates = CurrentTile.TilePosition + Direction;
            Debug.Log(CurrentTile.TilePosition);
            if(tiles.ContainsKey(NeighbourCoordinates))
            {
                ReturnedNeighbours.Add(tiles[NeighbourCoordinates]);
            }
        }
        return ReturnedNeighbours;
    }
    
    
}
