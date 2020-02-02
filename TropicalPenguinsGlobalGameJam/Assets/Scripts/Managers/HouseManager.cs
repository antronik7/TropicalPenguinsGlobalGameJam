using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
	// The referential for stocking the matrix is line storage, see below
	// [0,1,2]
	// [3,4,5] -> { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } }
	// [6,7,8]
	public GameObject myUITetris;
	private const int houseDimensions = 4;
	private bool[,] houseGridBool;
	public const int nbPlayers = 4;
	private int[] playersBlocksPlaced;
	private int minNbBlocksInit = 3;
	private int maxNbBlocksInit = 7;
	private Vector3 housePosition;
	private Shape[] childCubes;
	public int seed;

	// Start is called before the first frame update
	void Start()
	{
		InitializeConstants();
		InitializeHouseGrid();
		AddBlockToHouse(new int[] { 0, 0 }, new int[,] { { 0, 0 } });
	}

	private void InitializeHouseGrid()
	{
		HideHouseGrid();
		houseGridBool = new bool[houseDimensions, houseDimensions];
		playersBlocksPlaced = new int[nbPlayers];
		System.Random random = new System.Random(seed + (int)Time.time);

		int blocksToInit = random.Next(minNbBlocksInit, maxNbBlocksInit + 1);
		int nbBlocksPlaced = 0;
		int row = 0;
		int col = 0;

		while (nbBlocksPlaced < blocksToInit)
		{
			row = random.Next(0, houseDimensions);
			col = random.Next(0, houseDimensions);
			if (houseGridBool[row, col] != true)
			{
				houseGridBool[row, col] = true;
				nbBlocksPlaced++;
				//magie
				int childToActivate = (houseDimensions * row) + col;
				childCubes[childToActivate].transform.gameObject.SetActive(true);
			}
		}
	}

	public bool PlaceBlock(int player, int[] cursor, int[,] blockGrid)
	{
		bool returnValue = false;

		if (CanPlaceBlock(cursor, blockGrid))
		{
			returnValue = true;
			AddBlockToHouse(cursor, blockGrid);
			GivePlayerPoints(player, blockGrid);

			if (IsHouseComplete())
			{
				HouseComplete(player);
			}
		}

		return returnValue;
	}

	private bool CanPlaceBlock(int[] cursor, int[,] grid)
	{
		for (int i = 0; i < grid.GetLength(0); i++)
		{
			//if collision
			// invert the grid values because of line storage
			if (houseGridBool[cursor[1] + grid[i, 1], cursor[0] + grid[i, 0]] == true)
			{
				return false;
			}
		}
		return true;
	}

	// loop grid, if a 0 is left its not complete
	public bool IsHouseComplete()
	{
		for (int row = 0; row < houseGridBool.GetLength(0); row++)
		{
			for (int col = 0; col < houseGridBool.GetLength(1); col++)
			{
				if (houseGridBool[row, col] == false)
				{
					return false;
				}
			}
		}
		return true;
	}

	private void GivePlayerPoints(int player, int[,] blockGrid)
	{
		//points seulement selon grosseur du block (1 pour 1)
		playersBlocksPlaced[player] += blockGrid.Length;
	}

	private void AddBlockToHouse(int[] cursor, int[,] grid)
	{
		for (int i = 0; i < grid.GetLength(0); i++)
		{
			int row = cursor[1] + grid[i, 0];
			int col = cursor[0] + grid[i, 1];
			houseGridBool[row, col] = true;
			int childToActivate = (houseDimensions * row) + col;
			childCubes[childToActivate].transform.gameObject.SetActive(true);
		}
	}


	public ShapeType AvailableShapes()
	{
		Array values = Enum.GetValues(typeof(ShapeType));
		System.Random random = new System.Random();
		ShapeType randomShape = (ShapeType)values.GetValue(random.Next(values.Length));
		return randomShape;
	}

	private void HouseComplete(int player)
	{
		// double the points for who finished it
		playersBlocksPlaced[player] *= 2;

		for (int i = 0; i < playersBlocksPlaced.Length; i++)
		{
			GameManager.instance.addToPlayerScore(i, playersBlocksPlaced[i]);
		}

		//*************
		// TODO play demolition / new house animation
		//*************

		InitializeHouseGrid();
	}


	private void HideHouseGrid()
	{
		for (int i = 0; i < childCubes.Length; i++)
		{
			childCubes[i].transform.gameObject.SetActive(false);
		}
	}

	private void InitializeConstants()
	{
		childCubes = GetComponentsInChildren<Shape>();
		housePosition = gameObject.transform.position;
	}

	private void OnTriggerEnter(Collider other)
	{
		//PlayerController player = other.GetComponent<PlayerController>();
		PlayerController player = other.transform.root.GetComponent<PlayerController>();
		if (player != null)
		{
			Shape playerShape = player.pickUpController.GetHoldedShape();
			if (playerShape != null)
			{
				Vector2Int[] shapeCoord = playerShape.GetPlacements(Vector2Int.zero);

				int[,] sc = new int[shapeCoord.Length, 2];
				for (int i = 0; i < shapeCoord.Length; ++i )
				{
					Vector2Int vi = shapeCoord[i];
					sc[i, 0] = vi.y;
					sc[i, 1] = vi.x;
				}
				TetrisUIController tetrisUI = myUITetris.GetComponent<TetrisUIController>();
				tetrisUI.OpenUI(player, sc, houseGridBool);

				player.OpenTetrisUI(tetrisUI);
			}
		}
	}
}
