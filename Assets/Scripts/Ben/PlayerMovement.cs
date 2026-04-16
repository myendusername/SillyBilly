using UnityEngine;

public class PlayerMovement : MonoBehaviour, IDamageable
{
    private CharacterController charController;
    private Vector3 playerVelocity;
    private float speed;
    [Header("Horizontal Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 1f;
    public float stamina = 50.0f;
    public float maxStamina = 50f;
    public float currentStamina = 50f;
    public float movementSmoothing = 20f;

    [Header("Jumping")]
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    private bool isGrounded;


    void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    void Start()
    {
        speed = walkSpeed;
    }

    void Update()
    {
        isGrounded = charController.isGrounded;
    }

    // Receive the inputs for our InputManager and apply them to our chracter controller.
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = new Vector3(input.x, 0, input.y).normalized;

        Vector3 targetVelocity = transform.TransformDirection(moveDirection) * speed;
        Vector3 currentVelocity = new Vector3(charController.velocity.x, 0, charController.velocity.z);

        Vector3 horizontalVelocity = Vector3.Lerp(currentVelocity, targetVelocity, movementSmoothing * Time.deltaTime);

        playerVelocity.x = horizontalVelocity.x;
        playerVelocity.z = horizontalVelocity.z;

        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        charController.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }
    }

    public void Sprint(bool sprintStatus)
    {
        if (sprintStatus)
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = walkSpeed;
        }
    }

    public void OnHurt()
    {
        // Unused for now
    }

    public void OnDead()
    {
        // Usused for now
    }
}