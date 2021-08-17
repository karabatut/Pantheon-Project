using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TextController : MonoBehaviour
{
    GameObject gameManager;
    GameManager gameManagerScript;
    List<GameObject> runners;
    List<GameObject> finishers;
    public Text leaderBoard;

    // Start is called before the first frame update
    void Start()
    {
        //Gets the gamemanager instance
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = (GameManager)gameManager.GetComponent(typeof(GameManager));
    }

    // Update is called once per frame
    void Update()
    {
        leaderBoard.text = createLeaderBoard();
    }

    string createLeaderBoard()
    {
        runners = gameManagerScript.getRankings(); //Gets the rankings from GameManager class
        finishers = gameManagerScript.getFinishers(); //Gets the finishers from GameManager class

        string leaderBoardString = "";
        string nextRunner = "";

        //Loop through all of the runners.
        for (int i = 0; i < runners.Count; i++)
        {
            //If any of the runners finished the race, fix its position on the leaderboard (because finishers list doesn't get sorted constantly), and make its name green.
            if(finishers.ElementAtOrDefault(i) != null)
            {
                nextRunner = "<color=green>" + finishers[i].name.ToString() + "</color>";
            }
            //Non-finishers are sorted in GameManager constantly and written to the leaderboard here.
            else
            {
                nextRunner = runners[i].name.ToString();
            }
            leaderBoardString += (i+1) + ". " + nextRunner + "\n";
        }

        return leaderBoardString;
    }
}
