using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ServerSend
{
    /// <summary>Sends a packet to a client via TCP.</summary>
    /// <param name="_toClient">The client to send the packet the packet to.</param>
    /// <param name="_packet">The packet to send to the client.</param>
    private static void SendTCPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].tcp.SendData(_packet);
    }

    /// <summary>Sends a packet to a client via UDP.</summary>
    /// <param name="_toClient">The client to send the packet the packet to.</param>
    /// <param name="_packet">The packet to send to the client.</param>
    private static void SendUDPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].udp.SendData(_packet);
    }

    /// <summary>Sends a packet to all clients via TCP.</summary>
    /// <param name="_packet">The packet to send.</param>
    private static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(_packet);
        }
    }
    /// <summary>Sends a packet to all clients except one via TCP.</summary>
    /// <param name="_exceptClient">The client to NOT send the data to.</param>
    /// <param name="_packet">The packet to send.</param>
    private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
    }

    /// <summary>Sends a packet to all clients via UDP.</summary>
    /// <param name="_packet">The packet to send.</param>
    private static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].udp.SendData(_packet);
        }
    }
    /// <summary>Sends a packet to all clients except one via UDP.</summary>
    /// <param name="_exceptClient">The client to NOT send the data to.</param>
    /// <param name="_packet">The packet to send.</param>
    private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
    }

    #region Packets
    /// <summary>Sends a welcome message to the given client.</summary>
    /// <param name="_toClient">The client to send the packet to.</param>
    /// <param name="_msg">The message to send.</param>
    public static void Welcome(int _toClient, string _msg)
    {
        using (Packet _packet = new Packet((int)ServerPackets.welcome))
        {
            _packet.Write(_msg);
            _packet.Write(_toClient);
            //_packet.Write(NetworkManager.NormalEarthAttacks.ToArray().Length);
            //foreach(KeyValuePair<int, NormalEarthAttack> normEarthAttack in NetworkManager.NormalEarthAttacks)
            //{
            //    _packet.Write(normEarthAttack.Key);
            //    _packet.Write(normEarthAttack.Value.transform.position);
            //    _packet.Write(normEarthAttack.Value.transform.rotation);
            //}
            SendTCPData(_toClient, _packet);
        }
    }

    /// <summary>Tells a client to spawn a player.</summary>
    /// <param name="_toClient">The client that should spawn the player.</param>
    /// <param name="_player">The player to spawn.</param>
    public static void SpawnPlayer(int _toClient, Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.classId);
            _packet.Write(_player.username);
            _packet.Write(_player.transform.position);
            _packet.Write(_player.transform.rotation);
            _packet.Write(_player.setHp);

            SendTCPData(_toClient, _packet);
        }
    }

    /// <summary>Sends a player's updated position to all clients.</summary>
    /// <param name="_player">The player whose position to update.</param>
    public static void PlayerPosition(int _id, Vector3 _position)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
        {
            _packet.Write(_id);
            _packet.Write(_position);

            SendUDPDataToAll(_packet);
        }
    }

    /// <summary>Sends a player's updated rotation to all clients except to himself (to avoid overwriting the local player's rotation).</summary>
    /// <param name="_player">The player whose rotation to update.</param>
    public static void PlayerRotation(int _id, Quaternion _rotation)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerRotation))
        {
            _packet.Write(_id);
            _packet.Write(_rotation);

            SendUDPDataToAll(_id, _packet);
        }
    }

    public static void CreateProjectile(int _projectileId, int _id, Vector3 _position, Quaternion _rotation, bool _setUp, int _clientId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.createProjectile))
        {
            _packet.Write(_projectileId);
            _packet.Write(_id);
            _packet.Write(_position);
            _packet.Write(_rotation);
            if (!_setUp)
            {
                SendUDPDataToAll(_packet);
            }
            else
            {
                SendUDPData(_clientId, _packet);
            }
        }
    }
    public static void UpdateProjectile(int _projectileId,int _id, Vector3 _position, Quaternion _rotation)
    {

        using (Packet _packet = new Packet((int)ServerPackets.updateProjectile))
        {
            _packet.Write(_projectileId);
            _packet.Write(_id);
            _packet.Write(_position);
            _packet.Write(_rotation);
            SendUDPDataToAll(_packet);
        }
    }

    public static void RaiseEarthWall(int _wallId, Vector3 _scale)
    {
        using (Packet _packet = new Packet((int)ServerPackets.raiseEarthWall))
        {
            _packet.Write(_wallId);
            _packet.Write(_scale);
            SendUDPDataToAll(_packet);
        }
    }

    public static void UpdateHealth(int _id, int _curHp)
    {
        using (Packet _packet = new Packet((int)ServerPackets.updateHealth))
        {
            _packet.Write(_id);
            _packet.Write(_curHp);
            SendUDPDataToAll(_packet);
        }
    }

    public static void UpdateCoolDown(int _id, float _auto, float _Q, float _C, float _E, float _R)
    {
        using (Packet _packet = new Packet((int)ServerPackets.abilityCoolDown))
        {
            _packet.Write(_auto);
            _packet.Write(_Q);
            _packet.Write(_C);
            _packet.Write(_E);
            _packet.Write(_R);
            SendUDPData(_id,_packet);
        }
    }

    public static void PlayerDisconnected(int _id)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerDisconnected))
        {
            _packet.Write(_id);
            SendTCPDataToAll(_packet);
        }
    }

    public static void RenderMessage(int _id,string _user, string _message, bool _console)
    {
        using (Packet _packet = new Packet((int)ServerPackets.rendMessage))
        {
            _packet.Write(_id);
            _packet.Write(_user);
            _packet.Write(_message);
            _packet.Write(_console);
            SendUDPDataToAll(_packet);
        }
    }

    public static void DeleteObject(int _projectileId, int _id)
    {
        using (Packet _packet = new Packet((int)ServerPackets.deleteObject))
        {
            _packet.Write(_projectileId);
            _packet.Write(_id);
            SendTCPDataToAll(_packet);
        }
    }

    public static void ReturnPingCheck(int _id, long _ms)
    {
        using (Packet _packet = new Packet((int)ServerPackets.serverPingCheck))
        {
            _packet.Write(_ms);
            SendUDPData(_id, _packet);
        }
    }

    #endregion
}