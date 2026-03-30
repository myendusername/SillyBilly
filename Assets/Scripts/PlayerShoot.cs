using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerShoot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject flameBulletPrefab;
    public GameObject muzzleFlarePrefab;

    public float bulletLifetime = 3f;
    public float bulletSpeed = 30f;
    public float shootingDelay = 0.2f;

    // shotgun setting
    public float horizontalSpread = 0f;
    public float verticalSpread = 0f;
    public int bulletAmount = 1;

    // flamethrower setting (new fire rate changes)
    public float flameSpeed = 15f;
    public float flameLifetime = 0.5f;

    public bool allowHold = false;

    private bool isShooting;
    private bool readyToShoot;

    private GameObject muzzleFlareObject;
    private ParticleSystem muzzleFlare;

    public ParticleSystem flameEffect; // bruh, flame is hard to make.

    int currentGun = 1; // 1, 2 , 3 keybinds for gun switch

    // ammo
    public int currentAmmo;
    public int maxAmmo;
    public float reloadTime = 1.5f;
    bool isReloading = false;

    // bad Ui cause i don't know how to do it
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI reloadText;

    // each gun fire rate
    private float currentShootingDelay;


    void Awake()
    {
        readyToShoot = true;

        muzzleFlareObject = Instantiate(muzzleFlarePrefab, firePoint.position, firePoint.rotation, firePoint);
        muzzleFlare = muzzleFlareObject.GetComponent<ParticleSystem>();

        SetWeaponStats();
        UpdateUI();
    }

    void Update()
    {
        // switch weapon
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentGun = 1;
            StopAllCoroutines();
            SetWeaponStats();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentGun = 2;
            StopAllCoroutines();
            SetWeaponStats();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentGun = 3;
            StopAllCoroutines();
            SetWeaponStats();
        }

        //switch guns
        if (isReloading) return;

        // reload
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }

        // shooting
        if (allowHold)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        // auto reload when empty. "was bugged when switching"
        if (currentAmmo <= 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }

        if (readyToShoot && isShooting)
        {
            Shoot();
        }

        // flame visuals but it not bad now.
        if (currentGun == 3 && isShooting && !isReloading)
        {
            if (flameEffect != null && !flameEffect.isPlaying)
            {
                flameEffect.Play();
            }
        }
        else
        {
            if (flameEffect != null && flameEffect.isPlaying)
            {
                flameEffect.Stop();
            }
        }
    }

    void SetWeaponStats()
    {
        // Normal gun
        if (currentGun == 1)
        {
            maxAmmo = 30;
            reloadTime = 1.5f;
            allowHold = true;
            currentShootingDelay = 0.2f;
        }

        // Shotgun
        if (currentGun == 2)
        {
            maxAmmo = 8;
            reloadTime = 2f;
            allowHold = false;
            currentShootingDelay = 0.6f;
        }

        // Flamethrower (change to liking cause i never made this before)
        if (currentGun == 3)
        {
            maxAmmo = 120;
            reloadTime = 2.5f;
            allowHold = true;
            currentShootingDelay = 0.05f;
        }

        currentAmmo = maxAmmo;
        isReloading = false;
        readyToShoot = true;
        if (reloadText != null) reloadText.text = "";
        UpdateUI();
    }

    void Shoot()
    {
        readyToShoot = false;

        // Normal gun
        if (currentGun == 1)
        {
            bulletAmount = 1;
            horizontalSpread = 0f;
            verticalSpread = 0f;
            allowHold = true;
        }

        // Shotgun
        if (currentGun == 2)
        {
            bulletAmount = 8;
            horizontalSpread = 10f;
            verticalSpread = 5f;
            allowHold = false;
        }

        // Flamethrower (chnage to liking mabe more spread cause fire)
        if (currentGun == 3)
        {
            bulletAmount = 1;
            horizontalSpread = 15f;
            verticalSpread = 15f;
            allowHold = true;
        }

        // reduce ammo
        if (currentGun == 3)
        {
            currentAmmo -= 2; // flame uses ammo faster cause maybe it op ? or causes burn to enviorment or enemy.
        }
        else
        {
            currentAmmo -= 1;
        }

        UpdateUI();

        for (int i = 0; i < bulletAmount; i++)
        {
            float x = Random.Range(-horizontalSpread, horizontalSpread);
            float y = Random.Range(-verticalSpread, verticalSpread);

            Quaternion spreadRotation = firePoint.rotation * Quaternion.Euler(y, x, 0);

            // spawn
            if (currentGun != 3)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, spreadRotation);

                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                rb.linearVelocity = bullet.transform.forward * bulletSpeed;

                StartCoroutine(DestroyBullet(bullet, bulletLifetime));
            }
        }

        muzzleFlare.Play(true);

        Invoke("ResetShot", currentShootingDelay);
    }

    void ResetShot()
    {
        readyToShoot = true;
    }

    IEnumerator Reload()
    {
        isReloading = true;
        if (reloadText != null) reloadText.text = "Reloading...";

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;

        if (reloadText != null) reloadText.text = "";
        UpdateUI();
    }

    void UpdateUI()
    {
        if (ammoText != null)
            ammoText.text = currentAmmo + " / " + maxAmmo;
    }

    IEnumerator DestroyBullet(GameObject bullet, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(bullet);
    }
}
