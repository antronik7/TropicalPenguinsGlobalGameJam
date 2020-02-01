using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisUIController : MonoBehaviour
{
    public GameObject[] Blocks;
    public int[,] BlocksPos;
    private int[] CursorPos;

    public TetrisGrid GridUI;
    //public GameObject BlockUI;
    public TetrisBlockUI BlockView;

    bool isBlockPosValid = true;

    // Start is called before the first frame update
    void Start()
    {
        CursorPos = new int[] { 0, 0 };

        //Tests
        SetGridUI(new int[,] { { 3, 0 }, { 0, 0 }, { 0 , 3 }, { 3 , 3 } });
        SetBlock(new int[,] { {0,0}, {0,1}, {1,1} }, 0);
        MoveBlock(new int[] { 0, 1 },0);
    }

    void OpenUI(int playerId, int[,] blockPos, int[,] builtPos)
    {
        SetBlock(blockPos, 0);
    }

    void SetGridUI(int[,] blockpos)
    {
        GridUI.SetGridBlocks(blockpos);
    }

    void SetBlock(int[,] blockpos, int Playerid)
    {
        BlocksPos = blockpos;
        BlockView.DrawBlock(CursorPos, BlocksPos);
        UpdateBlockPosValidity();
    }

    public void MoveBlock(int[] direction, int PlayerId)
    {
        CursorPos[0] = Mathf.Clamp(CursorPos[0] + direction[0], 0, GridUI.GridSize);
        CursorPos[1] += direction[1];
        BlockView.DrawBlock(CursorPos, BlocksPos);
        UpdateBlockPosValidity();
    }

    public bool UpdateBlockPosValidity()
    {
        bool[,] gridIsEmpty = GridUI.gridIsEmpty;
        bool oldValidity = isBlockPosValid;
        isBlockPosValid = true;
        for (int i = 0; i < BlocksPos.GetLength(0); ++i)
        {
            if(!gridIsEmpty[CursorPos[0] + BlocksPos[i,0], CursorPos[1] + BlocksPos[i, 1]])
            {
                isBlockPosValid = false;
            }
        }
        if (oldValidity != isBlockPosValid)
            BlockView.ChangeValidity(isBlockPosValid);
        return true;
    }

    public void Place(int PlayerId)
    {
        //UpdateHouse(cursor, Blockpos, PlayerId)
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
