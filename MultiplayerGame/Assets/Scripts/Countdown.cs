using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Countdown : MonoBehaviour {

	public bool countDownStarted = false;
	public Text countDownText;
	float timeRemaining;
	// Use this for initialization
	void Start () {
		
	}
	
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
	public void startCountDown(float seconds){
		timeRemaining = seconds;
		countDownStarted = true;
	}
}
