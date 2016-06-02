
namespace UnityEngine.Networking
{
	using UnityEngine.UI;
	[AddComponentMenu("Network/NetworkManagerHUD")]
	[RequireComponent(typeof(NetworkManager))]
	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	public class NetworkManagerHud_Custom : MonoBehaviour
	{
		public NetworkManager manager;
		[SerializeField] public bool showGUI = true;
		[SerializeField] public int offsetX;
		[SerializeField] public int offsetY;

		public Vector2 buttonSize;
		public int spacing;
		public GUIStyle myStyle;

		// Runtime variable
		bool showServer = false;

		void Awake()
		{
			manager = GetComponent<NetworkManager>();
		}

		void Update()
		{
			if (!showGUI)
				return;

			if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
			{
				if (Input.GetKeyDown(KeyCode.S))
				{
					manager.StartServer();
				}
				if (Input.GetKeyDown(KeyCode.H))
				{
					manager.StartHost();
				}
				if (Input.GetKeyDown(KeyCode.C))
				{
					manager.StartClient();
				}
				if (Input.GetKeyDown(KeyCode.M))
				{
					manager.StartMatchMaker();
				}

				/*if (Input.GetKeyDown(KeyCode.J))
                {
                    manager.matchName = match.name;
                    manager.matchSize = (uint)match.currentSize;
                    manager.matchMaker.JoinMatch(match.networkId, "", manager.OnMatchJoined);
                }*/
			}
			if (NetworkServer.active && NetworkClient.active)
			{
				if (Input.GetKeyDown(KeyCode.X))
				{
					manager.StopHost();
				}
			}
		}

		void OnGUI()
		{
			if (!showGUI)
				return;
			float xpos = Screen.width/2 - buttonSize.x/2 + offsetX;
			float ypos = Screen.height/2 + buttonSize.y/2 + offsetY;
			if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
			{
				if (GUI.Button(new Rect(xpos, ypos, buttonSize.x, buttonSize.y), "LAN Host(H)", myStyle))
				{
					manager.StartHost();
				}
				ypos += spacing;

				if (GUI.Button(new Rect(xpos, ypos, buttonSize.x/2, buttonSize.y), "LAN Client(C)", myStyle))
				{
					manager.StartClient();
				}
				manager.networkAddress = GUI.TextField(new Rect(xpos + 100, ypos, buttonSize.x/2, buttonSize.y), manager.networkAddress,myStyle);
				ypos += spacing;

				/*if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Server Only(S)"))
				{
					manager.StartServer();
				}
				ypos += spacing;*/
			}
			/*else
			{
				if (NetworkServer.active)
				{
					GUI.Label(new Rect(xpos, ypos, 300, 20), "Server: port=" + manager.networkPort);
					ypos += spacing;
				}
				if (NetworkClient.active)
				{
					GUI.Label(new Rect(xpos, ypos, 300, 20), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort);
					ypos += spacing;
				}
			}*/

			if (NetworkClient.active && !ClientScene.ready)
			{
				if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Client Ready")||Input.GetKeyDown(KeyCode.R))
				{
					ClientScene.Ready(manager.client.connection);

					if (ClientScene.localPlayers.Count == 0)
					{
						ClientScene.AddPlayer(0);
					}
				}
				ypos += spacing;
			}

			if (NetworkServer.active || NetworkClient.active)
			{
				if (/*GUI.Button(new Rect(xpos, ypos, 200, 20), "Stop (Esc)", myStyle)||*/ Input.GetKeyDown(KeyCode.Escape))
				{
					manager.StopHost();
				}
				ypos += spacing;
			}

			if (!NetworkServer.active && !NetworkClient.active)
			{
				ypos += 10;

				if (manager.matchMaker == null || Input.GetKeyDown(KeyCode.M))
				{
					if (GUI.Button(new Rect(xpos, ypos, buttonSize.x, buttonSize.y), "Enable Match Maker (M)", myStyle))
					{
						manager.StartMatchMaker();
					}
					ypos += spacing;
				}
				else
				{
					if (manager.matchInfo == null)
					{
						if (manager.matches == null)
						{
							if (GUI.Button(new Rect(xpos, ypos, buttonSize.x, buttonSize.y), "Create Internet Match", myStyle)|| Input.GetKeyDown(KeyCode.N))
							{
								manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", manager.OnMatchCreate);
							}
							ypos += spacing;

							GUI.Label(new Rect(xpos, ypos, buttonSize.x/2, buttonSize.y), "Room Name:", myStyle);
							manager.matchName = GUI.TextField(new Rect(xpos+100, ypos, buttonSize.x/2, buttonSize.y), manager.matchName, myStyle);
							ypos += spacing;

							ypos += 10;

							if (GUI.Button(new Rect(xpos, ypos, buttonSize.x, buttonSize.y), "Find Internet Match", myStyle)|| Input.GetKeyDown(KeyCode.J))
							{
								manager.matchMaker.ListMatches(0,20, "", manager.OnMatchList);
							}
							ypos += spacing;
						}
						else
						{
							foreach (var match in manager.matches)
							{
								if (GUI.Button(new Rect(xpos, ypos, buttonSize.x, buttonSize.y), "Join Match:" + match.name,myStyle)|| Input.GetKeyDown(KeyCode.K))
								{
									manager.matchName = match.name;
									Debug.Log(match.name);
									manager.matchSize = (uint)match.currentSize;
									manager.matchMaker.JoinMatch(match.networkId, "", manager.OnMatchJoined);
								}
								ypos += spacing;
							}
						}
					}

					/*if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Change MM server"))
					{
						showServer = !showServer;
					}*/
					if (showServer)
					{
						ypos += spacing;
						if (GUI.Button(new Rect(xpos, ypos, buttonSize.x/2, buttonSize.y), "Local", myStyle))
						{
							manager.SetMatchHost("localhost", 1337, false);
							showServer = false;
						}
						ypos += spacing;
						if (GUI.Button(new Rect(xpos, ypos, buttonSize.x/2, buttonSize.y), "Internet", myStyle))
						{
							manager.SetMatchHost("mm.unet.unity3d.com", 443, true);
							showServer = false;
						}
						ypos += spacing;
						if (GUI.Button(new Rect(xpos, ypos, buttonSize.x/2, buttonSize.y), "Staging", myStyle))
						{
							manager.SetMatchHost("staging-mm.unet.unity3d.com", 443, true);
							showServer = false;
						}
					}

					ypos += spacing;

					/*GUI.Label(new Rect(xpos, ypos, buttonSize.x, buttonSize.y), "MM Uri: " + manager.matchMaker.baseUri);
					ypos += spacing;*/

					if (GUI.Button(new Rect(xpos, ypos, buttonSize.x, buttonSize.y), "Disable Match Maker", myStyle))
					{
						manager.StopMatchMaker();
					}
					ypos += spacing;
				}
			}
		}
	}
};
