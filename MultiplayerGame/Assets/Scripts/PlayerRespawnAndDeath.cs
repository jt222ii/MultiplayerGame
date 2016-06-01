using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerRespawnAndDeath : NetworkBehaviour {

	//private Transform spawnPos;
	//private GameObject newPlayer;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator DestroyAndRespawn(float delay) {
		yield return new WaitForSeconds(delay);

		CmdDieAndRespawn ();
		RemoveProjectiles ();
	}
	[Command]
	void CmdDieAndRespawn(){
		var spawn = NetworkManager.singleton.GetStartPosition();
		var newPlayer = ( GameObject) Instantiate(NetworkManager.singleton.playerPrefab, spawn.position, spawn.rotation );
		NetworkServer.Destroy( this.gameObject );
		NetworkServer.ReplacePlayerForConnection( this.connectionToClient, newPlayer, this.playerControllerId );
	}
	private void RemoveProjectiles()
	{
		GameObject[] projectiles = GameObject.FindGameObjectsWithTag ("Projectile");
		foreach (GameObject projectile in projectiles) {
			Destroy (projectile);
		}
	}
} 
