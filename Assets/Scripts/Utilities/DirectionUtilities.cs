using System;
using UnityEngine;

public static class DirectionUtilities
{
    public static EDirection GetOppositeDirection(EDirection ADirection)
    {
        switch (ADirection)
        {
            case EDirection.North:
                return EDirection.South;
            case EDirection.East:
                return EDirection.West;
            case EDirection.South:
                return EDirection.North;
            case EDirection.West:
                return EDirection.East;
        }

        return EDirection.North;
    }

    public static EDirection ReturnTileDirection(Tile Origin,Tile Direction)
    {
        Vector2Int Delta = Direction.TilePosition - Origin.TilePosition;

        if (Delta == Vector2Int.up)
            return EDirection.North;
        if (Delta == Vector2Int.left)
            return EDirection.West;
        if (Delta == Vector2Int.right)
            return EDirection.East;
        if (Delta == Vector2Int.down)
            return EDirection.South;
        
        
        Debug.Log(Origin.TilePosition+":"+Direction.TilePosition);
        return EDirection.North;
    }
}
