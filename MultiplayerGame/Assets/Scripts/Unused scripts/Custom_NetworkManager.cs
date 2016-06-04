using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Custom_NetworkManager : NetworkManager {

	public void StartUpLanHost()
	{
		NetworkManager.singleton.StartHost ();
	}

	public void JoinLanGame()
	{
		SetIPAddress ();
		NetworkManager.singleton.StartClient ();
	}

	public void StartupMatchMaker()
	{
		NetworkManager.singleton.StartMatchMaker ();
	}

	public void CreateInternetMatch()
	{
		NetworkManager.singleton.matchMaker.CreateMatch (NetworkManager.singleton.matchName, NetworkManager.singleton.matchSize, true, "", NetworkManager.singleton.OnMatchCreate);
	}

	public void FindMatches()
	{
		int xpos = 10;
		int ypos = 40;
		int spacing = 24;
		NetworkManager.singleton.matchMaker.ListMatches (0, 20, "", NetworkManager.singleton.OnMatchList);
		foreach (var match in NetworkManager.singleton.matches)
		{
			if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Join Match:" + match.name))
			{
				NetworkManager.singleton.matchName = match.name;
				NetworkManager.singleton.matchSize = (uint)match.currentSize;
				NetworkManager.singleton.matchMaker.JoinMatch(match.networkId, "", NetworkManager.singleton.OnMatchJoined);
			}
			ypos += spacing;
		}
	}
		
	void SetIPAddress()
	{
		string ipAddress = GameObject.Find ("InputFieldIPAddress").transform.FindChild ("Text").GetComponent<Text> ().text;
		Debug.Log (ipAddress);
		NetworkManager.singleton.networkAddress = ipAddress;
	}

	void OnLevelWasLoaded(int level)
	{
		if (level == 0) {
			SetupMenuSceneButtons ();
		} else {
			//SetupOtherSceneButtons ();
		}
	}

	void SetupMenuSceneButtons()
	{
		GameObject.Find ("HostLan").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("HostLan").GetComponent<Button> ().onClick.AddListener (StartUpLanHost);

		GameObject.Find ("JoinLan").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("JoinLan").GetComponent<Button> ().onClick.AddListener (JoinLanGame);

		GameObject.Find ("Enablematchmaker").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("Enablematchmaker").GetComponent<Button> ().onClick.AddListener (StartupMatchMaker);

		GameObject.Find ("FindMatches").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("FindMatches").GetComponent<Button> ().onClick.AddListener (FindMatches);
	}
}
