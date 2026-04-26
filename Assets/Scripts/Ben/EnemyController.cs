public class EnemyController : NpcController, IDamageable
{
    public void OnHurt()
    {
        // Unused for now
    }

    public void OnDead()
    {
        Destroy(gameObject);

        // Poolee version:
        // gameObject.SetActive(false);
    }
}
