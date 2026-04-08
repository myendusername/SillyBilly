using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform cameraPivot; // assign the head or camera pivot in prefab

    void LateUpdate()
    {
        if (cameraPivot == null) return;

        // Position matches camera pivot
        transform.position = cameraPivot.position;

        // Rotation matches cameraPivot rotation (first-person)
        transform.rotation = cameraPivot.rotation;
    }
}