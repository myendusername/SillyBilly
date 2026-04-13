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

    // each gun fire rate
    private float currentShootingDelay;


    void Awake()
    {
        readyToShoot = true;

        muzzleFlareObject = Instantiate(muzzleFlarePrefab, firePoint.position, firePoint.rotation, firePoint);
        muzzleFlare = muzzleFlareObject.GetComponent<ParticleSystem>();

        SetWeaponStats();
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

        // shooting
        if (allowHold)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (readyToShoot && isShooting)
        {
            Shoot();
        }

        // flame visuals but it not bad now.
        if (currentGun == 3 && isShooting)
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
            allowHold = true;
            currentShootingDelay = 0.2f;
        }

        // Shotgun
        if (currentGun == 2)
        {
            allowHold = false;
            currentShootingDelay = 0.6f;
        }

        // Flamethrower (change to liking cause i never made this before)
        if (currentGun == 3)
        {
            allowHold = true;
            currentShootingDelay = 0.05f;
        }

        readyToShoot = true;
        if (UIManager.instance != null)
        {
            UIManager.instance.SetAmmo("∞");
            UpdateWeaponText();
        }
    }

    void UpdateWeaponText()
    {
        if (UIManager.instance == null) return;

        if (currentGun == 1) UIManager.instance.SetWeapon("Rifle");
        if (currentGun == 2) UIManager.instance.SetWeapon("Shotgun");
        if (currentGun == 3) UIManager.instance.SetWeapon("Flamethrower");
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

        for (int i = 0; i < bulletAmount; i++)
        {
            float x = Random.Range(-horizontalSpread, horizontalSpread);
            float y = Random.Range(-verticalSpread, verticalSpread);

            Quaternion spreadRotation = firePoint.rotation * Quaternion.Euler(y, x, 0);

            // spawn
            if (currentGun == 1 || currentGun == 2)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, spreadRotation);

                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                rb.linearVelocity = bullet.transform.forward * bulletSpeed;

                StartCoroutine(DestroyBullet(bullet, bulletLifetime));
            }
            else if (currentGun == 3)
            {
                GameObject flame = Instantiate(flameBulletPrefab, firePoint.position, spreadRotation);

                Rigidbody rb = flame.GetComponent<Rigidbody>();
                rb.linearVelocity = spreadRotation * Vector3.forward * flameSpeed;

                StartCoroutine(DestroyBullet(flame, flameLifetime));
            }
        }

        muzzleFlare.Play(true);

        Invoke("ResetShot", currentShootingDelay);
    }

    void ResetShot()
    {
        readyToShoot = true;
    }

    IEnumerator DestroyBullet(GameObject bullet, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(bullet);
    }
}
