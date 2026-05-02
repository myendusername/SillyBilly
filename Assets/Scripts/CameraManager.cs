using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform cameraPivot;
    public float defaultFov = 60f;
    public float sprintFov = 70f;
    private float targetFov;

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        targetFov = defaultFov;
    }

    void LateUpdate()
    {
        if (cameraPivot == null) return;

        transform.position = cameraPivot.position;
        transform.rotation = cameraPivot.rotation;
    }

    void Update()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, 10f * Time.deltaTime);
    }

    public void SetSprintFov()
    {
        targetFov = sprintFov;
    }

    public void SetDefaultFov()
    {
        targetFov = defaultFov;
    }

    public void SetDefaultFovInstant()
    {
        cam.fieldOfView = defaultFov;
    }
}