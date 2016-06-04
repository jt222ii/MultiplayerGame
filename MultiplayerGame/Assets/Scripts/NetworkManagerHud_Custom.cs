
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
		[SerializeField] public int offsetX;//Offset for the meny on the x-axis
		[SerializeField] public int offsetY;//Offset for the meny on the y-axis

		public Vector2 buttonSize;//size of the buttons
		public int spacing;//spacing between the buttons
		public GUIStyle myStyle;//Style for the buttons


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
				//Button to host a LAN game. If pressed the networkmanager will start a hosted lan game
				if (GUI.Button(new Rect(xpos, ypos, buttonSize.x, buttonSize.y), "LAN Host(H)", myStyle))
				{
					manager.StartHost();
				}
				ypos += spacing;

				//Button to join a LAN game. If pressed the networkmanager will join a LAN game.
				if (GUI.Button(new Rect(xpos, ypos, buttonSize.x/2, buttonSize.y), "LAN Client(C)", myStyle))
				{
					manager.StartClient();
				}
				//Textfield where you can specify the address for the game. Defaults to localhost
				manager.networkAddress = GUI.TextField(new Rect(xpos + 100, ypos, buttonSize.x/2, buttonSize.y), manager.networkAddress,myStyle);
				ypos += spacing;
			}

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
				//if escape is pressed disconnect from the server
				if (Input.GetKeyDown(KeyCode.Escape))
				{
					manager.StopHost();
				}
				ypos += spacing;
			}

			if (!NetworkServer.active && !NetworkClient.active)
			{

				if (manager.matchMaker == null || Input.GetKeyDown(KeyCode.M))
				{
					//Button to enable matchmaking. If pressed - start matchmaker.
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
							//Button to create internet match. if pressed create a match
							if (GUI.Button(new Rect(xpos, ypos, buttonSize.x, buttonSize.y), "Create Internet Match", myStyle)|| Input.GetKeyDown(KeyCode.N))
							{
								manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", manager.OnMatchCreate);
							}
							ypos += spacing;

							GUI.Label(new Rect(xpos, ypos, buttonSize.x/2, buttonSize.y), "Room Name:", myStyle);
							manager.matchName = GUI.TextField(new Rect(xpos+100, ypos, buttonSize.x/2, buttonSize.y), manager.matchName, myStyle);
							ypos += spacing;

							ypos += 10;

							//button to search for internet matches. If pressed a list of available matches should appear if there is any.
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
								//button for each match that you can join-
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

					ypos += spacing;

					//Button for disabling the matchmaker. When pressed the matchmaker is stopped.
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
