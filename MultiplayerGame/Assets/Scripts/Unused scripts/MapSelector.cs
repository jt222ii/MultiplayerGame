using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MapSelector : MonoBehaviour {
	public Object[] maps;
	public NetworkManager manager;
	public void chooseMap(int index){
		Debug.Log (maps [index].name);
		manager.onlineScene = maps[index].name;

	}
}
