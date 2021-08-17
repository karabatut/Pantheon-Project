using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PercentageTextController : MonoBehaviour
{
    GameObject gameManager;
    GameManager gameManagerScript;
    GameObject endWall;
    DrawManager drawManagerScript;
    public GameObject player;
    public Text percentageText;
    List<GameObject> finishers;
    bool finished = false;

    // Start is called before the first frame update
    void Start()
    {
        //Gets the gamemanager instance
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = (GameManager)gameManager.GetComponent(typeof(GameManager));

        percentageText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        finished = gameManagerScript.isFinished();
        //If finished start showing the painted percentage.
        if (finished)
        {
            percentageText.text = calculatePercentage();
        }

    }

    //Gets the calculated paint percentage from the GameManager class.
    string calculatePercentage()
    {

        float percentage = gameManagerScript.getPaintedPercentage();
        return "Painted " + percentage + "%";
    }
}
