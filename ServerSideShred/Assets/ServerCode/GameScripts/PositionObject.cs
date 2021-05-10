using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionObject
{
    public PositionObject(Vector3 _pos, Quaternion _rot)
    {
        pos = _pos;
        rot = _rot;
    }
    
    public Vector3 pos;
    public Quaternion rot;
}
