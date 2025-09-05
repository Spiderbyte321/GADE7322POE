using UnityEngine;

[CreateAssetMenu(fileName = "WallConstraint", menuName = "Scriptable Objects/WallConstraint")]
public class WallConstraint : EdgeConstraint
{
    public override bool Matches(IEdgeConstraint AotherConstraint)
    {
        return AotherConstraint is WallConstraint;
    }
}
