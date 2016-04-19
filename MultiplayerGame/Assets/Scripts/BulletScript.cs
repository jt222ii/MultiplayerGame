using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
	public float speed;
	public int bouncesBeforeDeletion;
	public Collider2D collision;

	private int bounces = 0;
	private bool harmless = false;
	private Rigidbody2D rigidBody;
	private Renderer myRenderer;

	// Use this for initialization
	void Start () {
		myRenderer = gameObject.GetComponent<Renderer> ();
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
			//harmless = true;
		}
	}
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Wall")
		{
			if (bounces >= bouncesBeforeDeletion) {
				Destroy (gameObject);
			}

			rigidBody.velocity = Vector2.Reflect(rigidBody.velocity, other.contacts[0].normal);
			bounces++;
		}
	}
}
