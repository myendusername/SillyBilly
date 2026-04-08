using UnityEngine;
using static UnityEngine.UI.Image;

public class EnemyAttack : MonoBehaviour
{
    public Transform attackPoint;

    [Header("Melee Attack")]
    public bool meleeAttack = false;
    public float meleeRange = 1.4f;
    public float meleeRadiusSize = 0.5f;
    public float meleeDelay = 0.3f;
    public int meleeDamage = 4;
    public GameObject damageVolumePrefab;

    [Header("Projectile Attack")]
    public bool projectileAttack = false;
    // SCRAPPED
    // public float projectileSightDegrees = 60f;
    public float projectileMaxRange = 8f;
    public float projectileMinRange = 0.5f;
    public float projectileLifetime = 3f;
    public float projectileDelay = 0.3f;
    public int projectileDamage = 2;
    public float projectileSpeed = 20f;
    public float horizontalSpread = 0f;
    public float verticalSpread = 0f;
    public int projectileAmount = 1;
    public bool constantFire = false;
    public float constantFireDelay = 0.1f;
    public GameObject projectilePrefab;

    public bool isFiring = false;


    public bool canMelee(Transform target)
    {
        if (target == null)
        {
            return false;
        }

        Vector3 directionAttackPoint = target.position - attackPoint.position;
        float distanceAttackPoint = directionAttackPoint.magnitude;

        if (distanceAttackPoint > meleeRange)
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

        Vector3 direction = target.position - transform.position;
        float distance = direction.magnitude;

        // Check line of sight
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction.normalized, out hit, meleeRange))
        {
            if (hit.transform != target)
            {
                return false;
            }
        }

        return true;
    }

    public bool canProjectile(Transform target, bool checkChance)
    {
        if (target == null)
        {
            Debug.Log("Target Null.");
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
        if (Physics.Raycast(attackPoint.position, direction.normalized, out hit, projectileMaxRange))
        {
            if (hit.transform != target)
            {
                Debug.DrawLine(transform.position, hit.point, Color.red, 0.1f);
                return false;
            }
            Debug.DrawLine(transform.position, hit.point, Color.green, 0.1f);
        }



        // Attack Chance
        // MIGHT CHANGE THIS TO BE EXPONENTIAL
        if (checkChance)
        {
            float attackChance = Mathf.Lerp(1.4f, 0.2f, (distance - projectileMinRange) / (projectileMaxRange - projectileMinRange));

            if (attackChance < Random.value)
            {
                return false;
            }
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
        for (int i = 0; i < projectileAmount; i++)
        {
            // Spread
            float x = Random.Range(-horizontalSpread, horizontalSpread);
            float y = Random.Range(-verticalSpread, verticalSpread);

            Quaternion spreadRotation = attackPoint.rotation * Quaternion.Euler(y, x, 0);

            GameObject projectile = Instantiate(projectilePrefab, attackPoint.position, spreadRotation);

            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.linearVelocity = projectile.transform.forward * projectileSpeed;

            projectile.GetComponent<Projectile>().Setup(projectileLifetime, projectileDamage, "Player");

        }
        attackPoint.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void OnDrawGizmos()
    {
        if (meleeAttack)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, meleeRange);
        }

        if (projectileAttack)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackPoint.position, projectileMaxRange);
            Gizmos.color = Color.orange;
            Gizmos.DrawWireSphere(transform.position, projectileMinRange);
        }
    }

}
