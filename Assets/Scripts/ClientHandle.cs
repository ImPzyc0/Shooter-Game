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
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
        
    }
    
    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }
    
    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        {
            _player.transform.position = _position;
        }
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        {
            _player.transform.rotation = _rotation;
        }
    }
    
    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();
        GameManager.instance.RemovePlayer(_id);
    }

    public static void PlayerShoot(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _destination = _packet.ReadVector3();
        if (_id == Client.instance.myId) return;
        if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        {
            //Aktuelle Waffe bekommen, dann .ShootVisibleBullet
            _player.gunManager.registeredGuns[_player.gunManager.activeGun].ShootVisibleBullet(_destination);
        }
    }

    public static void PlayerRespawned(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        float _health = _packet.ReadFloat();

        if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        {
            _player.Respawn(_position, _health);
        }
    }

    public static void PlayerWeaponSwitch(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int _weaponId = _packet.ReadInt();

        if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        {
            // Überprüfe, ob dieser Spieler einen GunManager für andere Spieler hat
            if(_player.gunManager != null)
            {
                // Rufe die NEUE, korrekte Methode auf
                _player.gunManager.SetWeaponFromServer(_weaponId);
            }
        }
    }
    
    
}
