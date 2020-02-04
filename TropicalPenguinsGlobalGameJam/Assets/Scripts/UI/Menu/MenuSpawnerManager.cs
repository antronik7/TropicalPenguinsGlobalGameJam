using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSpawnerManager : MonoBehaviour
{
	private static MenuSpawnerManager _instance;

	public static MenuSpawnerManager Instance { get { return _instance; } }

	public MainMenuUIController MainMenuUI;

	public GameplayUIController GameUI;

	public EndGameUIController EndUI;

	private void Awake()
	{
		EventManager.GameplayEnd.AddListener(() =>
		{
			EndUI.UpdateScores(GameManager.Instance.playerScores);
			MainMenuUI.gameObject.SetActive(false);
			EndUI.gameObject.SetActive(false);
		});

		EventManager.GameplayReady.AddListener(() =>
		{
			MainMenuUI.gameObject.SetActive(true);
			EndUI.gameObject.SetActive(false);
		});

		EventManager.GameplayStart.AddListener(() =>
		{
			MainMenuUI.gameObject.SetActive(false);
			EndUI.gameObject.SetActive(false);
		});

		EventManager.GameplayEnd.AddListener(() =>
		{
			MainMenuUI.gameObject.SetActive(false);
			EndUI.gameObject.SetActive(true);
		});
	}
	private void Start()
	{
		if (_instance != null && _instance != this)
		{
			Debug.Log("Spawned extra MenuSpawnerManager");
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
			//SwitchUI(MainMenuUI);
		}
	}

	public void SwitchUI(int ui)
	{
		switch (ui)
		{
			case 0:
				MainMenuUI.gameObject.SetActive(true);
				EndUI.gameObject.SetActive(false);
				break;
			case 1:
				Debug.Log(MainMenuUI);
				MainMenuUI.gameObject.SetActive(false);
				EndUI.gameObject.SetActive(false);
				break;
			case 2:
				MainMenuUI.gameObject.SetActive(false);
				EndUI.gameObject.SetActive(true);
				break;
		}
	}
}
