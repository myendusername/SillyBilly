using System.Collections;
using UnityEngine;

public class OilSpill : MonoBehaviour
{
    public float playerSpeedMulti = 2.6f;
    public float playerSpeedOnFireMulti = 3.2f;
    public float playerSmoothingMulti = 0.5f;
    public float delayUntilNormal = 0.2f;
    private bool onFire = false;
    public GameObject oilFireArea;
    public float stunTime = 4f;
    public GameObject stunPrefab;

    private Coroutine resetCoroutine;

    private bool isPlayerInside = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stunner"))
        {
            GameObject stunObject = Instantiate(stunPrefab, transform.position, transform.rotation);
            stunObject.GetComponent<StunVolume>().Setup(stunTime);
            Destroy(other);
            Destroy(gameObject);
        }
    }

    // THIS LOGIC IS JANK, NEEDS WORK
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;

            if (resetCoroutine != null)
            {
                StopCoroutine(resetCoroutine);
                resetCoroutine = null;
            }

            PlayerMovement playerMove = other.GetComponent<PlayerMovement>();
            playerMove.SetMovementMulti(playerSpeedMulti);
            playerMove.SetSmoothingMulti(playerSmoothingMulti);
        }
        else if (other.CompareTag("Fire") && onFire == false)
        {
            onFire = true;
            Instantiate(oilFireArea, transform.position, transform.rotation, transform);
            playerSpeedMulti = playerSpeedOnFireMulti;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            resetCoroutine = StartCoroutine(DelayUntilNormal(other));
        }
    }

    private IEnumerator DelayUntilNormal(Collider other)
    {
        yield return new WaitForSeconds(delayUntilNormal);

        if (!isPlayerInside && other != null)
        {
            PlayerMovement playerMove = other.GetComponent<PlayerMovement>();
            playerMove.SetMovementMulti(1f);
            playerMove.SetSmoothingMulti(1f);
        }
    }
}
