using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    private PlayerMovement mover;
    public GameObject[] characters;

    public GameObject activeCharacter;
    private PlayerLook look;
    private PlayerShoot shooter;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        SetActiveCharacter(characters[0]);

        // Callback Context
        onFoot.Jump.performed += ctx => mover.Jump();
        onFoot.Sprint.started += ctx => mover.Sprint(true);
        onFoot.Sprint.canceled += ctx => mover.Sprint(false);
        onFoot.Shoot.started += ctx =>
        {
            if (shooter != null)
                shooter.SetShooting(true);
        };
        onFoot.Shoot.canceled += ctx => shooter.SetShooting(false);

        // Character switch callback
        onFoot.SwitchCharacter.performed += ctx =>
        {
            switch (ctx.control.name)
            {
                case "1": SetActiveCharacter(characters[0]); break;
                case "2": SetActiveCharacter(characters[1]); break;
                case "3": SetActiveCharacter(characters[2]); break;
            }
        };
    }

    public void SetActiveCharacter(GameObject character)
    {
        activeCharacter = character;
        mover = character.GetComponent<PlayerMovement>();
        look = character.GetComponent<PlayerLook>();
        shooter = character.GetComponent<PlayerShoot>();
        CameraSwitcher cam = Camera.main.GetComponent<CameraSwitcher>();
        if (cam != null)
        {
            cam.cameraPivot = character.GetComponent<PlayerLook>().cameraPivot;
        }
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
