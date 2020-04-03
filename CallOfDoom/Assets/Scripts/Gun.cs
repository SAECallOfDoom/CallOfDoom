using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Gun : MonoBehaviour
{
    /// <summary>
    /// Private reference to player. -PlayerMovement.
    /// </summary>
    [SerializeField]
    private PlayerMovement player;


    /// <summary>
    /// Private damage value. -Float.
    /// </summary>
    [SerializeField]
    private float damage;

    /// <summary>
    /// Private range value.-Float
    /// </summary>
    [SerializeField]
    private float range = 100.0f;

    [SerializeField]
    private Camera fpsCam;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float maximumTotalAmmo;

    [SerializeField]
    private float maximumClipAmmo;

    private float totalAmmoCount;

    private float clipAmmoCount;

    //Determines how fast we can shoot
    private float fireRate = 0.7f;

    [SerializeField]
    private float backFireAmount = 60f;

    private float nextFire = 0.0f;

    private bool isReloadAnimFinished = true;

    [SerializeField]
    private Text ammoText;

    private void Start()
    {
        clipAmmoCount = maximumClipAmmo;
        totalAmmoCount = maximumTotalAmmo;
    }
    void Update()
    {
        if (totalAmmoCount <= 0)
            totalAmmoCount = 0;

        ammoText.text = clipAmmoCount.ToString() + " / " + totalAmmoCount.ToString();

        if (Input.GetKey(KeyCode.Mouse0) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
            Reload();
    }

    private void Shoot()
    {
        if(isReloadAnimFinished && clipAmmoCount > 0)
        {
            animator.SetTrigger("gunSpin");
            UseAmmo();

            player.AddImpact(-fpsCam.transform.forward, backFireAmount);

            RaycastHit hitInfo;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hitInfo, range))
            {
                DestroyableObject destroyableObject = hitInfo.transform.GetComponent<DestroyableObject>();
                Enemy enemy = hitInfo.transform.GetComponentInParent<Enemy>();


                if (hitInfo.transform.gameObject.tag == "DestroyableObject")
                {
                    destroyableObject.gameObject.GetComponent<BoxCollider>().enabled = false;
                    destroyableObject.ExplodeObject();
                }

                else if (hitInfo.transform.gameObject.tag == "Enemy")
                {
                    enemy.TakeDamage(damage);
                }

            }
        }
    }
    private void Reload()
    {
        if(totalAmmoCount > 0 && clipAmmoCount != maximumClipAmmo)
        {
            float tmp = maximumClipAmmo - clipAmmoCount;
            clipAmmoCount = Mathf.Clamp(totalAmmoCount,1f,maximumClipAmmo);
            totalAmmoCount -= tmp;
            //totalAmmoCount -= clipAmmoCount;
            animator.SetTrigger("reload"); 
        }
    }
    private void UseAmmo()
    {
        if(clipAmmoCount > 0 )
        {
            clipAmmoCount--;
        }

        if (clipAmmoCount == 0)
            Reload();   

        if (clipAmmoCount == 0 && totalAmmoCount > 0)
            animator.SetTrigger("reload");
    }

    private void ReloadAnimationStarted()
    {
        isReloadAnimFinished = false;
    }
    private void ReloadAnimationFinished()
    {
        isReloadAnimFinished = true;
    }

}
