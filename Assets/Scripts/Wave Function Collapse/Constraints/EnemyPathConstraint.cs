using UnityEngine;

[CreateAssetMenu(fileName = "EnemyPathConstraint", menuName = "Scriptable Objects/EnemyPathConstraint")]
public class EnemyPathConstraint : EdgeConstraint
{
    public override bool Matches(IEdgeConstraint AotherConstraint)
    {
        return AotherConstraint is EnemyPathConstraint;
    }
}
