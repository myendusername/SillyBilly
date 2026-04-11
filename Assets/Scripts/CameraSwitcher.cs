using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Transform cameraPivot;

    void LateUpdate()
    {
        if (cameraPivot == null) return;

        transform.position = cameraPivot.position;
        transform.rotation = cameraPivot.rotation;
    }
}