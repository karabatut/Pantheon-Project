using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameObject gameManager;
    GameManager gameManagerScript;
    Rigidbody rb;
    float speed = 10f;
    Vector3 initialPos;
    MovementEnum movementState;
    bool finished = false;
    Vector3 finalPos;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Player";

        //Gets the gamemanager instance
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = (GameManager)gameManager.GetComponent(typeof(GameManager));


        rb = GetComponent<Rigidbody>();

        initialPos = transform.position; //Initial position is recorded for the player's death.

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //If movement state is not finished, the player can still move.
        if (movementState != MovementEnum.FINISHED)
        {
            movementState = gameManagerScript.getMovementState();
            Vector3 direction = new Vector3(-1f, 0f, 0f); //Go forward constantly.

            if (movementState == MovementEnum.GO_LEFT)
            {
                if (rb.position.z > -7.5f) // Don't go outside the platform
                {
                    direction = direction + new Vector3(0f, 0f, -1f);
                }
            }
            else if (movementState == MovementEnum.GO_RIGHT)
            {
                if (rb.position.z < 7.5f) // Don't go outside the platform
                {
                    direction = direction + new Vector3(0f, 0f, 1f);
                }

            }
            direction = direction.normalized;
            rb.MovePosition(rb.position + direction * Time.deltaTime * speed); //Move the player in the desired direction
        }
        else
        {
            if (!finished) //As soon as the player finishes the race, record its position.
            {
                finalPos = transform.position;
                finished = true;
            }
            gameObject.GetComponent<Animator>().enabled = false; //Disable the animation.
            transform.position = finalPos; //Move player to the finished position constantly to prevent possible drifting.
        }

    }

    //If the player hits an object with "StaticObstacle" or "HorizontalObstacle" tags, teleport it to the starting position.
    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "StaticObstacle" || tag == "HorizontalObstacle")
        {
            rb.MovePosition(initialPos);
        }
    }

    //Used by the GameManager to set the movement state.
    public void setMovementState(MovementEnum state)
    {
        movementState = state;
    }
}
