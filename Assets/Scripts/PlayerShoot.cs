using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerShoot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject weaponArt;
    public GameObject bulletPrefab;
    public GameObject muzzleFlarePrefab;
    public AudioSource firingAudio;
    public float bulletLifetime = 3f;
    public float bulletSpeed = 30f;
    public int bulletDamage = 2;
    public float shootingDelay = 0.2f;
    public float burstDelay = 0f;
    [Tooltip("In degrees, used for both left and right separately.")]
    public float horizontalSpread = 0f;
    [Tooltip("In degrees, used for both up and down separately.")]
    public float verticalSpread = 0f;
    public int bulletAmount = 1;
    public float firePointForwardOffset = 0f;
    public bool allowHold = false;
    public bool flamethrower = false;

    // bad Ui cause i don't know how to do it
    public TextMeshProUGUI weaponText;

    private bool activeShooter;
    private bool readyToShoot;
    // i wrote this
    private float cooldownTimer = 0f;

    private GameObject muzzleFlareObject;
    private ParticleSystem muzzleFlare;
    private Animator animator;


    private void Awake()
    {
        muzzleFlareObject = Instantiate(muzzleFlarePrefab, firePoint.position, firePoint.rotation, firePoint);
        muzzleFlare = muzzleFlareObject.GetComponent<ParticleSystem>();
        if (weaponArt)
        {
            weaponArt.SetActive(false);
            animator = weaponArt.GetComponent<Animator>();
        }
    }

    private void Start()
    {
        readyToShoot = true;
    }

    void Update()
    {
        // count up while on cooldown
        if (!readyToShoot)
            cooldownTimer += Time.deltaTime;

        if (allowHold)
        {
            if (readyToShoot && activeShooter)
            {
                StartCoroutine(Shoot());
            }
        }
        else
        {
            if (readyToShoot && activeShooter)
            {
                StartCoroutine(Shoot());
                activeShooter = false;
            }
        }
    }

    public IEnumerator Shoot()
    {
        readyToShoot = false;

        PlayFireSFX();
        if (animator)
        {
            PlayFireAnimation();
        }

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
            if (burstDelay > 0)
            {
                WaitForSeconds wait = new WaitForSeconds(burstDelay);
                yield return wait;
            }
        }

        Invoke("ResetShot", shootingDelay);
    }

    private void PlayFireSFX()
    {
        if (!flamethrower)
        {
            muzzleFlare.Play(true);
            AudioHelper.PlayRandomPitch(firingAudio);
        }
        else
        {
            if (!muzzleFlare.isPlaying)
            {
                muzzleFlare.Play(true);
            }
            if (!firingAudio.isPlaying)
            {
                AudioHelper.PlayRandomPitch(firingAudio);
            }
        }
    }

    private void PlayFireAnimation()
    {
        if(animator.gameObject.activeSelf)
        {
            animator.Play("Shoot");
        }
    }

    public void PlayIdleAnimation()
    {
        if (animator.gameObject.activeSelf)
        {
            animator.Play("Idle");
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        // new
        cooldownTimer = 0f;
    }

    public void SetShooting(bool state)
    {
        if (flamethrower && state == false)
        {
            muzzleFlare.Stop(true);
            firingAudio.Stop();
            PlayIdleAnimation();
        }

        activeShooter = state;
    }

    // returns 0 to 1 for cooldown progress
    public float GetCooldownProgress()
    {
        return readyToShoot ? 1f : Mathf.Clamp01(cooldownTimer / shootingDelay);
    }
}