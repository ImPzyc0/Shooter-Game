// In Scripts/PlayerManager.cs
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public float health;
    public float maxHealth = 100f;
    public OtherPlayerGunManager gunManager; // Referenz zum GunManager für andere Spieler

    private void Start()
    {
        health = maxHealth;
        // Falls es sich um einen anderen Spieler handelt, hole die Referenz
        if (id != Client.instance.myId)
        {
            gunManager = GetComponent<OtherPlayerGunManager>();
        }
    }
    
    public void SetHealth(float _health)
    {
        health = _health;
        if (health <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        // Deaktiviere das Spieler-Modell, um den Tod zu visualisieren
        // Du kannst hier auch eine Todesanimation starten
        gameObject.SetActive(false);
    }

    public void Respawn(Vector3 _position, float _health)
    {
        transform.position = _position;
        health = _health;
        // Aktiviere das Spieler-Modell wieder
        gameObject.SetActive(true);
    }
}