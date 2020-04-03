using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwap : MonoBehaviour
{
    /// <summary>
    /// Private gun reference.-GameObject
    /// </summary>
    [SerializeField]
    private GameObject gun;


    /// <summary>
    /// Private sword reference.-GameObject
    /// </summary>
    [SerializeField]
    private GameObject sword;

    public bool isSwordActive;

    public bool isSGunActive;

    /// <summary>
    /// Private crosshair image reference
    /// </summary>
    [SerializeField]
    private Image crosshair;

    [SerializeField]
    private Text ammoText;


    public static bool canSwapWeapon = true;

    private void Start()
    {
        isSGunActive = true;
        isSwordActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1) && canSwapWeapon)
        {
            ammoText.gameObject.SetActive(true);
            sword.SetActive(false);
            gun.SetActive(true);
            crosshair.gameObject.SetActive(true);
            isSGunActive = true;
            isSwordActive = false;
        }

        if (Input.GetKey(KeyCode.Alpha2) && canSwapWeapon)
        {
            ammoText.gameObject.SetActive(false);
            gun.SetActive(false);
            sword.SetActive(true);
            crosshair.gameObject.SetActive(false);
            isSGunActive = false;
            isSwordActive = true;
        }
    }
}
