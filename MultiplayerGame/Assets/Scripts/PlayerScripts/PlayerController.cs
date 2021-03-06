﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
    public GameObject projectile;
    public Transform projectileSpawn;
    [SyncVar]public float fireRate;
    private float nextShot;
	public float movementSpeed;
	public float jumpForce;
	[SyncVar]private bool isDead = false;

	public bool grounded = true;
	public bool candoublejump = false;

	//Animation
	public Animator animator;
	bool facingRight = true;
	[SyncVar]public float moveSpeed;

	//weapons
	[SyncVar]public int currentWeapon;
	public GameObject[] weps2;//Array with the different weapons
	public GameObject[] projectiles;//Array with different projectiles
	public float[] fireRates;//Array with different firerates


	private Rigidbody2D rigidBody;

	private NetworkInstanceId networkId;

    void Start () {
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
			GetComponent<NetworkAnimator> ().SetTrigger ("Run");//if player is moving set animationtrigger to run
		} else if (moveSpeed < 0.01f || moveSpeed > -0.01f) {
			GetComponent<NetworkAnimator> ().SetTrigger ("Idle");//if player is not moving set animationtrigger to idle
		}

		animator.SetFloat ("Speed", Mathf.Abs (moveSpeed));

		if (isLocalPlayer) {
			if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) //if player is moving in a new direction. Flip the sprite of the player
			{
				Flip ();
			}
		}
		rigidBody.velocity = new Vector2(velocityX, rigidBody.velocity.y);//set the velocity of the player 

	}
		
	void Jump()
	{
		if (rigidBody.velocity.y == 0) //if player is not moving on the y axis he can jump
		{
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
			candoublejump = true;
		} 
		else if(candoublejump) 
		{
			candoublejump = false;
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
		}

	}

	//flips the sprite of the player
	void Flip()
	{
		facingRight = !facingRight;
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
			isDead = value; 
		}
	}

	[Command]
	public void CmdSetPlayerDead()//sets the player to being dead and plays the particle system simulating blood
	{
		IsDead = true;
		gameObject.GetComponent<ParticleSystem> ().Play ();
		RpcClientParticles ();
	}
	[ClientRpc]
	public void RpcClientParticles()
	{
		gameObject.GetComponent<ParticleSystem> ().Play ();
	}

	[Command]
	public void CmdChangeWeapon(int index)
	{
		if (gameObject.transform.childCount > 0) {
			foreach (Transform child in transform) {
				if (child.gameObject.tag == "Weapon") {
					NetworkServer.Destroy (child.gameObject);
				}
			}
		}
		projectile = projectiles[index];

		GameObject weapon = GameObject.Instantiate(weps2[index], rigidBody.position, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
		fireRate = fireRates [index];
		weapon.transform.parent = gameObject.transform;
		projectileSpawn = weapon.gameObject.transform.GetChild (0);
		NetworkServer.SpawnWithClientAuthority (weapon, gameObject);
		RpcSyncWeaponChange (weapon);
	}
	[ClientRpc]
	public void RpcSyncWeaponChange(GameObject weapon)//synchronize the weapon change
	{
		weapon.transform.parent = gameObject.transform;
		projectileSpawn = weapon.gameObject.transform.GetChild (0);
	}
	[Command]
	public void CmdFire(Quaternion rotation, NetworkInstanceId netId)//Fire the weapon
	{
		GameObject projectileObject = (GameObject)Instantiate(projectile, projectileSpawn.position, rotation);
		projectileObject.GetComponent<ProjectileScript> ().spawnedBy = netId;
		NetworkServer.Spawn (projectileObject);
	}
}
