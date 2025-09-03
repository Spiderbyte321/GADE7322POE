using UnityEngine;
using System;

public class EdgeConstraint : ScriptableObject,IEdgeConstraint
{
    public virtual bool Matches(IEdgeConstraint AotherConstraint)
    {
        throw new NotImplementedException();
    }
}
