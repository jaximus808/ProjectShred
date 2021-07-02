using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        //ClientSend.WelcomeReceived();
        UIManager.instance.ClassSelectionShow();
        // Now that we have the client's id, connect UDP
        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
        ClientSend.ConnectionMade();
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int _classId = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();
        int _hp = _packet.ReadInt();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation, _classId, _hp);
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        if (!GameManager.players.ContainsKey(_id)) return;
        GameManager.players[_id].transform.position = _position;
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();
        if (!GameManager.players.ContainsKey(_id)) return;
        GameManager.players[_id].transform.rotation = _rotation;
    }

    public static void CreateProjectile(Packet _packet)
    {
        int _projectileTypeId = _packet.ReadInt();
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();
        GameManager.instance.CreateProjectile(_projectileTypeId,_id, _position, _rotation);
    }

    public static void UpdateProjectile(Packet _packet)
    {
        int _projectileId = _packet.ReadInt();
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();
        if (!GameManager.Projectiles[_projectileId].ContainsKey(_id)) return;
        GameManager.Projectiles[_projectileId][_id].transform.position = _position;
        GameManager.Projectiles[_projectileId][_id].transform.rotation = _rotation;
    }

    public static void RaiseEarthWall(Packet _packet)
    {
        int _wallId = _packet.ReadInt();
        Vector3 _newScale = _packet.ReadVector3();
        if (!GameManager.Projectiles[2].ContainsKey(_wallId)) return;
        GameManager.Projectiles[2][_wallId].transform.localScale = new Vector3(1f, _newScale.y, 1f) ;
       
    }

    public static void UpdateHealth(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int _curHp = _packet.ReadInt();
        GameManager.players[_id].UpdateHealth(_curHp);
        
    }

    public static void PlayerDisconnect(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
    }

    public static void RenderMessage(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _user = _packet.ReadString();
        string _msg = _packet.ReadString();
        bool _console = _packet.ReadBool();
        GameManager.instance.HandleMessage(_id, _user, _msg,_console);
    }

    public static void DeleteObject(Packet _packet)
    {
        int _projectileId = _packet.ReadInt();
        int _id = _packet.ReadInt();
        Destroy(GameManager.Projectiles[_projectileId][_id]);
        GameManager.Projectiles[_projectileId].Remove(_id);
    }

    public static void ServerPingCheck(Packet _packet)
    {
        long _sentMs = _packet.ReadLong();
        GameManager.instance.pingDisplay.text = $"Ping: {   DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - _sentMs}ms";
    }

}