using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunWithSpread : SemiAutomaticGun
{

    [SerializeField] Transform[] pelletLocations;
    [SerializeField][Range(1, 3)] float movementSpreadMultiplier;
    [SerializeField][Range(1, 5)] float jumpingSpreadMultiplier;

    protected override void Shoot()
    {
        foreach (Transform trans in pelletLocations)
        {
            Vector3 startRotation = trans.parent.parent.localEulerAngles;
            if (movementScript.GetVelocity().x != 0f || movementScript.GetVelocity().z != 0f)
            {
               trans.parent.parent.localEulerAngles = new Vector3(
                   (trans.parent.parent.localEulerAngles.x < 180 ? trans.parent.parent.localEulerAngles.x : (trans.parent.parent.localEulerAngles.x - 360)) * movementSpreadMultiplier, 
                   (trans.parent.parent.localEulerAngles.y < 180 ? trans.parent.parent.localEulerAngles.y : (trans.parent.parent.localEulerAngles.y - 360)) * movementSpreadMultiplier, 
                   trans.parent.parent.localEulerAngles.z);
            }
            if (movementScript.GetVelocity().y != 0f)
            {
               trans.parent.parent.localEulerAngles = new Vector3(
                   (trans.parent.parent.localEulerAngles.x < 180 ? trans.parent.parent.localEulerAngles.x : (trans.parent.parent.localEulerAngles.x - 360)) * jumpingSpreadMultiplier, 
                   (trans.parent.parent.localEulerAngles.y < 180 ? trans.parent.parent.localEulerAngles.y : (trans.parent.parent.localEulerAngles.y - 360)) * jumpingSpreadMultiplier, 
                   trans.parent.parent.localEulerAngles.z);
            }

            //Generate rotation and direction of the shot randomly based on movement velocity and recoil
            float bloom = (GetMovementBloom() + (standardBloom)) * Mathf.Sqrt(Random.Range(0, 1f));
            float randomRot = Random.Range(0f, 360f);
            trans.parent.localEulerAngles = new Vector3(trans.parent.localEulerAngles.x, trans.parent.localEulerAngles.y, trans.parent.localEulerAngles.z + randomRot);

            trans.localPosition = new Vector3(0, trans.localPosition.y + bloom, trans.localPosition.z);

            lastShootTime = Time.time;

            //Raycast from the cam to the point which was set beforehand, check if it hit anything
            if (Physics.Linecast(camPos.position, trans.position, out RaycastHit hit))
            {
                GetDamage(hit.distance);
                if (hit.collider.TryGetComponent(out IDamageable shotAt))
                {
                    shotAt.GotShot(GetDamage(hit.distance));
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
                ShootVisibleBullet(trans.position);
            }

            //Reset the point again to randomize for next shoot()
            trans.localPosition = new Vector3(0, trans.localPosition.y - bloom, trans.localPosition.z);
            trans.parent.localEulerAngles = new Vector3(trans.parent.localEulerAngles.x, trans.parent.localEulerAngles.y, trans.parent.localEulerAngles.z - randomRot);

            trans.parent.parent.localEulerAngles = startRotation;

        }

        currentAmmo--;

        currentBulletInRecoil--;

        UpdateAmmoText();
        ShootAnimation();
        ApplyRecoil();
    }

}
