using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVolume : MonoBehaviour
{
    public SphereCollider sphereCollider;

    public bool oneShot = false;
    public float lifetime = 0.1f;
    public int damage = 1;
    public LayerMask layersToDetect;

    // The things that have health that are in the attack radius
    private List<Health> targetList = new List<Health>();

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        StartCoroutine(RemoveAfterLifetime());
    }

    public void Setup(bool oneShot, float lifetime, int damage, LayerMask layersToDetect, float radius)
    {
        this.oneShot = oneShot;
        this.lifetime = lifetime;
        this.damage = damage;
        this.layersToDetect = layersToDetect;
        sphereCollider.radius = radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        // If gameObject is not in a valid layer to detect then stop.
        if (layersToDetect != (layersToDetect | (1 << other.gameObject.layer)))
        {
            return;
        }

        Health target = other.GetComponent<Health>();

        // If the collider has a damageable, then add it to the list of things that will be damaged.
        if (target != null)
        {
            if (oneShot)
            {
                target.TakeDamage(damage);
                Destroy(gameObject);
            }
            else
            {
                if (!targetList.Contains(target))
                {
                    targetList.Add(target);
                }
            }
        }
        else
        {
            Debug.LogWarning("TARGET IS NULL!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If gameObject is not in a valid layer to detect then stop.
        if (layersToDetect != (layersToDetect | (1 << other.gameObject.layer)))
        {
            return;
        }

        Health target = other.GetComponent<Health>();

        // If the collider has a damageable, then remove it from the list of things that will be damaged.
        if (target != null)
        {
            targetList.Remove(target);
        }
    }

    private IEnumerator RemoveAfterLifetime()
    {
        WaitForSeconds wait = new WaitForSeconds(lifetime);
        yield return wait;

        if (!oneShot)
        {
            // Remove all null entries.
            targetList.RemoveAll(t => t == null);

            foreach (Health target in targetList)
            {
                Debug.Log("Dealt damage to " + target);
                target.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (sphereCollider != null)
        {
            Gizmos.color = new Color(0.9f, 0.3f, 0.25f, 0.2f);
            Gizmos.DrawSphere(transform.position, sphereCollider.radius);
        }
    }

}
