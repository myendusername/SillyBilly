using UnityEngine;

public class Character : MonoBehaviour
{
    public static Character Instance;

    [SerializeField] private float speed = 2.0f;

    private void Awake()
    {
        Instance = this;
    }

    public float Speed()
    {
        return speed;
    }
}
