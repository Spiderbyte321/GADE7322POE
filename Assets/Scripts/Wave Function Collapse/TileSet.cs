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
}
