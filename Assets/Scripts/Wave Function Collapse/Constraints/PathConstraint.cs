using UnityEngine;

[CreateAssetMenu(fileName = "PathConstraint", menuName = "Scriptable Objects/PathConstraint")]
public class PathConstraint : ScriptableObject,IEdgeConstraint
{
    public bool Matches(IEdgeConstraint AotherConstraint)
    {
        return AotherConstraint is PathConstraint;
    }
}
