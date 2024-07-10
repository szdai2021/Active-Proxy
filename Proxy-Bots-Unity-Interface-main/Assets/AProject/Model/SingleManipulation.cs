using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingleManipulation : PhysicalManipulation
{
    public Transform target;

    public abstract void Evaluate();
}
