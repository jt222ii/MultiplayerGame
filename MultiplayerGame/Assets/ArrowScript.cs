using UnityEngine;
using System.Collections;

public class ArrowScript : MonoBehaviour {
    public float speed;
    public Vector2 minSpeedUntilHarmless;
    public Collider2D collision, hitBox;

    private bool harmless = false;
    private Rigidbody2D rigidBody;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = transform.right * speed; 
	}

    // Update is called once per frame
    void Update() {
        Vector2 moveDirection = rigidBody.velocity;
        if (moveDirection != Vector2.zero && !harmless)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            rigidBody.velocity = Vector2.zero;
            rigidBody.gravityScale = 0;
            Destroy(collision);
            Destroy(hitBox);
            harmless = true;
        }
    }
}
