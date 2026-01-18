using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiAutomaticGun : Gun
{
    protected override bool CheckShootKey()
    {
        return Input.GetKeyDown(KeyCode.Mouse0) && currentAmmo > 0 && Time.time > lastShootTime + (1f / fireRate) && !reloadingCurrently;
    }
}
