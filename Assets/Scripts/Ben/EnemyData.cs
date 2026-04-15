using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObject/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Enemy Stats")]
    public int health = 20;

    [Header("Melee Attack")]
    [Tooltip("Whether the enemy can do a melee attack.")]
    public bool meleeAttack = false;
    [Tooltip("The range in which the enemy can do a melee attack.")]
    public float meleeRange = 1.4f;
    [Tooltip("The radius size of the damage volume that the melee attack makes.")]
    public float meleeRadiusSize = 0.5f;
    [Tooltip("Delay before melee attack.")]
    public float meleeDelay = 0.3f;
    public int meleeDamage = 4;

    [Header("Projectile Attack")]
    [Tooltip("Whether the enemy can do a projectile attack.")]
    public bool projectileAttack = false;
    [Tooltip("The max range in which the enemy HAS A CHANCE to do a projectile attack.")]
    public float projectileMaxRange = 8f;
    [Tooltip("The minimum range in which the enemy is able to do a projectile attack; any closer and the enemy will not be able to do a projectile attack.")]
    public float projectileMinRange = 0.5f;
    [Tooltip("Time before projectile automatically gets destroyed.")]
    public float projectileLifetime = 3f;
    [Tooltip("Delay before projectile attack.")]
    public float projectileDelay = 0.3f;
    public int projectileDamage = 2;
    public float projectileSpeed = 20f;
    [Tooltip("In degrees, used for both left and right separately.")]
    public float horizontalSpread = 0f;
    [Tooltip("In degrees, used for both up and down separately.")]
    public float verticalSpread = 0f;
    public int projectileAmount = 1;
    [Tooltip("Whether the enemy will continue to shoot until it breaks line of sight with player or the player exits their range.")]
    public bool constantFire = false;
    [Tooltip("The delay between each constant fire shot.")]
    public float constantFireDelay = 0.1f;
    public GameObject projectilePrefab;

    [Header("Movement")]
    [Header("NOTE: NormalMoveChance is calculated from the \nremaining percent of adding these two chances.")]
    [Space(10)]
    [Range(0f, 1f)]
    public float cardinalMoveChance = 0.33f;
    [Range(0f, 1f)]
    public float diagonalMoveChance = 0.33f;

    [Header("NavMeshAgent Configs")]
    public float updateRate = 0.2f;

    public float speed = 3.5f;
    public float angularSpeed = 180f;
    public float acceleration = 20f;
    public float stoppingDistance = 1.1f;

    public float baseOffset = 1f;
    public float radius = 0.5f;
    public float height = 2f;
    public int avoidancePriority = 50;

    // -1 means everything
    public int areaMask = -1;
}
