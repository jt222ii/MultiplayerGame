using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public GameObject projectile;
    public Transform projectileSpawn;
    public float fireRate;


    private float nextShot;
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Fire1") && Time.time > nextShot)
        {
            nextShot = Time.time + (1 / fireRate);          
            Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
        }
    }
}
