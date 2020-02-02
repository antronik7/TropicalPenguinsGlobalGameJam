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

	// Start is called before the first frame update
	void Start()
	{
		bool test = true;
		InitializeConstants();
		InitializeHouseGrid();
		//int[] cursor = { 1, 1 }; c
		//int[,] blockGrid = { { 0, 0 }, { -1, 0 }, { -1, -1 } };
		//PlaceBlock(1, cursor, blockGrid);
		//ShapeType test = AvailableShapes();
		//playersBlocksPlaced[0] = 4;
		//HouseComplete(0);
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void InitializeHouseGrid()
	{
		HideHouseGrid();
		houseGridBool = new bool[houseDimensions, houseDimensions];
		playersBlocksPlaced = new int[nbPlayers];
		System.Random random = new System.Random((int)(housePosition.x * housePosition.z));
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
				childCubes[(houseDimensions*row)+col].transform.gameObject.SetActive(true);
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
			if (houseGridBool[cursor[0] + grid[i, 1], cursor[1] + grid[i, 0]] == true)
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
			//int x = cursor[0] + grid[i, 1];
			//int y = cursor[1] + grid[i, 0];

			// invert the grid values because of line storage
			houseGridBool[cursor[0] + grid[i, 1], cursor[1] + grid[i, 0]] = true;
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

	public void OpenUI(int player, int[] cursor, Shape shape)
	{
		int[,] shapeCoord = Shape.whatever();
		myUITetris.GetComponent<TetrisUIController>().OpenUI(player, shapeCoord, houseGridBool);
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

	//private int[,] GetBlocks()
	//{
	//	//List<>
	//}
}
