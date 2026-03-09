using System.Collections;
using System.Data.SqlTypes;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject muzzleFlarePrefab;

    public float bulletLifetime = 3f;
    public float bulletSpeed = 30f;
    public float shootingDelay = 0.2f;
    [Tooltip("In degrees, used for both left and right separately.")]
    public float horizontalSpread = 0f;
    [Tooltip("In degrees, used for both up and down separately.")]
    public float verticalSpread = 0f;
    public int bulletAmount = 1;
    public bool allowHold = false;

    private bool isShooting;
    private bool readyToShoot;

    private GameObject muzzleFlareObject;
    private ParticleSystem muzzleFlare;


    private void Awake()
    {
        readyToShoot = true;
        muzzleFlareObject = Instantiate(muzzleFlarePrefab, firePoint.position, firePoint.rotation, firePoint);
        muzzleFlare = muzzleFlareObject.GetComponent<ParticleSystem>();
    }

    void Update()
    {
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

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, spreadRotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.linearVelocity = bullet.transform.forward * bulletSpeed;

            StartCoroutine(DestroyBullet(bullet, bulletLifetime));
        }

        muzzleFlare.Play(true);

        Invoke("ResetShot", shootingDelay);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private IEnumerator DestroyBullet(GameObject bullet, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(bullet);
    }
}