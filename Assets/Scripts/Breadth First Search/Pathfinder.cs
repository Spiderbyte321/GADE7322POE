using UnityEngine;
using System.Collections.Generic;
using System.Net;

public abstract class Pathfinder : MonoBehaviour//cleanup this whole class
{
    private List<Tile> Frontier = new List<Tile>();
    private Queue<Tile> OpenSet = new Queue<Tile>();
    private Dictionary<Tile, TileData> TilesData = new Dictionary<Tile, TileData>();
    private Vector2Int[] ExplorationDirection ={ Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
    
    private Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();

    protected void CreateFrontier(Vector2 StartCoordinates,Tile[,]gridCopy)
    { 
        
        ClearFrontier();
        
        AddTiles(gridCopy);
        
        
        OpenSet.Enqueue(gridCopy[(int)StartCoordinates.x,(int)StartCoordinates.y]);
        
        while(OpenSet.Count > 0)
        {
            Tile CurrentTile = OpenSet.Dequeue();
            
            
            TileData CurrentData = new TileData(null,true,0);
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


    private void AddTiles(Tile[,] AgridCopy)
    {
        tiles.Clear();

        int width = AgridCopy.GetLength(0);
        int height = AgridCopy.GetLength(1);
        

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                
                Tile ChosenTile = AgridCopy[i, j];
                
                tiles.Add(ChosenTile.TilePosition,ChosenTile);
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
    
    private List<Tile> GetNeighbour(Tile CurrentTile)// it's in tiles but not getting out?
    {
        List<Tile> ReturnedNeighbours = new List<Tile>();
        foreach(Vector2Int Direction in ExplorationDirection)
        {
            Vector2Int NeighbourCoordinates = CurrentTile.TilePosition + Direction;
            
            
            if(tiles.ContainsKey(NeighbourCoordinates))
            {
                ReturnedNeighbours.Add(tiles[NeighbourCoordinates]);
            }
        }

        
        
        return ReturnedNeighbours;
    }
}
