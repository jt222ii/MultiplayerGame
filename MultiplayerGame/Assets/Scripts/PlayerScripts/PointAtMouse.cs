﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PointAtMouse : NetworkBehaviour
{
	private bool isParentLocalPlayer;
	// Use this for initialization
	void Start () {
		isParentLocalPlayer = transform.parent.GetComponent<PlayerController> ().isLocalPlayer;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isParentLocalPlayer) {
			return;
		}
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 5.23f;

		Vector3 objectPos = Camera.main.WorldToScreenPoint (transform.position);
		mousePos.x = mousePos.x - objectPos.x;
		mousePos.y = mousePos.y - objectPos.y;

		float angle = Mathf.Atan2 (mousePos.y, mousePos.x) * Mathf.Rad2Deg; //angle of rotation to mouse

		CmdRotation(Quaternion.Euler (new Vector3 (0, 0, angle)));
		
    }
	[Command]
	public void CmdRotation(Quaternion rotation)
	{
		RpcSyncRotation (rotation);
	}
	[ClientRpc]
	public void RpcSyncRotation(Quaternion rotation){
		transform.rotation = rotation;//set rotation to make the weapon point at mouse
	}

}
