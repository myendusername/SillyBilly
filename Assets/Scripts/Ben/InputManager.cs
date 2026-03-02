using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;

    private PlayerMovement mover;
    private PlayerLook look;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        mover = GetComponent<PlayerMovement>();
        look = GetComponent<PlayerLook>();

        // Callback Context
        onFoot.Jump.performed += ctx => mover.Jump();

        onFoot.Sprint.started += ctx => mover.Sprint(true);
        onFoot.Sprint.canceled += ctx => mover.Sprint(false);
    }

    void FixedUpdate()
    {
        // Tell PlayerMovement to move using the value from our movement action.
        mover.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}
