using System.Collections;
using UnityEngine;

public class PlayerNpcController : NpcController, IDamageable
{
    public void OnHurt()
    {
        // Unused for now
    }

    public void OnHeal()
    {
        // Unused
    }

    public void OnDead()
    {
        Debug.Log(gameObject + " is dead!");
        // Destroy(gameObject);

        // Poolee version:
        // gameObject.SetActive(false);
    }

    protected override IEnumerator EnterMelee()
    {
        if (closestTarget)
        {
            movement.FollowTarget(closestTarget);
        }

        yield return StartCoroutine(WaitAndLook(attack.meleeDelay / 3));
        yield return StartCoroutine(WaitAndLook(attack.meleeDelay / 3));
        yield return StartCoroutine(WaitAndLook(attack.meleeDelay / 3));

        LookAtY(closestTarget);

        attack.DoMeleeAttack(layerToTarget);

        ChangeState(State.CHASE);
    }

    protected override IEnumerator EnterProjectile()
    {
        if (closestTarget)
        {
            movement.FollowTarget(closestTarget);
        }

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

    public void SetNpcMode(bool value)
    {
        movement.enabled = value;
        agent.enabled = value;
        attack.enabled = value;
        // Done so npcs don't die when you aren't playing as them.
        health.SetInvincible(value);
    }
}
