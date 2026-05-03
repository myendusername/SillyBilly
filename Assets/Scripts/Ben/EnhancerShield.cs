using UnityEngine;

public class EnhancerShield : MonoBehaviour
{
    public GameObject activatedPrefab;
    public GameObject explosionPrefab;
    public float gasBallCloneSpeed = 15;
    private float gasBallCloneTimer = 1f;

    private void OnEnable()
    {
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Stunner") || other.gameObject.CompareTag("Sparker") || other.gameObject.name.StartsWith("Gas Ball"))
        {
            Transform trans = other.transform;
            if (other.gameObject.CompareTag("Stunner"))
            {
                Projectile projectile = other.GetComponent<Projectile>();
                projectile.damage = projectile.damage * 3;
            }
            else if (other.gameObject.CompareTag("Sparker"))
            {
                Projectile projectile = other.GetComponent<Projectile>();
                projectile.impactPrefab = explosionPrefab;
            }
            else if (other.gameObject.name.StartsWith("Gas Ball") && gasBallCloneTimer <= 0)
            {
                Vector3 dir1 = Quaternion.AngleAxis(8, other.transform.up) * other.transform.forward;
                Vector3 dir2 = Quaternion.AngleAxis(-8, other.transform.up) * other.transform.forward;

                GameObject gasBallClone1 = Instantiate(other.gameObject, other.transform.position, Quaternion.LookRotation(dir1));
                GameObject gasBallClone2 = Instantiate(other.gameObject, other.transform.position, Quaternion.LookRotation(dir2));

                Destroy(other.gameObject);

                Rigidbody rb1 = gasBallClone1.GetComponent<Rigidbody>();
                Rigidbody rb2 = gasBallClone2.GetComponent<Rigidbody>();

                rb1.linearVelocity = dir1 * gasBallCloneSpeed;
                rb2.linearVelocity = dir2 * gasBallCloneSpeed;
                gasBallCloneTimer = 1f;

            }
            Instantiate(activatedPrefab, trans.position, trans.rotation);
        }

    }


    private void Update()
    {
        if (gasBallCloneTimer > 0f)
            gasBallCloneTimer -= Time.deltaTime;
    }
}
