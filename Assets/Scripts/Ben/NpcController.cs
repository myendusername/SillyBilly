using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NpcController : MonoBehaviour
{
    public NpcMovement movement;
    public NavMeshAgent agent;
    public NpcAttack attack;
    public NpcData data;

    public Health health;

    public float spawnTime = 0.5f;
    public float updateRate = 0.2f;
    protected float stunTime = 0.5f;

    protected Transform closestTarget;
    protected string layerToTarget;

    public enum State
    {
        SPAWNING,
        CHASE,
        STUNNED,
        MELEEATTACK,
        PROJECTILEATTACK,
    }

    protected State currentState;

    public void Start()
    {
        SetupAgentFromConfig();
        ChangeState(State.SPAWNING);
        StartCoroutine(StateMachine());
    }

    public void SetupAgentFromConfig()
    {
        health.health = data.health;


        attack.meleeAttack = data.meleeAttack;
        attack.meleeRange = data.meleeRange;
        attack.meleeRadiusSize = data.meleeRadiusSize;
        attack.meleeDelay = data.meleeDelay;
        attack.meleeDamage = data.meleeDamage;


        attack.projectileAttack = data.projectileAttack;
        attack.projectileMinRange = data.projectileMinRange;
        attack.projectileMaxRange = data.projectileMaxRange;
        attack.projectileLifetime = data.projectileLifetime;
        attack.projectileDelay = data.projectileDelay;
        attack.projectileDamage = data.projectileDamage;
        attack.projectileSpeed = data.projectileSpeed;
        attack.horizontalSpread = data.horizontalSpread;
        attack.verticalSpread = data.verticalSpread;
        attack.projectileAmount = data.projectileAmount;
        attack.constantFire = data.constantFire;
        attack.attackCooldown = data.attackCooldown;
        attack.projectilePrefab = data.projectilePrefab;
        attack.tooCloseRange = data.tooCloseRange;


        movement.cardinalMoveChance = data.cardinalMoveChance;
        movement.diagonalMoveChance = data.diagonalMoveChance;
        movement.tooCloseRange = data.tooCloseRange;


        updateRate = data.updateRate;

        agent.speed = data.speed;
        agent.angularSpeed = data.angularSpeed;
        agent.acceleration = data.acceleration;
        agent.stoppingDistance = data.stoppingDistance;

        agent.baseOffset = data.baseOffset;
        agent.radius = data.radius;
        agent.height = data.height;
        agent.avoidancePriority = data.avoidancePriority;

        agent.areaMask = data.areaMask;

        layerToTarget = data.layerToTarget;
    }

    protected void LookAtY(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        if (direction == Vector3.zero)
        {
            return;
        }

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        float targetY = lookRotation.eulerAngles.y;
        float currentY = transform.eulerAngles.y;

        float delta = Mathf.DeltaAngle(currentY, targetY);

        float clampedDelta = Mathf.Clamp(delta, -30f, 30f);

        transform.rotation = Quaternion.Euler(0f, currentY + clampedDelta, 0f);
    }

    protected void LookAtX(Transform target)
    {
        Vector3 direction = target.position - attack.attackPoint.position;

        if (direction == Vector3.zero)
        {
            return;
        }

        float distanceXZ = new Vector2(direction.x, direction.z).magnitude;
        float angleX = Mathf.Atan2(direction.y, distanceXZ) * Mathf.Rad2Deg;

        attack.attackPoint.localRotation = Quaternion.Euler(-angleX, 0f, 0f);
    }

    public Transform GetClosestTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, 200, LayerMask.GetMask(layerToTarget));

        Transform closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider target in targets)
        {
            float distance = Vector3.Distance(target.transform.position, transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = target.transform;
            }
        }

        return closest;
    }

    protected IEnumerator StateMachine()
    {
        WaitForSeconds wait = new WaitForSeconds(updateRate);
        while (enabled)
        {
            closestTarget = GetClosestTarget();
            switch (currentState)
            {
                case State.SPAWNING:
                    break;

                case State.CHASE:
                    UpdateChase();
                    break;

                case State.STUNNED:
                    break;

                case State.MELEEATTACK:
                    break;

                case State.PROJECTILEATTACK:
                    break;
            }

            yield return wait;
        }
    }

    public void ChangeState(State newState)
    {
        OnStateExit(currentState);

        currentState = newState;

        OnStateEnter(newState);
    }

    protected void OnStateEnter(State state)
    {
        switch (state)
        {
            case State.SPAWNING:
                StartCoroutine(EnterSpawning());
                break;

            case State.CHASE:
                EnterChase();
                break;

            case State.STUNNED:
                StartCoroutine(EnterStunned());
                break;

            case State.MELEEATTACK:
                StartCoroutine(EnterMelee());
                break;

            case State.PROJECTILEATTACK:
                StartCoroutine(EnterProjectile());
                break;
        }
    }

    protected void OnStateExit(State state)
    {
        switch (state)
        {
            case State.MELEEATTACK:

                break;
        }
    }

    protected IEnumerator EnterSpawning()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnTime);
        yield return wait;

        ChangeState(State.CHASE);
    }

    protected void EnterChase()
    {
    }

    protected void UpdateChase()
    {
        if (closestTarget)
        {
            movement.FollowTarget(closestTarget);

            if (attack.meleeAttack)
            {
                if (attack.CanMelee(closestTarget, false))
                {
                    ChangeState(State.MELEEATTACK);
                }
            }
            if (attack.projectileAttack)
            {
                if (attack.CanProjectile(closestTarget, layerToTarget, true, false))
                {
                    ChangeState(State.PROJECTILEATTACK);
                }
            }
        }
    }

    protected IEnumerator EnterStunned()
    {
        WaitForSeconds wait = new WaitForSeconds(stunTime);
        yield return wait;

        ChangeState(State.CHASE);
    }

    protected virtual IEnumerator EnterMelee()
    {
        agent.ResetPath();

        yield return StartCoroutine(WaitAndLook(attack.meleeDelay / 3));
        yield return StartCoroutine(WaitAndLook(attack.meleeDelay / 3));
        yield return StartCoroutine(WaitAndLook(attack.meleeDelay / 3));

        LookAtY(closestTarget);

        attack.DoMeleeAttack(layerToTarget);

        ChangeState(State.CHASE);
    }

    protected virtual IEnumerator EnterProjectile()
    {
        agent.ResetPath();

        yield return StartCoroutine(WaitAndLook(attack.projectileDelay / 3));
        yield return StartCoroutine(WaitAndLook(attack.projectileDelay / 3));
        yield return StartCoroutine(WaitAndLook(attack.projectileDelay / 3));

        LookAtY(closestTarget);
        LookAtX(closestTarget);

        attack.DoProjectileAttack(layerToTarget);


        if (attack.constantFire)
        {
            StartCoroutine(ProjectileConstantFire());
            attack.isFiring = true;
        }
        else
        {
            ChangeState(State.CHASE);
        }
    }

    protected IEnumerator ProjectileConstantFire()
    {
        while (attack.CanProjectile(closestTarget, layerToTarget, false, true))
        {
            WaitForSeconds wait = new WaitForSeconds(attack.attackCooldown);
            yield return wait;

            LookAtY(closestTarget);
            LookAtX(closestTarget);
            attack.DoProjectileAttack(layerToTarget);
        }
        ChangeState(State.CHASE);
        attack.isFiring = false;
    }

    protected IEnumerator WaitAndLook(float delay)
    {
        LookAtY(closestTarget);
        WaitForSeconds wait = new WaitForSeconds(delay);
        yield return wait;
    }
}
