using UnityEngine;

public class TileData
{
    private Tile parent;
    private bool visited;
    private int cost;

    public int Cost
    {
        get { return cost; }
    }

    public bool Visited
    {
        get { return visited; }
    }
    
    public Tile Parent
    {
        get { return parent; }
    }

    public TileData(Tile AParent=null,bool AVisited=false,int ACost=0)
    {
        parent = AParent;
        visited = AVisited;
        cost = ACost;
    }
}
