using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    // [0,0,1]
    // [0,0,1]
    // [0,0,0]
    private int[,] houseGrid = { { 0, 0, 1 }, { 0, 0, 1 }, { 0, 0, 0 } };
    private const int houseDimensions = 3;
    //private int[,] houseGrid;
    private int[] playerPoints = { 0, 0, 0, 0 };

    public enum Shapes
    {
        Zigzag, // ^>^>
        ShortL, // ShortLine + 1 block on the side on one end
        LongL, // LongLine + 1 block on the side at the end
        ShortLine, // 2 blocks
        LongLine, // 3 blocks
        Block, // a single block (1)
        Square, // 4 blocks
        T // Long Line + 1 block on the middle
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeHouseGrid();
        int[] cursor = { 1, 1 };
        int[,] blockGrid = { { 0, 0 }, { -1, 0 }, { -1, -1 } };
        PlaceBlock(1, cursor, blockGrid);
        Shapes test = AvailableShapes();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool PlaceBlock(int player, int[] cursor, int[,] blockGrid)
    {
        bool returnValue = false;

        if (CanPlaceBlock(cursor, blockGrid))
        {
            returnValue = true;
            GivePlayerPoints(player, blockGrid);
            AddBlockToHouse(cursor, blockGrid);
        }

        return returnValue;
    }

    private bool CanPlaceBlock(int[] cursor, int[,] grid)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            //if collision
            if (houseGrid[ cursor[0] + grid[i, 0], cursor[1] + grid[i, 1] ] == 1)
            {
                return false;
            }

        }
        return true;
    }

    // loop grid, if a 0 is left its not complete
    public bool IsHouseComplete()
    {
        for (int i = 0; i < houseGrid.GetLength(0); i++)
        {
            for (int j = 0; j < houseGrid.GetLength(1); j++)
            {
                if (houseGrid[i, j] == 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void GivePlayerPoints(int player, int[,] blockGrid)
    {
        if (IsHouseComplete())
        {
            playerPoints[player] += (blockGrid.Length) * 2;
        }
        else
        {
            playerPoints[player] += blockGrid.Length;
        }
    }

    private void AddBlockToHouse(int[] cursor, int[,] grid)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            int x = cursor[0] + grid[i, 0];
            int y = cursor[1] + grid[i, 1];
            houseGrid[cursor[0] + grid[i, 0], cursor[1] + grid[i, 1]] = 1;
        }
    }

    private void InitializeHouseGrid()
    {

    }

    public Shapes AvailableShapes()
    {
        Array values = Enum.GetValues(typeof(Shapes));
        System.Random random = new System.Random();
        Shapes randomShape = (Shapes)values.GetValue(random.Next(values.Length));
        return randomShape;
    }

}
