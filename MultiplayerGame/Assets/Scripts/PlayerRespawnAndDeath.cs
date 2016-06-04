using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerRespawnAndDeath : NetworkBehaviour {


	public IEnumerator DestroyAndRespawn(float delay) 
	{
		yield return new WaitForSeconds(delay);

		CmdDieAndRespawn ();
		RemoveProjectiles ();
	}
	[Command]
	void CmdDieAndRespawn()//destroys the gameobject and replaces it at spawn. Effectively "respawning" the player
	{
		var spawn = NetworkManager.singleton.GetStartPosition();
		var newPlayer = ( GameObject) Instantiate(NetworkManager.singleton.playerPrefab, spawn.position, spawn.rotation );
		NetworkServer.Destroy( this.gameObject );
		NetworkServer.ReplacePlayerForConnection( this.connectionToClient, newPlayer, this.playerControllerId );
	}
	private void RemoveProjectiles()//removes all projectiles on the map
	{
		GameObject[] projectiles = GameObject.FindGameObjectsWithTag ("Projectile");
		foreach (GameObject projectile in projectiles) {
			Destroy (projectile);
		}
	}
} 
