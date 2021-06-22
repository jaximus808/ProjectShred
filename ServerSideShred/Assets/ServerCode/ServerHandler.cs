using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();
        int _classId = _packet.ReadInt();

        //Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
        //if (_fromClient != _clientIdCheck)
        //{
        //    Debug.Log($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
        //}
        Debug.Log($"Player with ip: {Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} has taken the username {_username}");
        Server.clients[_fromClient].SendIntoGame(_username, _classId);
    }

    public static void RenderCurrentServerInit(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"new Player (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
        }
        Server.clients[_fromClient].SendOrginalPlayers();

    }

    public static void PlayerMovement(int _fromClient, Packet _packet)
    {
        bool[] _inputs = new bool[_packet.ReadInt()];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }
        Quaternion _rotation = _packet.ReadQuaternion();
        //maybe instead just send camera rotation instead of entire player
        Quaternion _headRotation = _packet.ReadQuaternion();
        Server.clients[_fromClient].player.SendInput(_inputs, _rotation, _headRotation);
    }
    public static void HandleChatMsg(int __fromClient, Packet _packet)
    {
        int _clientId = _packet.ReadInt();
        if (__fromClient != _clientId) return;
        string _message = _packet.ReadString();
        Debug.Log($"{Server.clients[__fromClient].player.username} has said {_message}");
        ServerSend.RenderMessage(_clientId,Server.clients[__fromClient].player.username, _message, false);
    }
}