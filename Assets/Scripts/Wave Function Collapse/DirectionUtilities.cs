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
}
