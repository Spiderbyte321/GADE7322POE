using UnityEngine;

public interface IEdgeConstraint 
{
    public bool Matches(IEdgeConstraint AotherConstraint);
}
