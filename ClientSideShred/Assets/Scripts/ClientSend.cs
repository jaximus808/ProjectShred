using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    /// <summary>Sends a packet to the server via TCP.</summary>
    /// <param name="_packet">The packet to send to the sever.</param>
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    /// <summary>Sends a packet to the server via UDP.</summary>
    /// <param name="_packet">The packet to send to the sever.</param>
    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    public static void ConnectionMade()
    {
        using (Packet _packet = new Packet((int)ClientPackets.initRender) )
        {
            _packet.Write(Client.instance.myId);
            Debug.Log("wtf");
            SendTCPData(_packet);
        }
    }

    /// <summary>Lets the server know that the welcome message was received.</summary>
    public static void WelcomeReceived(int _classId)
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.usernameField.text);
            _packet.Write(_classId);
            Debug.Log(UIManager.instance.usernameField.text);
            SendTCPData(_packet);
        }
    }

    /// <summary>Sends player input to the server.</summary>
    /// <param name="_inputs"></param>
    public static void PlayerMovement(bool[] _inputs, Quaternion _cameraRot)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputs.Length);
            foreach (bool _input in _inputs)
            {
                _packet.Write(_input);
            }
            _packet.Write(GameManager.players[Client.instance.myId].transform.rotation);
            _packet.Write(_cameraRot);
            SendUDPData(_packet);
        }
    }

    public static void SendChatMessage(string _chatMessage)
    {
        using (Packet _packet = new Packet((int)ClientPackets.sendChatMsg))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(_chatMessage);
            SendUDPData(_packet);
        }
    }

    public static void PingCheck()
    {
        using (Packet _packet = new Packet((int)ClientPackets.pingCheck))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            SendUDPData(_packet);
        }
    }

    #endregion
}