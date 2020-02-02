﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
	private const int houseDimensions = 4;
	public const int nbPlayers = 4;
	private int[] playersBlocksPlaced;
	public const int maxCubes = 16;
	protected int nbCubes = 0;

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
		}
	}

	private bool CanPlaceBlock(int size)
	{
		if ( (nbCubes + size) > maxCubes)
		{
			return false;
		}
		return true;
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
		EventManager.PlayerScored.Invoke(player, playersBlocksPlaced[player.playerId]);

		//*************
		// TODO play demolition / new house animation
		//*************

	}

	private void InitializeConstants()
	{
		nbCubes = 0;
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
				}
			}
		}
	}

	private void PlayerScore(int points, PlayerController player)
	{
		EventManager.PlayerScored.Invoke(player, points);
		GivePlayerPoints(player.playerId, points);
	}
}
