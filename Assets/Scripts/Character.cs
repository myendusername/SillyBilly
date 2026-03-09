using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static Character Instance;
    [SerializeField] private float health = 50.0f;
    [SerializeField] private float stamina = 50.0f;
    [SerializeField] private float speed = 2.0f;


    // Update is called once per frame
    void Update()
    {
        Move();
        Regen();
    }

    // sort of like a Unity-specific constructor
    private void Awake()
    {
        Instance = this;
    }

    public void Move() {
        // player can't go as fast backwards as they can forwards
        float forward = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float backward = Input.GetAxis("Vertical") * 0.5f * -speed * Time.deltaTime;
        float turnRight = Input.GetAxis("Horizontal") * 100f * Time.deltaTime;
        float turnLeft = Input.GetAxis("Horizontal") * -100f * Time.deltaTime;

        // sprint mechanics --> make the forward/backward vectors a higher value
        // When sprinting, 10 stamina is lost per second (because of Time.deltaTime
        // --> without time.deltaTime, 10 stamina would be lost per frame instead).
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && stamina > 0)
        {
            forward = forward * 3f;
            backward = backward * 1.5f;
            stamina -= 10 * Time.deltaTime;
        }


        // making our character move
        // (should probably be changed to a switch statement later)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            transform.Translate(forward, 0, 0);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(-backward, 0, 0);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, turnRight, 0);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, -turnLeft, 0);
        }
    }

    public void Regen() {
        // Stamina regens @ 2 per second when not sprinting,
        // and not @ max stamina
        if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && stamina < 50)
        {
            stamina += 2 * Time.deltaTime;
        }

        // Health regen could go here later
    }

    // Getter for character's health stat
    public float Health() {
        return health; 
    }

    // Getter for character's stamina stat
    public float Stamina() { 
        return stamina;
    }
}

