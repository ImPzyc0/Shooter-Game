using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BurstGun : Gun
{
    [SerializeField] float waitAfterLastShot;
    [SerializeField] int burstShotAmount;

    //Overrides: checkshootkey, shoot

    protected override bool CheckShootKey()
    {
        return Input.GetKey(KeyCode.Mouse0) && currentAmmo > 0 && Time.time >= lastShootTime + (1f / (fireRate/burstShotAmount)) + waitAfterLastShot && !reloadingCurrently;
    }

    protected void ShootBurst()
    {
        Shoot();
        StartCoroutine(ShootAfterSeconds(burstShotAmount-1, 1f/fireRate));
    }

    private IEnumerator ShootAfterSeconds(int _times, float _delay)
    {
        yield return new WaitForSeconds(_delay);
        
        Shoot();
        _times = _times-1;
        if (_times > 0) {
            StartCoroutine(ShootAfterSeconds(_times, _delay));
        }

        
    }
}
