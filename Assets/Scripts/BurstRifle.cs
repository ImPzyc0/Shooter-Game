using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstRifle : BurstGunWithSight
{
    [SerializeField] GameObject settingsScreen;


    void Update()
    {
        if (!active) return;

        if (settingsScreen.activeSelf) return;
        if (CheckShootKey()) { ShootBurst(); return; }
        if (CheckReloadKey()) Reload();

        if (adsAnimation) UpdateFOV();
        if (CheckADS() && !adsAnimation) ADS();
        if (CheckApplyRecoil()) ApplyRecoil();
        else ResetRecoil();
        if (reloadingCurrently) WhileReloading();

        
    }
}
