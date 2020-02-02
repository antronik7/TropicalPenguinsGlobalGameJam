using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	public int scoreToWin = 50;
	[NonSerialized] public readonly int[] playerScores = new int[4];

	//Awake is always called before any Start functions
	protected override void Awake()
	{
		base.Awake();

		EventManager.PlayerScored.AddListener((player, _, scoreIncrement) => addToPlayerScore(player.playerId, scoreIncrement));
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void addToPlayerScore(int playerID, int score)
	{
		playerScores[playerID] += score;
		CheckIfWinner();
	}

	private int CheckIfWinner()
	{
		for (int i = 0; i < playerScores.Length; ++i)
		{
			if (playerScores[i] >= scoreToWin)
				return i;
		}

		return -1;
	}
}
