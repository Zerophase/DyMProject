using UnityEngine;
using System.Collections;

public class Player2 : MonoBehaviour {

	// Basic Player Variables
    public float speed = 50f;
    private Vector3 movement;
    private bool onGround;
    public Vector3 jumpForce;
    public bool doubleJump;

    void Update() {
        float joystickDirectionX = Input.GetAxis("Horizontal");
        movement.x = joystickDirectionX * speed;
    }
	void FixedUpdate () {
        rigidbody.velocity = movement;
        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }
	}

    void Jump() {
        if(onGround)
        {
            Debug.Log("Button? " + Input.GetButtonDown("Jump"));
            Debug.Log("Grounded? " + onGround);
            rigidbody.AddForce(jumpForce, ForceMode.VelocityChange);
            onGround = false;
        }
    }

    void OnCollisionEnter()
    {
        onGround = true;
    }

    void OnCollisionExit()
    {
        onGround = false;
    }
}
