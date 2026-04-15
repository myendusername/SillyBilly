using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject impactPrefab;
    public float projectileLifetime = 3f;
    public int damage = 2;
    public string tagToDetect = null;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(tagToDetect))
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
            // print("Hit " + collision.gameObject.name);
        }
        GameObject impact = Instantiate(impactPrefab, this.transform.position, this.transform.rotation);

        Destroy(gameObject);
    }

    private IEnumerator RemoveAfterLifetime(float lifetime)
    {
        WaitForSeconds wait = new WaitForSeconds(lifetime);
        yield return wait;

        Destroy(gameObject);
    }

    public void Setup(float lifetime, int damage, string tag)
    {
        this.projectileLifetime = lifetime;
        this.damage = damage;
        this.tagToDetect = tag;
        StartCoroutine(RemoveAfterLifetime(projectileLifetime));
    }
}
