using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayerHealth : MonoBehaviour, IDamageable
{
    public void GotShot(float _damage)
    {
        //When another Player gets shot by the PlayerSelf + the damage
        //print("Shot!");
    }
}
