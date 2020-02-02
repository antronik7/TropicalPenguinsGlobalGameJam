using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HouseManager : MonoBehaviour
{
	private const int houseDimensions = 4;
	public const int nbPlayers = 4;
	private int[] playersBlocksPlaced = new int[nbPlayers];
	public const int maxCubes = 16;
	private int _nbCubes = 0;
	[SerializeField]
	protected TextMeshProUGUI houseScore;
	 
	protected int nbCubes
	{
		get { return _nbCubes; }
		set
		{
			_nbCubes = value;
			houseScore.SetText(_nbCubes.ToString() + "/" + maxCubes.ToString());
		}
	}


	// Start is called before the first frame update
	void Start()
	{
		InitializeConstants();
	}

	public void PlaceBlock(int score, PlayerController player)
	{
		PlayerScore(score, player);
		nbCubes += score;

		if (IsHouseComplete())
		{
			HouseComplete(player);
			gameObject.GetComponent<HouseAnimationScript>().Play();
		}
	}

	private bool CanPlaceBlock(int size)
	{
		return nbCubes + size <= maxCubes;
	}

	public bool IsHouseComplete()
	{
		if (nbCubes >= maxCubes)
		{
			return true;
		}
		return false;
	}

	private void GivePlayerPoints(int playerID, int score)
	{
		//points seulement selon grosseur du block (1 pour 1)
		playersBlocksPlaced[playerID] += score;
	}

	public ShapeType AvailableShapes()
	{
		Array values = Enum.GetValues(typeof(ShapeType));
		System.Random random = new System.Random();
		ShapeType randomShape = (ShapeType)values.GetValue(random.Next(values.Length));
		return randomShape;
	}

	private void HouseComplete(PlayerController player)
	{
		// double the points for who finished it
		EventManager.PlayerScored.Invoke(player, GameManager.Instance.playerScores[player.playerId], playersBlocksPlaced[player.playerId]);

		// animation for when house is complete
		gameObject.GetComponent<HouseAnimationScript>().Play();
		InitializeConstants();
	}

	private void InitializeConstants()
	{
		nbCubes = 0;
		playersBlocksPlaced = new int[nbPlayers];
	}

	private void OnTriggerEnter(Collider other)
	{
		PlayerController player = other.transform.root.GetComponent<PlayerController>();
		if (player != null)
		{
			Shape playerShape = player.pickUpController.GetHoldedShape();
			if (playerShape != null)
			{
				Vector2Int[] shapeCoord = playerShape.GetPlacements(Vector2Int.zero);
				int score = shapeCoord.Length;
				if (CanPlaceBlock(score))
				{
					PlaceBlock(score, player);
					player.pickUpController.DropShape(true);
				}
			}
		}
	}

	private void PlayerScore(int points, PlayerController player)
	{
		EventManager.PlayerScored.Invoke(player, GameManager.Instance.playerScores[player.playerId], points);
		GivePlayerPoints(player.playerId, points);
	}
}
