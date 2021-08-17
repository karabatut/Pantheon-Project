using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    GameObject gameManager;
    GameManager gameManagerScript;
    bool devMode = false;  //If the input mode is in development mode.
    Vector2 beginningPos;
    Vector2 currentPos;
    bool firstPressed = false;

    void Start()
    {
        //Gets the gamemanager instance
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = (GameManager)gameManager.GetComponent(typeof(GameManager));
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //If the input is interpreted as a direction, the movement state of the player is changed through the GameManager script.


        //if in development mode, the input is taken from "a" and "d" keys.
        if (devMode)
        {
            //Change state based on the pressed key
            if (Input.GetKey("a"))
            {
                gameManagerScript.setMovementState(MovementEnum.GO_LEFT);
            }
            else if (Input.GetKey("d"))
            {
                gameManagerScript.setMovementState(MovementEnum.GO_RIGHT);
            }
            else
            {
                gameManagerScript.setMovementState(MovementEnum.FORWARD);
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                //When the mouse is clicked for the first time, the swipe action's beginning position is recorded.
                if (!firstPressed)
                {
                    beginningPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    firstPressed = true;
                }
             
                //After the swipe action starts, current position of the mouse is constantly taken.
                currentPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                //Deciding the direction the player should be going based on the input.
                if (currentPos.x < beginningPos.x)
                {
                    gameManagerScript.setMovementState(MovementEnum.GO_LEFT); 

                }
                else if(currentPos.x > beginningPos.x)
                {
                    gameManagerScript.setMovementState(MovementEnum.GO_RIGHT);
                }
                
            }
            else
            {
                //If no swipe action, go forward.
                gameManagerScript.setMovementState(MovementEnum.FORWARD);
                firstPressed = false;
            }
        }
       
    }
}
