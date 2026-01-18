using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{

    public Gun[] registeredGuns;
    [SerializeField] int startGun;
    public int activeGun;

    private void Awake()
    {
        activeGun = startGun;
    }

    void Update()
    {
        if (Input.mouseScrollDelta.y == 0) return;
        ChangeGun((int) Input.mouseScrollDelta.y);
    }

    protected void ChangeGun(int by)
    {
        
        int newGun = activeGun - by;
        
        while(newGun < 0 )
        {
            newGun = registeredGuns.Length + newGun;
        }
        while(newGun >= registeredGuns.Length)
        {
            newGun = newGun - registeredGuns.Length;
        }

        SetActiveGun(newGun);
        
    }

    protected virtual void SetActiveGun(int _gun)
    {
        if (!registeredGuns[activeGun].CheckSwitch()) return;

        registeredGuns[activeGun].gameObject.SetActive(false);
        registeredGuns[activeGun].UnloadGun();

        activeGun = _gun;

        registeredGuns[_gun].gameObject.SetActive(true);
        registeredGuns[_gun].LoadGun(true);
        
        ClientSend.PlayerWeaponSwitch(activeGun);
    }

}
