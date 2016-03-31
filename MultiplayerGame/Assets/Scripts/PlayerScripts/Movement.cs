using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    public float movementSpeed;
    public float jumpForce;

    public bool grounded;

    private Rigidbody2D rigidBody;
    private float velocityY;

    // Use this for initialization
    void Start () {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump"))
        {
            // rigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            Jump();
        }
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float velocityX = movementSpeed * horizontal;
        rigidBody.velocity = new Vector2(velocityX, rigidBody.velocity.y);

    }

    void Jump()
    {

        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
    }
}
