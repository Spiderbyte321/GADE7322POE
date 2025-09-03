using UnityEngine;

[CreateAssetMenu(fileName = "PathConstraint", menuName = "Scriptable Objects/PathConstraint")]
public class PathConstraint :  EdgeConstraint
{
    public override bool Matches(IEdgeConstraint AotherConstraint)
    {
        return AotherConstraint is PathConstraint;
    }
}
