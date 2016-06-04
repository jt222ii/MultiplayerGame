using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Countdown : NetworkBehaviour {
	[SyncVar]public bool countDownStarted = false;
	[SyncVar]float timeRemaining;
	public Text countDownText;

	
	// Update is called once per frame
	void Update () {
		if (countDownStarted) {
			timeRemaining -= Time.deltaTime;
			countDownText.text = "A player died!\nRestarting in: " + (int)timeRemaining;
		}
		if (timeRemaining < 0) {
			countDownText.text = "";
			countDownStarted = false;
		}
	}
	//starts the countdown for specified amount of seconds
	public void startCountDown(float seconds){
		timeRemaining = seconds;
		countDownStarted = true;
	}
}
