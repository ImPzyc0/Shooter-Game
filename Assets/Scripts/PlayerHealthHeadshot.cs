using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthHeadshot : MonoBehaviour, IDamageable
{
    [SerializeField] public MonoBehaviour redirectDamage;
    [SerializeField] float headshotMultiplier;

    public void GotShot(float _damage)
    {
        ((IDamageable) redirectDamage).GotShot(_damage * headshotMultiplier);
    }
}
