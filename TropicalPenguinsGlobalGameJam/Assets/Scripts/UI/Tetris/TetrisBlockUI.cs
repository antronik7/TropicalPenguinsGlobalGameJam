using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TetrisBlockUI : MonoBehaviour
{

    public Image blockTemplate;
    private Image[] blocks;
    
    public Color validColor;
    public Color invalidColor;

    public Canvas BlockCanvas;

    void InstantiateBlockImgs(int nBlocks)
    {
        blocks = new Image[nBlocks];
        for (int i = 0; i < nBlocks; ++i)
        {
            Image newBlock = Instantiate(blockTemplate);
            newBlock.rectTransform.SetParent(BlockCanvas.transform, false);
            blocks[i] = newBlock;
        }
    }

    public void DrawBlock(int[] cursorPos, int[,] blocksPos)
    {
        if (blocks == null)
            InstantiateBlockImgs(blocksPos.GetLength(0));
        if (blocksPos.GetLength(0) != blocks.Length)
        {
            Debug.Log("Images and position lists are not of same size!");
        }

        for(int i = 0; i < blocksPos.GetLength(0); ++i)
        {
            blocks[i].rectTransform.anchoredPosition = new Vector3(cursorPos[0] + blocksPos[i,0], 
                                                                    cursorPos[1] + blocksPos[i,1], 
                                                                    blocks[i].rectTransform.localPosition.z);
        }
    }

    public void ChangeValidity(bool isValid)
    {
        Color color = validColor;
        if (!isValid)
        {
            color = invalidColor;
        }
        foreach(Image block in blocks)
        {
            block.color = color;
        }
    }
}
