using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : GunWithSight
{
    [SerializeField] GameObject settingsScreen;

    // Update is called once per frame
    void Update()
    {
        if (!active) return;

        if (settingsScreen.activeSelf) return;
        if (CheckShootKey()) { Shoot(); return; }
        if (CheckReloadKey()) Reload();

        if (adsAnimation) UpdateFOV();
        if (CheckADS() && !adsAnimation) ADS();
        if (CheckApplyRecoil()) ApplyRecoil();
        else ResetRecoil();
        if (reloadingCurrently) WhileReloading();

        
    }

    protected override bool CheckShootKey()
    {
        return Input.GetKeyDown(KeyCode.Mouse0) && currentAmmo > 0 && Time.time >= lastShootTime + (1f / fireRate) && !reloadingCurrently && !adsAnimation;
    }

}
