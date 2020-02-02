using System.Collections;
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

	private PlayerController currentPlayer;

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
        //Place(0);
        RotateBlock(true,0);*/
    }

    /**
     * <summary>Opens the Tetris UI</summary>
     * <param name="playerId"> Id of the player who opens the UI</param>
     * <param name="shapePosArray"> Array of the positions of the blocks to be placed relative to the pivot</param>
     * <param name="houseBlockGrid"> Array of bool, describing where the house blocks are</param>
     **/
    public void OpenUI(PlayerController player, int[,] shapePosArray, bool[,] houseBlockGrid)
    {
        gameObject.SetActive(true);
        SetGridUI(houseBlockGrid);
        SetShape(shapePosArray, 0);
        isSetup = true;

		currentPlayer = player;
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

    public void MoveBlock(Vector2 direction, int PlayerId)
    {
        CursorPos[0] = Mathf.Clamp(CursorPos[0] + (int)direction.x, 0, GridUI.GridSize);
        CursorPos[1] = Mathf.Clamp(CursorPos[1] + (int)direction.y, 0, GridUI.GridSize);
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
            int posX = CursorPos[0] + ShapePosArray[i, 1];
            int posY = CursorPos[1] + ShapePosArray[i, 0];
            if (posX < 0 || posY < 0 || posX >= GridUI.GridSize || posY >= GridUI.GridSize)
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

    public void Place(int playerId)
    {
        if (isBlockPosValid)
        {

            transform.parent.GetComponent<HouseManager>().PlaceBlock(playerId, CursorPos, ShapePosArray);
            GridUI.AddGridBlocks(CursorPos, ShapePosArray);

            Close();
        }
        else
        {
            //Play invalid placement sound
        }
    }

    /**
     * <summary>Closes the Tetris UI</summary>
     **/
    public void Close(bool playerForceClose = false)
    {
        BlockView.Close();
        gameObject.SetActive(false);
        CursorPos = new int[] { 0, 0 };

        if (currentPlayer == null || playerForceClose)
			return;

		currentPlayer.CloseTetrisUI();
		currentPlayer = null;
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
