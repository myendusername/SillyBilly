using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunVolume : MonoBehaviour
{
    private SphereCollider sphereCollider;

    public float lifetime = 0.1f;
    public float stunTime = 2;
    public LayerMask layersToDetect;

    private List<NpcController> targetList = new List<NpcController>();

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        StartCoroutine(RemoveAfterLifetime());
    }

    public void Setup(float stunTime)
    {
        this.stunTime = stunTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // If gameObject is not in a valid layer to detect then stop.
        if (layersToDetect != (layersToDetect | (1 << other.gameObject.layer)))
        {
            return;
        }

        NpcController target = other.GetComponent<NpcController>();

        // If the collider has a NpcController, then add it to the list of things that will be stunned.
        if (target != null)
        {
            if (!targetList.Contains(target))
            {
                targetList.Add(target);
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

        NpcController target = other.GetComponent<NpcController>();

        // If the collider has a NpcController, then remove it from the list of things that will be stunned.
        if (target != null)
        {
            targetList.Remove(target);
        }
    }

    private IEnumerator RemoveAfterLifetime()
    {
        WaitForSeconds wait = new WaitForSeconds(lifetime);
        yield return wait;

        StunAllInArea();

        Destroy(gameObject);
    }

    private void StunAllInArea()
    {
        // Remove all null entries.
        targetList.RemoveAll(t => t == null);

        foreach (NpcController target in targetList)
        {
            target.SetStun(stunTime);
        }
    }

    private void OnDrawGizmos()
    {
        if (sphereCollider != null)
        {
            Gizmos.color = new Color(0.3f, 0.3f, 0.8f, 0.2f);
            Gizmos.DrawSphere(transform.TransformPoint(sphereCollider.center), sphereCollider.radius);
        }
    }

}
