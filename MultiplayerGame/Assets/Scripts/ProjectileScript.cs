using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ProjectileScript : NetworkBehaviour {

	public float speed;
	public bool stickToWalls, bounce;
	public int bouncesBeforeDeletion;
	public Collider2D collision;

	private int bounces = 0;
	private bool harmless = false;
	private Rigidbody2D rigidBody;

	[SyncVar]
	public NetworkInstanceId spawnedBy;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D>();
		rigidBody.velocity = transform.right * speed; 
	}
	public override void OnStartClient(){
		GameObject obj = ClientScene.FindLocalObject(spawnedBy);
		Physics2D.IgnoreCollision(GetComponent<Collider2D>(), obj.GetComponent<Collider2D>());
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
		if (other.tag == "Wall" && stickToWalls)
		{
			rigidBody.velocity = Vector2.zero;
			rigidBody.gravityScale = 0;
			Destroy(collision);
			harmless = true;
		}
	}
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Wall" && bounce)
		{
			if (bounces >= bouncesBeforeDeletion) {
				Destroy (gameObject);
			}

			rigidBody.velocity = Vector2.Reflect(rigidBody.velocity, other.contacts[0].normal);
			bounces++;
		} else if (other.gameObject.tag == "Player") {
			Destroy (gameObject);
			if (!other.gameObject.GetComponent<PlayerController> ().IsDead) {
				other.gameObject.GetComponent<PlayerController> ().CmdSetPlayerDead();
				GameObject controller = GameObject.FindGameObjectWithTag ("GameController");
				controller.GetComponent<GameController> ().ResetPlayers ();
			}
		}
	}
}
