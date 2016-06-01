using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
    public GameObject projectile;
    public Transform projectileSpawn;
    public float fireRate;
    private float nextShot;
	public float movementSpeed;
	public float jumpForce;
	private bool isDead = false;

	public bool grounded;

	//Animation
	public Animator animator;
	bool facingRight = true;
	[SyncVar]public float moveSpeed;

	//weapons
	[SyncVar]public int currentWeapon;
	public Transform[] weapons;
	public GameObject[] weps2;
	public GameObject[] projectiles;
	public float[] fireRates;


	private Rigidbody2D rigidBody;
	private Collider2D collider;

	private NetworkInstanceId networkId;

    void Start () {
		collider = GetComponent<Collider2D>();
		rigidBody = gameObject.GetComponent<Rigidbody2D>();
		CmdChangeWeapon (currentWeapon);
		networkId = gameObject.GetComponent<NetworkIdentity> ().netId;
		GetComponent<NetworkAnimator> ().SetParameterAutoSend (0, true);
	}

	public override void PreStartClient()
	{
		GetComponent<NetworkAnimator> ().SetParameterAutoSend (0, true);
	}
		
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer || isDead)
        {
            return;
        }
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			CmdChangeWeapon (0);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			CmdChangeWeapon (1);
		}
        if (Input.GetButton("Fire1") && Time.time > nextShot)
        {
            nextShot = Time.time + (1 / fireRate);      
			CmdFire (projectileSpawn.rotation, networkId);
        }
		if (Input.GetButtonDown("Jump"))
		{
			Jump();
		}
    }

	void FixedUpdate()
	{
		if (isDead) {
			return;
		}
		float horizontal = Input.GetAxis("Horizontal");
		float velocityX = movementSpeed * horizontal;

		moveSpeed = horizontal;
		if (moveSpeed > 0.01f || moveSpeed < -0.01f) {
			GetComponent<NetworkAnimator> ().SetTrigger ("Run");
		} else if (moveSpeed < 0.01f || moveSpeed > -0.01f) {
			GetComponent<NetworkAnimator> ().SetTrigger ("Idle");
		}

		animator.SetFloat ("Speed", Mathf.Abs (moveSpeed));

		if (isLocalPlayer) {
			if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) {
				Flip ();
			}
		}
		rigidBody.velocity = new Vector2(velocityX, rigidBody.velocity.y);

	}

	void Jump()
	{
		rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
	}

	//flips the sprite based on direction you are moving
	void Flip()
	{
		facingRight = !facingRight;
		GetComponent<SpriteRenderer> ().flipX = !facingRight;
		foreach (Transform child in transform) {
			if (child.gameObject.tag != "Weapon") {
				SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer> ();
				Vector3 position = child.transform.localPosition;
				position.x *= -1;
				child.transform.localPosition = position;
				spriteRenderer.flipX = !spriteRenderer.flipX;
			}
		}
	}
		
	public bool IsDead
	{
		get{ return isDead; }
		set
		{ 
			if (value) {
				gameObject.GetComponent<ParticleSystem> ().Play ();
			} else if (!value) {
				gameObject.GetComponent<ParticleSystem> ().Stop ();
			}
			isDead = value; 
		}
	}

	[Command]
	public void CmdChangeWeapon(int index)
	{
		if (gameObject.transform.childCount > 0) {
			Transform[] children = gameObject.GetComponentsInChildren<Transform>();
			foreach (Transform child in transform) {
				if (child.gameObject.tag == "Weapon") {
					NetworkServer.Destroy (child.gameObject);
				}
			}
			//NetworkServer.Destroy(gameObject.transform.GetChild (0).gameObject);
		}
		projectile = projectiles[index];

		GameObject weapon = GameObject.Instantiate(weps2[index], rigidBody.position, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
		weapon.transform.parent = gameObject.transform;
		projectileSpawn = weapon.gameObject.transform.GetChild (0);
		NetworkServer.SpawnWithClientAuthority (weapon, gameObject);
		RpcSyncWeaponChange (weapon);
	}
	[ClientRpc]
	public void RpcSyncWeaponChange(GameObject weapon){
		weapon.transform.parent = gameObject.transform;
		projectileSpawn = weapon.gameObject.transform.GetChild (0);
	}
	[Command]
	public void CmdFire(Quaternion rotation, NetworkInstanceId netId)
	{
		GameObject projectileObject = (GameObject)Instantiate(projectile, projectileSpawn.position, rotation);
		projectileObject.GetComponent<ProjectileScript> ().spawnedBy = netId;
		NetworkServer.Spawn (projectileObject);
	}
}
