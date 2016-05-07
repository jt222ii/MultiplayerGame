using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class weapon_rotationSync : NetworkBehaviour {

	[SyncVar] private Quaternion syncWeaponRotation;
	[SerializeField] private Transform weaponTransform;
	[SerializeField] private float lerpRate = 15;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		Rotate ();
		LerpRotations ();
	}

	void LerpRotations()
	{
		if(!isLocalPlayer){
			weaponTransform.rotation = Quaternion.Lerp (weaponTransform.rotation, syncWeaponRotation, Time.deltaTime * lerpRate);

		}
	}

	[Command]
	void CmdSyncRotations(Quaternion weaponRot)
	{
		syncWeaponRotation = weaponRot;
	}
	[Client]
	void Rotate()
	{
		if (isLocalPlayer) {
			CmdSyncRotations (weaponTransform.rotation);
		}
	}
}
