using UnityEngine;

public class Stunner : MonoBehaviour
{
    public float stunTime;


    public void ApplyStun(GameObject target)
    {
        NpcController npcController = target.GetComponent<NpcController>();

        npcController.SetStun(stunTime);
    }


}
