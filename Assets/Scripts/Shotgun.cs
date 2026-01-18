using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : GunWithSpread
{
    [SerializeField] GameObject settingsScreen;

    // Update is called once per frame
    void Update()
    {
        if (!active) return;

        if (settingsScreen.activeSelf) return;
        if (CheckShootKey()) { Shoot(); return; }
        if (CheckReloadKey()) Reload();

        if (CheckApplyRecoil()) ApplyRecoil();
        else ResetRecoil();
        if (reloadingCurrently) WhileReloading();

       
    }
}
