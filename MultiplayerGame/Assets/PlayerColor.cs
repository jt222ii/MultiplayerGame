using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerColor : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			Debug.Log ("hejhej");
			var hej = GetComponentInChildren<SpriteRenderer> ();
			Debug.Log (hej);
			GetComponentInChildren<SpriteRenderer> ().color = Color.white;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
