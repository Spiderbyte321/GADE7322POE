using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileSet", menuName = "Scriptable Objects/TileSet")]
public class TileSet : ScriptableObject
{
    [SerializeField] private GameObject tileMeshObject;
    [SerializeField] private EdgeConstraint topConstraint;
    [SerializeField] private EdgeConstraint rightConstraint;
    [SerializeField] private EdgeConstraint bottomConstraint;
    [SerializeField] private EdgeConstraint leftConstraint;
    
    public GameObject TileMeshObject => tileMeshObject;
    public EdgeConstraint TopConstraint => topConstraint;
    public EdgeConstraint RightConstraint => rightConstraint;
    public EdgeConstraint BottomConstraint => bottomConstraint;
    public EdgeConstraint LeftConstraint => leftConstraint;
    
    
    public EdgeConstraint OppositeConstraint(EDirection ADirection)
    {
        EDirection OppositeDirection = DirectionUtilities.GetOppositeDirection(ADirection);
        switch(OppositeDirection)
        {
            case EDirection.North:
                return BottomConstraint;
            case EDirection.East:
                return LeftConstraint;
            case EDirection.South:
                return TopConstraint;
            case EDirection.West:
                return RightConstraint;
            
        }

        return TopConstraint;
    }
    

    public void InitialiseTileSet(Dictionary<EDirection,EdgeConstraint> AConstraintConfig)//for every direction of constraint assign it to relevant constraint field
    {
        foreach (EDirection direction in Enum.GetValues(typeof(EDirection)))//this lets you loop over all enum values
        {
            switch (direction)
            {
                case EDirection.North:
                  topConstraint =AConstraintConfig[direction]; 
                    break;
                case EDirection.East:
                    rightConstraint  =AConstraintConfig[direction];
                    break;
                case EDirection.South:
                    bottomConstraint =AConstraintConfig[direction];
                    break;
                case EDirection.West:
                    leftConstraint =AConstraintConfig[direction];
                    break;
            }
        }
    }
}
