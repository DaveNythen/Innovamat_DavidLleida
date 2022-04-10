using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class FloatReference
{
    public FloatVariable variable;

    public float Value
    {
        get { return variable.value; }
    }
}
