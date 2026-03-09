using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public float bulletLifetime = 3f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // left click
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = firePoint.forward.normalized * bulletSpeed;

        Destroy(bullet, bulletLifetime);
    }
}