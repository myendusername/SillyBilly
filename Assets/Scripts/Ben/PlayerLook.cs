using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private float xRotation = 0f;
    private float yRotation = 0f;

    public float xSens = 50f;
    public float ySens = 50f;

    public float xClamp = 85f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        // Rotation around the x axis (Look up and down)
        xRotation -= mouseY * xSens * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);

        // Rotation around the Y axis (Look left and right)
        yRotation += mouseX * ySens * Time.deltaTime;

        // Apply rotations to our transform
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

    }
}
