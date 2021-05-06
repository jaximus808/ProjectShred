using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int classId;
    public int id;
    public string username; 
    public EarthBenderPlayer earthPlayer;

    public void StartDataToClass(int _id, string _username)
    {
        id = _id;
        username = _username;
        switch (classId)
        {
            case 0:
                earthPlayer.Initialize(this);
                break;
        }
    }

    public void SendInput(bool[] _inputs,Quaternion _rotation)
    {
        switch(classId)
        {
            case 0:
                earthPlayer.SetInput(_inputs, _rotation);
                break;
        }
    }
}