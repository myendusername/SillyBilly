using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    private PlayerMovement mover;
    public GameObject[] characters;

    public GameObject activeCharacter;
    private PlayerLook look;
    private PlayerShoot shooter;
    private PlayerSecondaryShoot secondaryShooter;

    private bool isMouseSwitching = false;
    private MouseSwitching currentTarget;

    void Awake()
    {
        Instance = this;

        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        // SetActiveCharacter(characters[0]);

        // modifying the for loop to suit cases where players 
        // do not select characters[0] at first.
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] != UiManager.Instance.player)
            {
                PlayerNpcController npcController = characters[i].GetComponent<PlayerNpcController>();
                npcController.enabled = true;
                npcController.SetNpcMode(true);
            }
            //PlayerNpcController npcController = characters[i].GetComponent<PlayerNpcController>();
            //npcController.enabled = true;
            //npcController.SetNpcMode(true);
        }

        // Callback Context
        onFoot.Jump.performed += ctx => mover.Jump();
        onFoot.Sprint.started += ctx => mover.Sprint(true);
        onFoot.Sprint.canceled += ctx => mover.Sprint(false);

        onFoot.Shoot.started += ctx => shooter?.SetShooting(true);
        onFoot.Shoot.canceled += ctx => shooter?.SetShooting(false);

        onFoot.SecondaryShoot.started += ctx => secondaryShooter?.SetShooting(true);
        onFoot.SecondaryShoot.canceled += ctx => secondaryShooter?.SetShooting(false);

        onFoot.MouseSwitch.started += ctx => StartMouseSwitch();
        onFoot.MouseSwitch.canceled += ctx => EndMouseSwitch();

        // Character switch callback
        onFoot.SwitchCharacter.performed += ctx =>
                {
                    switch (ctx.control.name)
                    {
                        case "1":
                            SetActiveCharacter(characters[0]);
                            UiManager.Instance.SetPlayer(characters[0]);
                            break;
                        case "2":
                            SetActiveCharacter(characters[1]);
                            UiManager.Instance.SetPlayer(characters[1]);
                            break;
                        case "3":
                            SetActiveCharacter(characters[2]);
                            UiManager.Instance.SetPlayer(characters[2]);
                            break;
                    }
                };
    }

    public void SetActiveCharacter(GameObject character)
    {
        if (shooter)
        {
            shooter.weaponArt.SetActive(false);
            shooter.SetShooting(false);
        }

        if (secondaryShooter)
        {
            secondaryShooter.SetShooting(false);
        }

        if (mover)
        {
            mover.Sprint(false);
        }
        if (activeCharacter)
        {
            PlayerNpcController oldNpcController = activeCharacter.GetComponent<PlayerNpcController>();
            oldNpcController.enabled = true;
            oldNpcController.SetNpcMode(true);
        }

        activeCharacter = character;
        PlayerNpcController newNpcController = activeCharacter.GetComponent<PlayerNpcController>();
        newNpcController.SetNpcMode(false);
        newNpcController.enabled = false;

        mover = character.GetComponent<PlayerMovement>();
        look = character.GetComponent<PlayerLook>();
        shooter = character.GetComponent<PlayerShoot>();
        secondaryShooter = character.GetComponent<PlayerSecondaryShoot>();
        shooter.weaponArt.SetActive(true);
        CameraManager cam = Camera.main.GetComponent<CameraManager>();
        if (cam != null)
        {
            cam.cameraPivot = character.GetComponent<PlayerLook>().cameraPivot;
            cam.SetDefaultFovInstant();
        }
    }

    void StartMouseSwitch()
    {
        isMouseSwitching = true;
        foreach (GameObject obj in characters)
        {
            MouseSwitching playerOverlay = obj.GetComponent<MouseSwitching>();
            if (playerOverlay != null && obj != activeCharacter)
            {
                playerOverlay.SetTint(Color.red);
            }
        }
    }
    void EndMouseSwitch()
    {
        isMouseSwitching = false;

        foreach (GameObject obj in characters)
        {
            MouseSwitching playerOverlay = obj.GetComponent<MouseSwitching>();
            if (playerOverlay != null)
            {
                playerOverlay.ResetTint();
            }
        }

        if (currentTarget != null)
        {
            SetActiveCharacter(currentTarget.gameObject);
            UiManager.Instance.SetPlayer(currentTarget.gameObject);
        }

        currentTarget = null;
    }

    MouseSwitching GetTarget()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        int mask = LayerMask.GetMask("Player");

        if (Physics.SphereCast(ray, 1f, out RaycastHit hit, 100f, mask))
        {
            return hit.collider.GetComponentInParent<MouseSwitching>();
        }

        return null;
    }
    void Update()
    {
        if (!isMouseSwitching) return;

        foreach (GameObject obj in characters)
        {
            if (obj == activeCharacter) continue;

            MouseSwitching playerOverlay = obj.GetComponent<MouseSwitching>();
            if (playerOverlay != null)
            {
                playerOverlay.SetTint(new Color(10000f, 0f, 0f));
            }
        }

        currentTarget = GetTarget();

        if (currentTarget != null && currentTarget.gameObject != activeCharacter)
        {
            currentTarget.SetTint(new Color(0f, 10000f, 0f));
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

    public PlayerMovement GetMover()
    {
        return mover;
    }
}