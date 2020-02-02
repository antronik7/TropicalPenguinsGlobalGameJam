﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisUIController : MonoBehaviour
{
    public int[,] ShapePosArray;
    private int[] CursorPos;

    public TetrisGrid GridUI;
    //public GameObject BlockUI;
    public TetrisBlockUI BlockView;

    bool isBlockPosValid;
    bool isSetup;

    // Start is called before the first frame update
    void Start()
    {
        CursorPos = new int[] { 0, 0 };

        //Tests
        /*SetGridUI(new bool[,] { { true, false, false, true } ,
                                { false, false, false, false } ,
                                { true, false, false, false } ,
                                { true, false, false, true } });
        SetShape(new int[,] { {0,0}, {0,1}, {1,1} }, 0);
        MoveBlock(new int[] { 1, 0 },0);
        Place(0);*/
        //RotateBlock(true,0);
    }

    public void OpenUI(int playerId, int[,] shapePosArray, bool[,] houseBlockGrid)
    {
        SetGridUI(houseBlockGrid);
        SetShape(shapePosArray, 0);
        isSetup = true;
    }

    void SetGridUI(bool[,] houseBlockGrid)
    {
        GridUI.SetGrid(houseBlockGrid);
    }

    void SetShape(int[,] blockpos, int Playerid)
    {
        ShapePosArray = blockpos;
        BlockView.DrawBlock(CursorPos, ShapePosArray);
        UpdateBlockPosValidity();
    }

    public void MoveBlock(int[] direction, int PlayerId)
    {
        CursorPos[0] = Mathf.Clamp(CursorPos[0] + direction[0], 0, GridUI.GridSize);
        CursorPos[1] = Mathf.Clamp(CursorPos[1] + direction[1], 0, GridUI.GridSize);
        BlockView.DrawBlock(CursorPos, ShapePosArray);
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

        for(int i = 0; i < ShapePosArray.GetLength(0); ++i)
        {
            int[] vec = new int[] { ShapePosArray[i, 0], ShapePosArray[i, 1] };
            vec = RotateIntVector(vec, rotMat);
            ShapePosArray[i, 0] = vec[0];
            ShapePosArray[i, 1] = vec[1];
        }
        BlockView.DrawBlock(CursorPos, ShapePosArray);
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
        bool[,] gridHasBlock = GridUI.gridHasBlock;
        bool oldValidity = isBlockPosValid;
        isBlockPosValid = true;
        for (int i = 0; i < ShapePosArray.GetLength(0) && isBlockPosValid; ++i)
        {
            int posX = CursorPos[1] + ShapePosArray[i, 1];
            int posY = CursorPos[0] + ShapePosArray[i, 0];
            if (posX < 0 || posY < 0 || posX > GridUI.GridSize || posY > GridUI.GridSize)
            {
                isBlockPosValid = false;
            }
            else if(gridHasBlock[posY, posX])
            {
                isBlockPosValid = false;
            }
        }
        if (!isSetup || oldValidity != isBlockPosValid)
            BlockView.ChangeValidity(isBlockPosValid);
        return isBlockPosValid;
    }

    public void Place(int PlayerId)
    {
        if (isBlockPosValid)
        {
            GridUI.AddGridBlocks(CursorPos, ShapePosArray);
            //UpdateHouse(CursorPos, ShapePosArray, PlayerId)
            Close();
        }
        else
        {
            //Play invalid placement sound
        }
    }

    public void Close()
    {
        BlockView.Close();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
