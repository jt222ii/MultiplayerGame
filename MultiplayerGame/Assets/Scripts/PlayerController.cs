using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
    public GameObject projectile;
    public Transform projectileSpawn;
    public float fireRate;

    private float nextShot;

	public int currentWeapon;
	public Transform[] weapons;
	public GameObject[] projectiles;
	public float[] fireRates;

    // Use this for initialization
    void Start () {
		changeWeapon (currentWeapon);

	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
        {
            return;
        }
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			changeWeapon (0);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			changeWeapon (1);
		}
        if (Input.GetButton("Fire1") && Time.time > nextShot)
        {
            nextShot = Time.time + (1 / fireRate);          
            Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
        }
    }
	public void changeWeapon(int index){
		currentWeapon = index;
		for (int i = 0; i < weapons.Length; i++) {
			if (i == index) {
				fireRate = fireRates [i];
				weapons [i].gameObject.SetActive (true);
				projectile = projectiles[i];
				projectileSpawn = weapons [i].gameObject.transform.GetChild (0);
			} else {
				weapons [i].gameObject.SetActive (false);
			}
				
		}
	}
}
