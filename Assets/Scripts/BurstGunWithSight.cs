using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstGunWithSight : BurstGun
{
    protected bool ads = false;
    protected bool adsAnimation = false;
    protected float standardFOV;
    [SerializeField] Camera cam;
    [SerializeField] float adsFOV;
    [SerializeField] float adsAnimationLength;
    [SerializeField] string adsAnimationInTrigger;
    [SerializeField] string adsAnimationOutTrigger;
    [SerializeField][Range(0, 3)] float adsMovementBloomMultiplier;
    [SerializeField][Range(0, 10)] float adsStandardBloomLess;
    [SerializeField][Range(0, 20)] float adsRecoilX;
    [SerializeField][Range(0, 20)] float adsRecoilYMax;
    [SerializeField][Range(0, 20)] float adsRecoilYMin;

    private void Awake()
    {
        if (!active) return;
        standardFOV = cam.fieldOfView;
    }

    protected override void Reload()
    {
        ReloadAnimation();

        ads = false;

        cam.fieldOfView = standardFOV;

        menuManager.SetCrosshairActive(!ads);

        lastReloadtime = Time.time;
        reloadingCurrently = true;
    }

    protected override bool CheckShootKey()
    {
        return base.CheckShootKey() &&  !adsAnimation;
    }

    protected virtual bool CheckADS()
    {
        return (Input.GetMouseButton(1) && !currentlyApplyingRecoil && !reloadingCurrently && !adsAnimation && !ads) || (!(Input.GetMouseButton(1)) && ads);
    }

    protected virtual void ADS()
    {
        ads = !ads;
        adsAnimation = true;

        ADSAnimation();
        menuManager.SetCrosshairActive(!ads);
    }

    IEnumerator EndADSAnimation()
    {
        yield return new WaitForSeconds(adsAnimationLength);
        cam.fieldOfView = ads ? adsFOV : standardFOV;
        adsAnimation = false;
    }

    protected virtual void ADSAnimation()
    {
        gunAnimator.SetTrigger(ads ? adsAnimationInTrigger : adsAnimationOutTrigger);

        StartCoroutine(EndADSAnimation());
    }

    protected override void ShootAnimation()
    {
        if (!ads) base.ShootAnimation();
    }

    protected override float GetMovementBloom()
    {
        return ads ? base.GetMovementBloom() * adsMovementBloomMultiplier : base.GetMovementBloom();
    }

    protected override void ApplyRecoil()
    {
        if (!ads)
        {
            base.ApplyRecoil();
        }
        else
        {
            camMovement.MoveCam(((int)((Random.Range(-1, 1) + 0.5f) * 2)) * Random.Range(adsRecoilYMin, adsRecoilYMax) * Time.deltaTime, adsRecoilX * Time.deltaTime);
        }
    }

    protected override void Shoot()
    {
        standardBloom -= adsStandardBloomLess;
        base.Shoot();
        standardBloom += adsStandardBloomLess;
    }

    protected virtual void UpdateFOV()
    {
        if (ads)
        {
            cam.fieldOfView = cam.fieldOfView - ((standardFOV - adsFOV) / adsAnimationLength) * Time.deltaTime;
        }
        else
        {
            cam.fieldOfView = cam.fieldOfView + ((standardFOV - adsFOV) / adsAnimationLength) * Time.deltaTime;
        }
    }

    public override void UnloadGun()
    {
        base.UnloadGun();
    }

    public override bool CheckSwitch()
    {
        if(ads || adsAnimation)
        {
            return false;
        }
        return base.CheckSwitch();
    }
}
