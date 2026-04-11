using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private float xRotation = 0f;
    private float yRotation = 0f;

    public Transform cameraPivot;
    public float xSens = 50f;
    public float ySens = 50f;

    public float xClamp = 85f;

    public void ProcessLook(Vector2 input)
    {

        float mouseX = input.x;
        float mouseY = input.y;

        // Rotation around the x axis (Look up and down)
        xRotation -= mouseY * xSens * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);

        // Rotation around the Y axis (Look left and right)
        yRotation += mouseX * ySens * Time.deltaTime;

        // Apply Y axis rotation to player only
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

        // Apply X axis rotation to camera pivot only
        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
