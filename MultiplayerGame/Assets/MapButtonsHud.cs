using UnityEngine;
using System.Collections;
namespace UnityEngine.Networking
{
	public class MapButtonsHud : MonoBehaviour {

		// Use this for initialization
		[SerializeField] public int offsetX;
		[SerializeField] public int offsetY;
		public Vector2 buttonSize;
		public string[] mapNames;
		public NetworkManager manager;
		public NetworkManagerHud_Custom managerHud;
		public int spacing;

		void Start () {
			manager = GetComponent<NetworkManager> ();
			managerHud = GetComponent<NetworkManagerHud_Custom> ();
		}

		void OnGUI(){
			if (!managerHud.showGUI) {
				return;
			}
			if (!NetworkClient.active) {
				float mapButtonPosX = Screen.width / 2 - buttonSize.x / 2 + offsetX;
				float mapButtonPosY = Screen.height / 2 + buttonSize.y / 2 + offsetY;
				for (int i = 0; i < 2; i++) {
					var test = mapNames[i];
					if (GUI.Button (new Rect (mapButtonPosX, mapButtonPosY, buttonSize.x / 2, buttonSize.y), test, managerHud.myStyle)) {
						chooseMap (mapNames[i]);
					}
					mapButtonPosY += spacing;
				}
			}
		}
		public void chooseMap(string mapName){
			manager.onlineScene = mapName;
		}
	}
}
