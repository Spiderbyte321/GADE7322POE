using UnityEngine;
using System;

public abstract class EdgeConstraint : ScriptableObject,IEdgeConstraint
{
    public virtual bool Matches(IEdgeConstraint AotherConstraint)
    {
        throw new NotImplementedException();
    }
}
