using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuUIController : MonoBehaviour
{

	public AK.Wwise.Event UIConfirmation;

	public void OnPressPlay()
	{
		//Debug.Log("play");
		MenuSpawnerManager menuSpawnerManager = MenuSpawnerManager.Instance;
		EventManager.GameplayStart.Invoke();
		UIConfirmation.Post(gameObject);
		Debug.Log("Post UIConfirmation");
	}

	public void OnPressQuit()
	{
		UIConfirmation.Post(gameObject);
		Application.Quit();
	}
}
