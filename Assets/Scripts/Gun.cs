using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;


public class Gun : MonoBehaviour
{
    [SerializeField] protected Animator gunAnimator;

    [SerializeField] protected KeyCode reloadKey;

    [SerializeField] protected float reloadTime;
    protected float lastReloadtime = -10f;
    protected bool reloadingCurrently = false;
    [SerializeField] protected int maxAmmo;
    protected int currentAmmo;
    [SerializeField] protected float damage;
    [SerializeField] protected float fireRate;
    [SerializeField] protected string shootAnimationTrigger;
    [SerializeField] protected string pulloutAnimationTrigger;
    [SerializeField] protected float pulloutTime;
    public bool active;

    [SerializeField] protected Transform camPos;
    [SerializeField] protected Transform infrontOfCam;
    [SerializeField] protected Movement movementScript;
    [SerializeField] protected MainMenuManager menuManager;

    [SerializeField] protected Vector2[] recoil;
    [SerializeField][Range(0, 5)] protected float recoilMultiplier; 
    [SerializeField] protected float xRecoilReset;
    [SerializeField] protected float yRecoilReset;
    [SerializeField] protected CameraMovement camMovement;
    protected float xRecoilResetParts = -5;
    protected float xRecoilResetStart;
    protected int currentBulletInRecoil;
    protected bool currentlyApplyingRecoil = false;
    protected float alreadyAppliedRecoilX = 0;
    protected float alreadyAppliedRecoilY = 0;

    [SerializeField] protected float standardBloom;
    [SerializeField] protected float movementBloomMultiplier;
    [SerializeField] protected float maxMovementBloom;
    [SerializeField] protected float jumpBloomAdder;
    [SerializeField] protected Transform infrontOfCamHolder;

    protected float lastShootTime = -10f;

    [SerializeField] protected Transform barrelEnd;

    [SerializeField] protected string reloadTrigger;
    [SerializeField] protected string reloadAnimSpeedMultiParam;
    [SerializeField] protected float reloadAnimationTime;
    [SerializeField] protected ParticleSystem muzzleFlash;

    [SerializeField] protected GameObject bullet;
    [SerializeField] GameObject bulletHole;

    [SerializeField] AudioSource gunAudioSource;
    [SerializeField] AudioClip gunShootSound;
    [SerializeField] AudioClip gunReloadSound;
    [SerializeField] AudioClip gunCockSound;

    [Header("DamageDrop: range from 0 : damage multiplier")]
    [SerializeField] Vector2[] damageDrop;

    private void Start()
    {
        if (gunAnimator == null)
        {
            return;
        }
        
        currentAmmo = maxAmmo;
        currentBulletInRecoil = maxAmmo;
            UpdateAmmoText();

        gunAnimator.SetFloat(reloadAnimSpeedMultiParam, reloadAnimationTime / reloadTime);
    }

    protected virtual bool CheckReloadKey()
    {
        return Input.GetKeyDown(reloadKey) && currentAmmo != maxAmmo && !reloadingCurrently;
    }

    protected virtual void Reload()
    {
        ReloadAnimation();

        lastReloadtime = Time.time;
        reloadingCurrently = true;
    }

    protected virtual void WhileReloading()
    {
        if (Time.time > lastReloadtime + reloadTime) EndOfReload();
    }

    protected virtual void EndOfReload()
    {
        reloadingCurrently = false;
        currentAmmo = maxAmmo;

        EndReloadAnimation();
        UpdateAmmoText();
    }

    protected virtual bool CheckShootKey()
    {
        return Input.GetKey(KeyCode.Mouse0) && currentAmmo > 0 && Time.time >= lastShootTime + (1f / fireRate) && !reloadingCurrently;
    }

    protected virtual bool CheckApplyRecoil()
    {
        return Time.time < lastShootTime + (1f / fireRate) && !reloadingCurrently;
    }

    protected virtual void Shoot()
    {

        //Generate rotation and direction of the shot randomly based on movement velocity and recoil
        float bloom = (GetMovementBloom() + standardBloom) * Mathf.Sqrt(Random.Range(0, 1f));
        float randomRot = Random.Range(0f, 360f);
        infrontOfCamHolder.localEulerAngles = new Vector3(infrontOfCamHolder.localEulerAngles.x, infrontOfCamHolder.localEulerAngles.y, infrontOfCamHolder.localEulerAngles.z+randomRot);

        infrontOfCam.localPosition = new Vector3(0, infrontOfCam.localPosition.y+bloom, infrontOfCam.localPosition.z);

        currentAmmo--;

        currentBulletInRecoil--;

        alreadyAppliedRecoilX = 0;
        alreadyAppliedRecoilY = 0;


        lastShootTime = Time.time;

        //Raycast from the cam to the point which was set beforehand, check if it hit anything
        if(Physics.Linecast(camPos.position, infrontOfCam.position, out RaycastHit hit))
        {
            if(hit.collider.TryGetComponent(out IDamageable shotAt))
            {
                //shotAt.GotShot(GetDamage(hit.distance));

                // Überprüfe, ob ein anderer Spieler getroffen wurde
                PlayerManager player = hit.collider.GetComponent<PlayerManager>();
                if (player != null && player.id != Client.instance.myId)
                {
                    ClientSend.PlayerDamage(player.id, GetDamage(hit.distance)); // damage ist der Schaden deiner Waffe
                }
                
                ShootVisibleBullet(hit.point);
            }
            else
            {
                ShootVisibleBullet(hit.point);
            }
            DrawBulletHole(hit.point, hit.normal);
        }
        else
        {
            ShootVisibleBullet(infrontOfCam.position);
        }

        //Reset the point again to randomize for next shoot()
        infrontOfCam.localPosition = new Vector3(0, infrontOfCam.localPosition.y - bloom, infrontOfCam.localPosition.z);
        infrontOfCamHolder.localEulerAngles = new Vector3(infrontOfCamHolder.localEulerAngles.x, infrontOfCamHolder.localEulerAngles.y, infrontOfCamHolder.localEulerAngles.z - randomRot);

        UpdateAmmoText();
        ShootAnimation();
        ApplyRecoil();
        
        ClientSend.PlayerShoot(infrontOfCam.position);
    }

    public virtual void ShootVisibleBullet(Vector3 _to)
    {
        muzzleFlash.transform.LookAt(_to);
        muzzleFlash.Play();

        PlaySound(gunShootSound);

        Bullet bulletScript = Instantiate(bullet, barrelEnd.position, Quaternion.identity).GetComponent<Bullet>();
        bulletScript.gameObject.transform.LookAt(_to);
        bulletScript.distance = Vector3.Distance(_to, barrelEnd.position);
    }

    protected virtual void ShootAnimation()
    {
        gunAnimator.SetTrigger(shootAnimationTrigger);
    }

    protected virtual void ReloadAnimation()
    {
        gunAnimator.SetTrigger(reloadTrigger);

        PlaySound(gunReloadSound);
    }

    protected virtual void EndReloadAnimation()
    {
        PlaySound(gunCockSound);

    }

    protected void UpdateAmmoText()
    {
        menuManager.UpdateAmmo(currentAmmo, maxAmmo);
    }

    private void PlaySound(AudioClip _clip)
    {
        gunAudioSource.clip = _clip;
        gunAudioSource.Play();
    }

    protected virtual void ApplyRecoil()
    {
        //Apply the recoil gradually over time till the next shot to avoid stiffness
        //Clamping the value to ensure that due to not perfect time there won't be a little more recoil than there should be

        float xRot = recoilMultiplier * recoil[currentBulletInRecoil].x * Time.deltaTime * fireRate + alreadyAppliedRecoilX;
        float yRot = recoilMultiplier * recoil[currentBulletInRecoil].y * Time.deltaTime * fireRate + alreadyAppliedRecoilY;

        xRot = Mathf.Clamp(xRot, recoil[currentBulletInRecoil].x * recoilMultiplier, 0);
        yRot = Mathf.Clamp(yRot, Mathf.Min(recoil[currentBulletInRecoil].y, -recoil[currentBulletInRecoil].y) * recoilMultiplier, Mathf.Max(recoil[currentBulletInRecoil].y, -recoil[currentBulletInRecoil].y) * recoilMultiplier);

        camMovement.extraXRotation += xRot - alreadyAppliedRecoilX;
        camMovement.extraYRotation += yRot - alreadyAppliedRecoilY;

        alreadyAppliedRecoilX = xRot;
        alreadyAppliedRecoilY = yRot;

        currentlyApplyingRecoil = true;
    }

    protected virtual void ResetRecoil()
    {

        //Set the starting points for resetting the recoil if necessery
        if (currentlyApplyingRecoil)
        {
            alreadyAppliedRecoilX = 0;
            alreadyAppliedRecoilY = 0;

            xRecoilResetParts = camMovement.extraXRotation / (maxAmmo - currentBulletInRecoil);
            xRecoilResetStart = camMovement.extraXRotation-0.02f;

            currentlyApplyingRecoil = false;
        }

        if (camMovement.extraXRotation >= -0.04f)
        {
            currentBulletInRecoil = maxAmmo;
        }
        if (currentBulletInRecoil == maxAmmo) return;

        //Check what Bullet in recoil it is currently
        while (xRecoilResetStart - xRecoilResetParts <= camMovement.extraXRotation)
        {
            xRecoilResetStart -= xRecoilResetParts;

            currentBulletInRecoil++;
        }
        
        //Set the extraX and extraY rotation back to 0
        camMovement.extraXRotation = camMovement.extraXRotation > 0 ? Mathf.Clamp(camMovement.extraXRotation - xRecoilReset * Time.deltaTime, 0, 360f) : Mathf.Clamp(camMovement.extraXRotation + xRecoilReset * Time.deltaTime, -360f, 0);
        camMovement.extraYRotation = camMovement.extraYRotation > 0 ? Mathf.Clamp(camMovement.extraYRotation - yRecoilReset * Time.deltaTime, 0, 360f) : Mathf.Clamp(camMovement.extraYRotation + yRecoilReset * Time.deltaTime, -360f, 0);
    }

    protected virtual float GetMovementBloom()
    {
        Vector3 velocity = movementScript.GetVelocity();

        float relativeVelocity = Mathf.Sqrt(Mathf.Pow(velocity.x, 2) + Mathf.Pow(velocity.z, 2)) + ((velocity.y != 0) ? jumpBloomAdder: 0);

        float movementBloom = Mathf.Clamp(relativeVelocity * movementBloomMultiplier, 0, maxMovementBloom);

        return movementBloom;
    }

    protected virtual void DrawBulletHole(Vector3 _hitPos, Vector3 _hitDirection)
    {
        GameObject newBulletHole = Instantiate(bulletHole, _hitPos, Quaternion.LookRotation(_hitDirection));
        newBulletHole.transform.Translate(new Vector3(0,0, 0.01f));
    }

    protected virtual float GetDamage(float _range)
    {
        for (int i = damageDrop.Length - 1; i != -1; i--)
        {
            if (damageDrop[i].x < _range)
            {
                return damage * damageDrop[i].y;
            }
        }

        return 1;
    }

    public virtual void LoadGun(bool _active)
    {
        active = false;
        if(_active) UpdateAmmoText();
        gunAnimator.SetFloat(reloadAnimSpeedMultiParam, reloadAnimationTime / reloadTime);

        PulloutAnimation(_active);
    }

    public virtual void UnloadGun()
    {
        currentBulletInRecoil = maxAmmo;

        camMovement.extraXRotation = 0;
        camMovement.extraYRotation = 0;
        active = false;
    }

    public virtual bool CheckSwitch()
    {
        return (!reloadingCurrently && active);
    }

    protected void PulloutAnimation(bool _active)
    {
        gunAnimator.SetTrigger(pulloutAnimationTrigger);

        StartCoroutine(EndPulloutAnimation(_active));
    }

    protected IEnumerator EndPulloutAnimation(bool _active)
    {
        yield return new WaitForSeconds(pulloutTime);

        active = _active;
    }
}
