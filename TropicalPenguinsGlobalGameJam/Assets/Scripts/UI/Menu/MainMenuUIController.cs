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

    public void OnPressPlay1()
    {
        PlayerManager.Instance.SetPlayerCount(1);
        OnPressPlay();
    }

    public void OnPressPlay2()
    {
        PlayerManager.Instance.SetPlayerCount(2);
        OnPressPlay();
    }

    public void OnPressPlay3()
    {
        PlayerManager.Instance.SetPlayerCount(3);
        OnPressPlay();
    }

    public void OnPressPlay4()
    {
        PlayerManager.Instance.SetPlayerCount(4);
        OnPressPlay();
    }

    private void OnPressPlay()
	{
		//Debug.Log("play");
		EventManager.GameplayStart.Invoke();
		UIConfirmation.Post(gameObject);
        BlockSpawner.Instance.StartSpawningBlocks();
	}

	public void OnPressQuit()
	{
		UIConfirmation.Post(gameObject);
		Application.Quit();
	}
}
