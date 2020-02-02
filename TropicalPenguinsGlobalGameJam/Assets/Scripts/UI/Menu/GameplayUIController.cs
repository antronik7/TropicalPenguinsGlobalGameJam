using System;
using UnityEngine;
using TMPro;

public class GameplayUIController : MonoBehaviour
{
	public PlayerUIOverlayElement Player1;
	public PlayerUIOverlayElement Player2;
	public PlayerUIOverlayElement Player3;
	public PlayerUIOverlayElement Player4;
	public TextMeshProUGUI Timer;

	private TimeSpan timerSpan = new TimeSpan();

	private PlayerUIOverlayElement[] PlayerUIArray;

	private void Awake()
	{
		PlayerUIArray = new PlayerUIOverlayElement[] { Player1, Player2, Player3, Player4 };
		Timer.gameObject.SetActive(false);

		foreach (var playerUI in PlayerUIArray)
		{
			playerUI.gameObject.SetActive(false);
		}

		EventManager.GameplayStart.AddListener(() =>
		{
			int playerCount = PlayerManager.Instance.PlayerCount;
			for (int i = 0; i < playerCount; ++i)
			{
				PlayerUIArray[i].gameObject.SetActive(true);
			}

			Timer.gameObject.SetActive(true);
			timerSpan = new TimeSpan(0, 0, 90);
			Timer.SetText($"{timerSpan.Minutes}:{timerSpan.Seconds}");
		});

		EventManager.GameplayEnd.AddListener(() =>
		{
			foreach (var playerUI in PlayerUIArray)
			{
				playerUI.gameObject.SetActive(false);
			}
			Timer.gameObject.SetActive(false);
		});

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
	}

	private void FixedUpdate()
	{
		UpdateTimer((int)(Time.fixedDeltaTime * 1000));
	}
	//public void OnEndGame()
	//{
	//	MenuSpawnerManager menuSpawnerManager = MenuSpawnerManager.Instance;
	//	menuSpawnerManager.SwitchUI(menuSpawnerManager.EndUI);
	//}


	public void UpdateTimer(int deltaInMilli)
	{
		if (timerSpan == null)
			return;

		timerSpan = timerSpan.Subtract(new TimeSpan(0, 0, 0, 0, deltaInMilli));
		if (timerSpan.Minutes > 0)
			Timer.SetText($"{timerSpan.Minutes}:{timerSpan.Seconds}");
		else
			Timer.SetText($"{timerSpan.Seconds}");

		if (timerSpan.Ticks <= 0)
			EventManager.GameplayEnd.Invoke();
	}
}
