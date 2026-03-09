using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject impactPrefab;
    public float impactLifetime;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            print("Hit " + collision.gameObject.name);
        }
        GameObject impact = Instantiate(impactPrefab, this.transform.position, this.transform.rotation);
        Destroy(impact, impactLifetime);

        Destroy(gameObject);
    }
}
