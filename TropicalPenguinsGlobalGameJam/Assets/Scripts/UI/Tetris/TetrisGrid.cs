using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TetrisGrid : MonoBehaviour
{

    public int GridSize = 4;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private GameObject gridCellTemplate;

    [SerializeField]
    private Sprite EmptySprite;

    [SerializeField]
    private Sprite BlockSprite;


    private GridLayout gridLayout;
    private Image[,] gridCells;
    public bool[,] gridHasBlock;

    // Start is called before the first frame update
    void Start()
    {
        RectTransform canvasTransform = canvas.GetComponent<RectTransform>();
        float h = canvasTransform.rect.height;
        float w = canvasTransform.rect.width;

        gridLayout = canvas.GetComponent<GridLayout>();

        gridCells = new Image[GridSize, GridSize];
        gridHasBlock = new bool[GridSize, GridSize];
        for (int i = 0; i < GridSize; ++i)
        {
            for(int j = 0; j < GridSize; ++j)
            {
                gridHasBlock[i, j] = false;
                GameObject newGridCell = Instantiate(gridCellTemplate);
                newGridCell.transform.SetParent(canvas.transform, false);
                gridCells[i, j] = newGridCell.GetComponent<Image>();
            }
        }
    }

    public void SetGrid(bool[,] house)
    {
        gridHasBlock = house;
        for (int i = 0; i < house.GetLength(0); ++i)
        {
            for (int j = house.GetLength(1) - 1; j >= 0; --j)
            {
                Image currentImg = gridCells[i,j];
                if(gridHasBlock[i,j] && currentImg.sprite == EmptySprite)
                    currentImg.sprite = BlockSprite;
                else if(currentImg.sprite == BlockSprite)
                    currentImg.sprite = EmptySprite;
            }
        }
    }

    // *****************************
    // VALIDER x,y
    // *****************************
    public void AddGridBlocks(int[] cursorPos, int[,] ShapePosArray)
    {
        for (int i = 0; i < ShapePosArray.GetLength(0); ++i)
        {
            gridHasBlock[cursorPos[1] + ShapePosArray[i, 0], cursorPos[0] + ShapePosArray[i, 1]] = true;
            Image currentImg = gridCells[cursorPos[1] + ShapePosArray[i, 0], cursorPos[0] + ShapePosArray[i, 1]];
            currentImg.sprite = BlockSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
