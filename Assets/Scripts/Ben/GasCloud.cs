using System.Collections;
using UnityEngine;

public class GasCloud : MonoBehaviour
{
    public GameObject explosionPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sparker"))
        {
            StartCoroutine(Explode());
        }
    }

    private IEnumerator Explode()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        yield return wait;

        Instantiate(explosionPrefab, this.transform.position, this.transform.rotation);
        Destroy(gameObject);
    }
}
