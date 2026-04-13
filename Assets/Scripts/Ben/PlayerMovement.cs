using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private float speed;
    [Header("Horizontal Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 1f;
    public float stamina = 50.0f;
    public float movementSmoothing = 20f;

    [Header("Jumping")]
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    private bool isGrounded;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = walkSpeed;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
    }

    // Receive the inputs for our InputManager and apply them to our chracter controller.
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = new Vector3(input.x, 0, input.y).normalized;

        Vector3 targetVelocity = transform.TransformDirection(moveDirection) * speed;
        Vector3 currentVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);

        Vector3 horizontalVelocity = Vector3.Lerp(currentVelocity, targetVelocity, movementSmoothing * Time.deltaTime);

        playerVelocity.x = horizontalVelocity.x;
        playerVelocity.z = horizontalVelocity.z;

        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        controller.Move(playerVelocity * Time.deltaTime);
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
}
