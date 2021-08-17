using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;
    Vector3 offset;
    bool finished;

    GameObject gameManager;
    GameManager gameManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        //Offets the camera behind the player
        player = GameObject.FindGameObjectWithTag("Player");
        offset = new Vector3(10f, 5f, 0f);
        transform.Rotate(new Vector3(10f, -90f, 0f));

        //Gets the gamemanager instance
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = (GameManager)gameManager.GetComponent(typeof(GameManager));
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if the player finished the race.
        finished = gameManagerScript.isFinished();

        //In case the player object isn't created before this script's start function.
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        //If the player hasn't finished the race, follow him with the initial offset.
        else if(!finished)
        {
            transform.position = player.transform.position + offset;
        }
        //If the player finished the race, smoothly center the end wall.
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(-130f, transform.position.y, 0f), Time.deltaTime * 2f);
        }
    }
}
