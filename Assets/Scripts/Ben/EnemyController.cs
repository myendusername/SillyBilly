using UnityEngine;

public class EnemyController : NpcController, IDamageable
{
    public void OnHurt()
    {
        // Unused for now
    }

    public void OnDead()
    {
        // Adding something to this that subtracts enemies
        // for the GameManager, to help keep track of how many
        // enemies there are in the game.
        Destroy(gameObject);
        //GameManager.Instance.currentEnemiesNumber--;
        //Debug.Log("You killed an enemy! There are currently " + GameManager.Instance.currentEnemiesNumber + " enemies in the game.");

        // Poolee version:
        // gameObject.SetActive(false);
    }
}
