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
        Debug.Log(other + " entered!");

        // If gameObject is not in a valid layer to detect then stop.
        if (layersToDetect != (layersToDetect | (1 << other.gameObject.layer)))
        {
            Debug.Log("case 2");
            return;
        }

        Health target = other.GetComponent<Health>();

        // If the collider has a damageable, then add it to the list of things that will be damaged.
        if (target != null)
        {
            Debug.Log("case 3");
            if (oneShot)
            {
                target.TakeDamage(damage);
                Destroy(gameObject, 0.01f);
                Debug.Log("case 4");
            }
            else
            {
                if (!targetList.Contains(target))
                {
                    targetList.Add(target);
                    Debug.Log("case 5");
                }
            }
        } else
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
            foreach (Health target in targetList)
            {
                target.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

}
