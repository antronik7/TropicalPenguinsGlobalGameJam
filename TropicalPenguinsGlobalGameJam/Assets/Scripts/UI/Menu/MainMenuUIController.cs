using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuUIController : MonoBehaviour
{

	public AK.Wwise.Event UIConfirmation;

	private void Awake()
	{
		EventManager.GameplayReady.AddListener(() =>
		{
			gameObject.SetActive(true);
		});

		EventManager.GameplayStart.AddListener(() =>
		{
			gameObject.SetActive(false);
		});

		gameObject.SetActive(false);
	}

	public void OnPressPlay()
	{
		//Debug.Log("play");
		EventManager.GameplayStart.Invoke();
		UIConfirmation.Post(gameObject);
	}

	public void OnPressQuit()
	{
		UIConfirmation.Post(gameObject);
		Application.Quit();
	}
}
