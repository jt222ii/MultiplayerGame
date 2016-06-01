using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour {
	public float respawnTime;
	private bool isResetting = false;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void ResetPlayers(){
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		gameObject.GetComponent<Countdown> ().startCountDown (respawnTime);
		foreach (GameObject player in players) {
			StartCoroutine (player.GetComponent<PlayerRespawnAndDeath> ().DestroyAndRespawn (respawnTime));
		}
	}
}
