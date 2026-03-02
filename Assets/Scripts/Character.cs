using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.Move();
    }

    public void Move() {
        // player can't go as fast backwards as they can forwards
        float forward = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float backward = Input.GetAxis("Vertical") * 0.5f * -speed * Time.deltaTime;
        float turnRight = Input.GetAxis("Horizontal") * 100f * Time.deltaTime;
        float turnLeft = Input.GetAxis("Horizontal") * -100f * Time.deltaTime;

        // sprint mechanics --> make the forward/backward vectors a higher value
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            forward = forward * 3f;
            backward = backward * 1.5f;
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
}
