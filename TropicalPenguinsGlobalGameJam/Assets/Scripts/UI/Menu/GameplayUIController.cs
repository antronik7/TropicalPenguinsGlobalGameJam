using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayUIController : MonoBehaviour
{
	public PlayerUIOverlayElement Player1;
	public PlayerUIOverlayElement Player2;
	public PlayerUIOverlayElement Player3;
	public PlayerUIOverlayElement Player4;
	public TextMeshProUGUI Timer;

	private PlayerUIOverlayElement[] PlayerUIArray;

	private void Awake()
	{
		PlayerUIArray = new PlayerUIOverlayElement[] { Player1, Player2, Player3, Player4 };

		EventManager.PlayerScored.AddListener((player, previousScore, scoreIncrement) =>
		{
			PlayerUIArray[player.playerId].UpdateScore(previousScore + scoreIncrement);
		});
	}
	private void Start()
	{
		//UpdateScore(1, 1);
		//UpdateScore(2, 4);
		//UpdateScore(3, 9);
		//UpdateScore(4, 16);
		UpdateTimer(90);
	}

	public void OnEndGame()
	{
		MenuSpawnerManager menuSpawnerManager = MenuSpawnerManager.Instance;
		menuSpawnerManager.SwitchUI(menuSpawnerManager.EndUI);
	}


	public void UpdateTimer(int seconds)
	{
		int min = seconds / 60;
		int sec = seconds % 60;

		string timerTxt = $"{min}:{sec}";
		Timer.SetText(timerTxt);
	}
}
