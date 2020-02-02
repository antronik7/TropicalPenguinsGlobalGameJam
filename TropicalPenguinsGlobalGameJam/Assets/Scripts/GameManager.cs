using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int scoreToWin = 50;
    public static GameManager instance = null;                //Static instance of GameManager which allows it to be accessed by any other script.
    private int[] playerScores = new int[4];

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addToPlayerScore(int playerID, int score)
    {
        playerScores[playerID] += score;
        CheckIfWinner();
    }

    private int CheckIfWinner()
    {
        for (int i = 0; i < playerScores.Length; ++i)
        {
            if (playerScores[i] >= scoreToWin)
                return i;
        }

        return -1;
    }
}
