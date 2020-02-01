using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisGrid : MonoBehaviour
{

    public int GridSize = 4;

    [SerializeField]
    private Canvas canvas;
    private GridLayout gridUI;

    [SerializeField]
    private GameObject gridCell;

    // Start is called before the first frame update
    void Start()
    {
        RectTransform canvasTransform = canvas.GetComponent<RectTransform>();
        float h = canvasTransform.rect.height;
        float w = canvasTransform.rect.width;

        gridUI = canvas.GetComponent<GridLayout>();
        for (int i = 0; i < GridSize * GridSize; ++i)
        {
            GameObject newGridCell = Instantiate(gridCell);
            newGridCell.transform.SetParent(canvas.transform, false);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
