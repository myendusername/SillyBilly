using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Transform attackPoint;

    [Header("Melee Attack")]
    public bool meleeAttack = false;
    public float meleeRange = 1.5f;
    public float meleeRadiusSize = 0.5f;
    public float meleeDelay = 0.3f;
    public int meleeDamage = 4;
    public GameObject damageVolumePrefab;

    [Header("Projectile Attack")]
    public bool projectileAttack = false;
    // SCRAPPED
    // public float projectileSightDegrees = 60f;
    public float projectileMaxRange = 8f;
    public float projectileMinRange = 0.2f;
    public float projectileLifetime = 3f;
    public float projectileDelay = 0.3f;
    public int projectileDamage = 2;
    public float projectileSpeed = 20f;
    public GameObject projectilePrefab;


    public bool canMelee(Transform target)
    {
        if (target == null)
        {
            return false;
        }

        Vector3 direction = target.position - transform.position;
        float distance = direction.magnitude;

        if (distance > meleeRange)
        {
            return false;
        }

        /**
         * SCRAPPED
        // Check if target is within a forward cone
        float angle = Vector3.Angle(transform.forward, direction.normalized);
        if (angle > projectileSightDegrees)
        {
            return false;
        }
        */

        // Check line of sight
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction.normalized, out hit, meleeRange))
        {
            if (hit.transform != target)
                return false;
        }

        return true;
    }

    public bool canProjectile(Transform target)
    {
        if (target == null)
        {
            return false;
        }

        Vector3 direction = target.position - transform.position;
        float distance = direction.magnitude;

        // Check min + max range
        if (distance > projectileMaxRange || distance < projectileMinRange)
        {
            return false;
        }

        /**
         * SCRAPPED
        // Check if target is within a forward cone
        float angle = Vector3.Angle(transform.forward, direction.normalized);
        if (angle > projectileSightDegrees)
        {
            return false;
        }
        */

        // Check line of sight
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction.normalized, out hit, projectileMaxRange))
        {
            if (hit.transform != target)
                return false;
        }

        // Attack Chance
        // MIGHT CHANGE THIS TO BE EXPONENTIAL
        float attackChance = Mathf.Lerp(1f, 0.2f, (distance - projectileMinRange) / (projectileMaxRange - projectileMinRange));

        if (attackChance < Random.value)
        {
            return false;
        }

        return true;
    }

    public void doMeleeAttack()
    {
        GameObject damageVolume = Instantiate(damageVolumePrefab, attackPoint.position, attackPoint.rotation);
        damageVolume.GetComponent<DamageVolume>().Setup(true, 0.1f, meleeDamage, LayerMask.GetMask("Player"), meleeRadiusSize);

        attackPoint.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void doProjectileAttack()
    {
        GameObject projectile = Instantiate(projectilePrefab, attackPoint.position, attackPoint.rotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.linearVelocity = projectile.transform.forward * projectileSpeed;

        projectile.GetComponent<Projectile>().Setup(projectileLifetime, projectileDamage, "Player");

        attackPoint.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }



}
