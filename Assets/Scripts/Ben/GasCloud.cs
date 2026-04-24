using System.Collections;
using UnityEngine;

public class GasCloud : MonoBehaviour
{
    public GameObject explosionPrefab;
    public GameObject flamePrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sparker"))
        {
            StartCoroutine(Explode());
        }
        else if (other.gameObject.CompareTag("Fire"))
        {
            StartCoroutine(Flame());
        }
    }

    private IEnumerator Explode()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        yield return wait;

        Instantiate(explosionPrefab, this.transform.position, this.transform.rotation);
        Destroy(gameObject);
    }

    private IEnumerator Flame()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        yield return wait;

        Instantiate(flamePrefab, this.transform.position, this.transform.rotation);
        Destroy(gameObject);
    }
}
