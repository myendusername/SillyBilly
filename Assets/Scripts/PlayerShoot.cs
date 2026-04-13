using System.Collections;
using System.Data.SqlTypes;
using UnityEngine;
using TMPro;

public class PlayerShoot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject muzzleFlarePrefab;
    public GameObject flameBulletPrefab;
    public AudioSource FiringAudio;

    public float bulletLifetime = 3f;
    public float bulletSpeed = 30f;
    public int bulletDamage = 2;
    public float shootingDelay = 0.2f;
    [Tooltip("In degrees, used for both left and right separately.")]
    public float horizontalSpread = 0f;
    [Tooltip("In degrees, used for both up and down separately.")]
    public float verticalSpread = 0f;
    public int bulletAmount = 1;
    public bool allowHold = false;

    // bad Ui cause i don't know how to do it
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI weaponText;


    private bool activeShooter;
    private bool readyToShoot;
    // each gun fire rate
    public int currentGun;

    private GameObject muzzleFlareObject;
    private ParticleSystem muzzleFlare;


    // flamethrower setting (new fire rate changes)
    public float flameSpeed = 15f;
    public float flameLifetime = 0.5f;
    public ParticleSystem flameEffect; // bruh, flame is hard to make.

    private float currentShootingDelay;


    private void Awake()
    {
        muzzleFlareObject = Instantiate(muzzleFlarePrefab, firePoint.position, firePoint.rotation, firePoint);
        muzzleFlare = muzzleFlareObject.GetComponent<ParticleSystem>();
        SetWeaponStats();
    }

    void SetWeaponStats()
    {
        // Normal gun
        if (currentGun == 1)
        {
            allowHold = true;
            currentShootingDelay = 0.2f;
            bulletAmount = 1;
            horizontalSpread = 0f;
            verticalSpread = 0f;
        }

        // Shotgun
        if (currentGun == 2)
        {
            allowHold = false;
            currentShootingDelay = 0.6f;
            bulletAmount = 8;
            horizontalSpread = 10f;
            verticalSpread = 5f;
        }

        // Flamethrower (change to liking cause i never made this before)
        if (currentGun == 3)
        {
            allowHold = true;
            currentShootingDelay = 0.05f;
            bulletAmount = 1;
            horizontalSpread = 15f;
            verticalSpread = 15f;
        }

        readyToShoot = true;
        if (ammoText != null) ammoText.text = "∞";
        UpdateWeaponText();
    }

    void UpdateWeaponText()
    {
        if (weaponText == null) return;

        if (currentGun == 1) weaponText.text = "Rifle";
        if (currentGun == 2) weaponText.text = "Shotgun";
        if (currentGun == 3) weaponText.text = "Flamethrower";
    }

    private void Start()
    {
        readyToShoot = true;
    }

    void Update()
    {
        if (allowHold)
        {
            if (readyToShoot && activeShooter)
            {
                Shoot();
            }
        }
        else
        {
            if (readyToShoot && activeShooter)
            {
                Shoot();
                activeShooter = false;
            }
        }

        // flame visuals but it not bad now.
        if (currentGun == 3 && activeShooter)
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

    void Shoot()
    {
        readyToShoot = false;

        for (int i = 0; i < bulletAmount; i++)
        {
            // Spread
            float x = Random.Range(-horizontalSpread, horizontalSpread);
            float y = Random.Range(-verticalSpread, verticalSpread);

            Quaternion spreadRotation = firePoint.rotation * Quaternion.Euler(y, x, 0);

            if (currentGun == 1 || currentGun == 2)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, spreadRotation);

                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                rb.linearVelocity = bullet.transform.forward * bulletSpeed;

                Projectile projectile = bullet.GetComponent<Projectile>();
                projectile.Setup(bulletLifetime, bulletDamage, "Enemy");

                StartCoroutine(DestroyBullet(bullet, bulletLifetime));
            }
            else if (currentGun == 3)
            {
                GameObject flame = Instantiate(flameBulletPrefab, firePoint.position, spreadRotation);

                Rigidbody rb = flame.GetComponent<Rigidbody>();
                rb.linearVelocity = spreadRotation * Vector3.forward * flameSpeed;

                Projectile projectile = flame.GetComponent<Projectile>();
                projectile.Setup(bulletLifetime, bulletDamage, "Enemy");


                StartCoroutine(DestroyBullet(flame, flameLifetime));
            }
        }

        muzzleFlare.Play(true);
        FiringAudio.Play();

        Invoke("ResetShot", currentShootingDelay);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    public void SetShooting(bool state)
    {
        activeShooter = state;
    }

    IEnumerator DestroyBullet(GameObject bullet, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(bullet);
    }
}