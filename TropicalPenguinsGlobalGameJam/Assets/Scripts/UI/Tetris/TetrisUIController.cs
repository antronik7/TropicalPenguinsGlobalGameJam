using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisUIController : MonoBehaviour
{
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
        //RotateBlock(true,0);
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

    public void RotateBlock(bool ClockWise, int PlayerId)
    {
        int[,] rotMat;
        if (ClockWise)
        {
            //Clockwise
            rotMat = new int[,] { { 0, -1 }, { 1, 0 } };
        }
        else
        {
            //AntiClockwise
            rotMat = new int[,] { { 0, 1 }, { -1, 0 } };
        }

        for(int i = 0; i < BlocksPos.GetLength(0); ++i)
        {
            int[] vec = new int[] { BlocksPos[i, 0], BlocksPos[i, 1] };
            vec = RotateIntVector(vec, rotMat);
            BlocksPos[i, 0] = vec[0];
            BlocksPos[i, 1] = vec[1];
        }
        BlockView.DrawBlock(CursorPos, BlocksPos);
        UpdateBlockPosValidity();
    }

    private int[] RotateIntVector(int[] vec, int[,] mat)
    {
        int[] resVec = new int[2];
        resVec[0] = vec[0] * mat[0, 0] + vec[1] * mat[0, 1];
        resVec[1] = vec[0] * mat[1, 0] + vec[1] * mat[1, 1];
        return resVec;
    }

    public bool UpdateBlockPosValidity()
    {
        bool[,] gridIsEmpty = GridUI.gridIsEmpty;
        bool oldValidity = isBlockPosValid;
        isBlockPosValid = true;
        for (int i = 0; i < BlocksPos.GetLength(0) && isBlockPosValid; ++i)
        {
            int posX = CursorPos[0] + BlocksPos[i, 0];
            int posY = CursorPos[1] + BlocksPos[i, 1];
            if (posX < 0 || posY < 0 || posX > GridUI.GridSize || posY > GridUI.GridSize)
            {
                isBlockPosValid = false;
            }else if(!gridIsEmpty[posX, posY])
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
