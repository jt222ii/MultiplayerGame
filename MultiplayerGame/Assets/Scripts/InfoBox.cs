using UnityEngine;
using System.Collections;

public class InfoBox : MonoBehaviour {

	public Canvas menu;
	void Start()
	{
		menu.enabled = false;
	}
	public void hideMenu()
	{
		menu.enabled = false;
	}
	public void showMenu()
	{
		menu.enabled = true;
	}
}
