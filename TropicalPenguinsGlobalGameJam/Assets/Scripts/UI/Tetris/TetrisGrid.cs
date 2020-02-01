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
    public bool[,] gridIsEmpty;

    // Start is called before the first frame update
    void Start()
    {
        RectTransform canvasTransform = canvas.GetComponent<RectTransform>();
        float h = canvasTransform.rect.height;
        float w = canvasTransform.rect.width;

        gridLayout = canvas.GetComponent<GridLayout>();

        gridCells = new Image[GridSize, GridSize];
        gridIsEmpty = new bool[GridSize, GridSize];
        for (int i = 0; i < GridSize; ++i)
        {
            for(int j = GridSize - 1; j >= 0; --j)
            {
                gridIsEmpty[i, j] = true;
                GameObject newGridCell = Instantiate(gridCellTemplate);
                newGridCell.transform.SetParent(canvas.transform, false);
                gridCells[i, j] = newGridCell.GetComponent<Image>();
            }
        }
    }

    public void SetGridBlocks(int[,] builtPos)
    {
        for (int i = 0; i < builtPos.GetLength(0); ++i)
        {
            gridIsEmpty[builtPos[i, 0], builtPos[i, 1]] = false;
            Image currentImg = gridCells[builtPos[i, 0], builtPos[i, 1]];
            currentImg.sprite = BlockSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
