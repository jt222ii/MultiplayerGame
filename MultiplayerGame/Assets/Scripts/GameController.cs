using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour {
	public float respawnTime;
	//Finds the player objects and calls a function to respawn the players.
	public void ResetPlayers(){
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		gameObject.GetComponent<Countdown> ().startCountDown (respawnTime);
		foreach (GameObject player in players) {
			StartCoroutine (player.GetComponent<PlayerRespawnAndDeath> ().DestroyAndRespawn (respawnTime));
		}
	}
}
