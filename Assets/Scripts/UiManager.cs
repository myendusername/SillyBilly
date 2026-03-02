using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    // We drag and drop the canvas's text objects into these fields
    // to connect them with this script
    [SerializeField] private GameObject health, stamina;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        trackPlayer();
    }

    public void trackPlayer() {
        // these set the health and stamina stats in the UI
        // to the player's current health and stamina stats.
        // Casting the numbers as ints so that we don't see
        // a bunch of unneeded decimal places for our stats
        health.GetComponentInChildren<TextMeshProUGUI>().text = "Health: " + (int)Character.Instance.Health();
        stamina.GetComponentInChildren<TextMeshProUGUI>().text = "Stamina: " + (int)Character.Instance.Stamina();
    }
}
