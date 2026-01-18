using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{

    [SerializeField] MainMenuManager mainMenuManager;
    [SerializeField] float maxHP;
    private float hp;
    

    public void GotShot(float _damage)
    {
        hp -= _damage;

        UpdateHealth();

        if (hp <= 0)
        {
            hp = maxHP;
        }
    }

    void Start()
    {
        hp = maxHP;

        UpdateHealth();
    }


    void UpdateHealth()
    {
        mainMenuManager.UpdateHealth(hp, maxHP);
    }
}
