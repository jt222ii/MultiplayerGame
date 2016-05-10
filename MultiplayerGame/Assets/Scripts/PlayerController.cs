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

	public bool grounded;


	[SyncVar]public int currentWeapon;
	public Transform[] weapons;
	public GameObject[] weps2;
	public GameObject[] projectiles;
	public float[] fireRates;
	private Rigidbody2D rigidBody;
	private Collider2D collider;

	private NetworkInstanceId networkId;
//    // Use this for initialization
    void Start () {
		//changeWeapon (currentWeapon);
		collider = GetComponent<Collider2D>();
		rigidBody = gameObject.GetComponent<Rigidbody2D>();
		CmdChangeWeapon (currentWeapon);
		networkId = gameObject.GetComponent<NetworkIdentity> ().netId;
	}
	/*void OnStartLocalPlayer () {
		//changeWeapon (currentWeapon);
		rigidBody = gameObject.GetComponent<Rigidbody2D>();
		CmdChangeWeapon (currentWeapon);

	}*/

	void OnCollisionEnter2D(Collision2D other)
	{
		/*if (other.gameObject.tag == "Projectile") {
			Destroy (other.gameObject);
			Destroy (gameObject);
		}*/
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
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
			//Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
			CmdFire (projectileSpawn.rotation, networkId);
        }
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
	[Command]
	public void CmdChangeWeapon(int index)
	{
		//Destroy (gameObject.transform.GetChild (0));
		if (gameObject.transform.childCount > 0) {
			NetworkServer.Destroy(gameObject.transform.GetChild (0).gameObject);
		}
		projectile = projectiles[index];

		GameObject weapon = GameObject.Instantiate(weps2[index], rigidBody.position, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
		weapon.transform.parent = gameObject.transform;
		weapon.transform.localScale = new Vector3(2f, 2f, 2f);//Fullösnign för tillfället
		projectileSpawn = weapon.gameObject.transform.GetChild (0);
		NetworkServer.SpawnWithClientAuthority (weapon, gameObject);
		//NetworkServer.Spawn (weapon);
		RpcSyncWeaponChange (weapon);
	}
	[ClientRpc]
	public void RpcSyncWeaponChange(GameObject weapon){
		weapon.transform.parent = gameObject.transform;
		weapon.transform.localScale = new Vector3(2f, 2f, 2f);//Fullösnign för tillfället
		projectileSpawn = weapon.gameObject.transform.GetChild (0);
	}
	void Jump()
	{
		rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
	}
	[Command]
	public void CmdFire(Quaternion rotation, NetworkInstanceId netId)
	{
		GameObject projectileObject = (GameObject)Instantiate(projectile, projectileSpawn.position, rotation);
		projectileObject.GetComponent<ProjectileScript> ().spawnedBy = netId;
		//Physics2D.IgnoreCollision (projectileObject.GetComponent<Collider2D> (), collider);
		NetworkServer.Spawn (projectileObject);
	}
}
