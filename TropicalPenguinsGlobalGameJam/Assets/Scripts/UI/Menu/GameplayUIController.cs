using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayUIController : MonoBehaviour
{

    public TextMeshProUGUI ScoreP1;
    public TextMeshProUGUI ScoreP2;
    public TextMeshProUGUI ScoreP3;
    public TextMeshProUGUI ScoreP4;

    private void Start()
    {
        //UpdateScore(1, 1);
        //UpdateScore(2, 4);
        //UpdateScore(3, 9);
        //UpdateScore(4, 16);
    }

    public void OnEndGame()
    {
        MenuSpawnerManager menuSpawnerManager = MenuSpawnerManager.Instance;
        menuSpawnerManager.SwitchUI(menuSpawnerManager.EndUI);
    }

    public void UpdateScore(int playerID, int Score)
    {
        switch (playerID)
        {
            case 1:
                ScoreP1.SetText(Score.ToString());
                break;
            case 2:
                ScoreP2.SetText(Score.ToString());
                break;
            case 3:
                ScoreP3.SetText(Score.ToString());
                break;
            case 4:
                ScoreP4.SetText(Score.ToString());
                break;
            default: break;
        }
    }
}
