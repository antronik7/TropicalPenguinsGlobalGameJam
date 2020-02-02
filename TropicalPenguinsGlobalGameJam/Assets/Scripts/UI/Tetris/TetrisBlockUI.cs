using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TetrisBlockUI : MonoBehaviour
{

    public Image blockTemplate;
    private Image[] blocksArray;
    
    public Color validColor;
    public Color invalidColor;

    public Canvas BlockCanvas;

    private void Start()
    {
        validColor = new Color(0, 1, 0, 1);
        invalidColor = new Color(1, 0, 0, 1);
    }

    void InstantiateBlockImgs(int nBlocks)
    {
        blocksArray = new Image[nBlocks];
        for (int i = 0; i < nBlocks; ++i)
        {
            Image newBlock = Instantiate(blockTemplate);
            newBlock.rectTransform.SetParent(BlockCanvas.transform, false);
            blocksArray[i] = newBlock;
        }
    }

    public void DrawBlock(int[] cursorPos, int[,] blocksPos)
    {
        if (blocksArray == null)
            InstantiateBlockImgs(blocksPos.GetLength(0));
        if (blocksPos.GetLength(0) != blocksArray.Length)
        {
            Debug.Log("Images and position lists are not of same size!");
        }

        for(int i = 0; i < blocksPos.GetLength(0); ++i)
        {
            blocksArray[i].rectTransform.anchoredPosition = new Vector3(cursorPos[0] + blocksPos[i,0], 
                                                                    cursorPos[1] + blocksPos[i,1], 
                                                                    blocksArray[i].rectTransform.localPosition.z);
        }
    }

    public void ChangeValidity(bool isValid)
    {
        Color color = validColor;
        if (!isValid)
        {
            color = invalidColor;
        }
        foreach(Image block in blocksArray)
        {
            block.color = color;
        }
    }
}
