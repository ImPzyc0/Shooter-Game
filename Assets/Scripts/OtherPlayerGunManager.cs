// In Scripts/OtherPlayerGunManager.cs
using UnityEngine;

public class OtherPlayerGunManager : GunManager
{
    // Die 'changeActiveGunBy' Logik wird nicht mehr benötigt und kann entfernt werden.

    // NEUE ÖFFENTLICHE METHODE
    public void SetWeaponFromServer(int _weaponIndex)
    {
        // Stellt sicher, dass der Index im gültigen Bereich liegt
        if (_weaponIndex < 0 || _weaponIndex >= registeredGuns.Length)
        {
            Debug.LogWarning($"Ungültiger Waffen-Index {_weaponIndex} vom Server erhalten.");
            return;
        }

        // Deaktiviere die aktuell aktive Waffe
        registeredGuns[activeGun].gameObject.SetActive(false);

        // Setze den neuen Index
        activeGun = _weaponIndex;

        // Aktiviere die neue Waffe
        registeredGuns[activeGun].gameObject.SetActive(true);
        // LoadGun(false), da es sich nicht um den lokalen Spieler handelt
        registeredGuns[activeGun].LoadGun(false);
    }

    // Die alte SetActiveGun-Methode wird für andere Spieler nicht direkt aufgerufen,
    // kann aber zur Sicherheit überschrieben bleiben.
    protected override void SetActiveGun(int _gun)
    {
        // Diese Methode sollte für "OtherPlayer" nicht mehr direkt verwendet werden.
        // Der Waffenwechsel wird jetzt durch SetWeaponFromServer gesteuert.
        SetWeaponFromServer(_gun);
    }

    // Die Update-Methode kann entfernt oder leer gelassen werden, da
    // 'changeActiveGunBy' nicht mehr verwendet wird.
    void Update() {}
}