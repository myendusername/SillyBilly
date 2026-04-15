using UnityEngine;
using TMPro;

public class PlayerShoot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject gunArt;
    public GameObject bulletPrefab;
    public GameObject muzzleFlarePrefab;
    public AudioSource firingAudio;

    public float bulletLifetime = 3f;
    public float bulletSpeed = 30f;
    public int bulletDamage = 2;
    public float shootingDelay = 0.2f;
    [Tooltip("In degrees, used for both left and right separately.")]
    public float horizontalSpread = 0f;
    [Tooltip("In degrees, used for both up and down separately.")]
    public float verticalSpread = 0f;
    public int bulletAmount = 1;
    public float firePointForwardOffset = 0f;
    public bool allowHold = false;
    public bool flamethrower = false;

    // bad Ui cause i don't know how to do it
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI weaponText;


    private bool activeShooter;
    private bool readyToShoot;

    private GameObject muzzleFlareObject;
    private ParticleSystem muzzleFlare;


    private void Awake()
    {
        muzzleFlareObject = Instantiate(muzzleFlarePrefab, firePoint.position, firePoint.rotation, firePoint);
        muzzleFlare = muzzleFlareObject.GetComponent<ParticleSystem>();
        gunArt.SetActive(false);
        SetWeaponStats();
    }

    void SetWeaponStats()
    {
        readyToShoot = true;
        if (ammoText != null) ammoText.text = "∞";
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

            Vector3 spawnPosition = firePoint.position + (spreadRotation * Vector3.forward) * firePointForwardOffset;
            if (flamethrower)
            {
                GameObject damageVolume = Instantiate(bulletPrefab, spawnPosition, spreadRotation);
                damageVolume.GetComponent<DamageVolume>().Setup(false, bulletLifetime, bulletDamage, LayerMask.GetMask("Enemy"), 2);
            }
            else
            {
                GameObject bullet = Instantiate(bulletPrefab, spawnPosition, spreadRotation);

                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                rb.linearVelocity = bullet.transform.forward * bulletSpeed;

                Projectile projectile = bullet.GetComponent<Projectile>();
                projectile.Setup(bulletLifetime, bulletDamage, "Enemy");
            }
        }

        if (!flamethrower)
        {
            muzzleFlare.Play(true);
            firingAudio.Play();
        }
        else
        {
            if (!muzzleFlare.isPlaying)
            {
                muzzleFlare.Play(true);
            }
            if (!firingAudio.isPlaying)
            {
                firingAudio.Play();
            }
        }

        Invoke("ResetShot", shootingDelay);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    public void SetShooting(bool state)
    {
        if (flamethrower && state == false)
        {
            muzzleFlare.Stop(true);
            firingAudio.Stop();
        }

        activeShooter = state;
    }
}
