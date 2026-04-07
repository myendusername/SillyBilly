using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVolume : MonoBehaviour
{
    public SphereCollider sphereCollider;

    public bool oneShot = true;
    private bool alreadyEntered = false;
    public float lifetime = 0.1f;
    public int damage = 1;
    public LayerMask layersToDetect = -1;

    // The things that have health that are in the attack radius
    private List<Health> targetList = new List<Health>();

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        alreadyEntered = false;
        StartCoroutine(RemoveAfterLifetime());
    }

    public void Setup(bool oneShot, float lifetime, int damage, LayerMask layersToDetect, float radius)
    {
        this.oneShot = oneShot;
        this.lifetime = lifetime;
        this.damage = damage;
        this.layersToDetect = layersToDetect;
        this.GetComponent<SphereCollider>().radius = radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the damage volume is a one shot and already entered before then stop.
        if (oneShot && alreadyEntered)
        {
            return;
        }

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
                targetList.Add(target);
            }
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

        Destroy(gameObject);
    }


}
