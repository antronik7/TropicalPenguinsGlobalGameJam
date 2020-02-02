using System;
using UnityEngine;

public enum GameState
{
	MainMenu,
	Play,
	EndGame,
}

public class GameManager : Singleton<GameManager>
{
	public int scoreToWin = 50;
	[NonSerialized] public readonly int[] playerScores = new int[4];

	public GameState state;

	//Awake is always called before any Start functions
	protected override void Awake()
	{
		base.Awake();

		EventManager.PlayerScored.AddListener((player, _, scoreIncrement) => addToPlayerScore(player.playerId, scoreIncrement));
	}

	private void Start()
	{
		EventManager.GameplayReady.Invoke();
	}

	private void addToPlayerScore(int playerID, int score)
	{
		playerScores[playerID] += score;
		if (CheckIfWinner())
		{
			EventManager.GameplayEnd.Invoke();
		}
	}

	private bool CheckIfWinner()
	{
		for (int i = 0; i < playerScores.Length; ++i)
		{
			if (playerScores[i] >= scoreToWin)
				return true;
		}

		return false;
	}
}
