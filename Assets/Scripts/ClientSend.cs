using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }
    
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(Client.username);

            SendTCPData(_packet);
        }
    }
    
    public static void PlayerMovement(Vector3 _position, Quaternion _rotation)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_position);
            _packet.Write(_rotation);
            SendUDPData(_packet);
        }
    }

    public static void PlayerShoot(Vector3 _destination)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerShoot))
        {
            _packet.Write(_destination);
            SendUDPData(_packet);
        }
    }

    public static void PlayerDamage(int _playerId, float _damage)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerDamage))
        {
            _packet.Write(_playerId);
            _packet.Write(_damage);
            SendTCPData(_packet);
        }
    }
    
    public static void PlayerWeaponSwitch(int _weaponIndex)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerWeaponSwitch))
        {
            _packet.Write(_weaponIndex);
            SendTCPData(_packet);
        }
    }
    
}
