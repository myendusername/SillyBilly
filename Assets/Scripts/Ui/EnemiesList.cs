using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesList", menuName = "ScriptableObject/EnemiesList")]
public class EnemiesList : ScriptableObject
{
    // I'm going to put the enemies in this array,
    // then the GameMamager will spawn the enemies with a loop
    // after you press the button to play the game.
    [SerializeField] public GameObject[] enemies;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
