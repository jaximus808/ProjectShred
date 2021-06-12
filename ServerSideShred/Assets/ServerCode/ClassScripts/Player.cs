using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int classId;
    public int id;
    public string username;
    public int setHp;
    public EarthBenderPlayer earthPlayer;

    public void StartDataToClass(int _id, string _username)
    {
        id = _id;
        username = _username;
        switch (classId)
        {
            case 0:
                setHp = earthPlayer.Initialize(this);
                break;
        }
    }

    public void SendInput(bool[] _inputs,Quaternion _rotation,Quaternion _headRotation)
    {
        switch(classId)
        {
            case 0:
                earthPlayer.SetInput(_inputs, _rotation, _headRotation);
                break;
        }
    }

    public void CleanUp()
    {
        switch(classId)
        {
            case 0:
                earthPlayer.Disconnect();
                break;
        }
    }
}