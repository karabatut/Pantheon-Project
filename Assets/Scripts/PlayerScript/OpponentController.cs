using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentController : MonoBehaviour
{

    Rigidbody rb;
    float speed = 10f;
    MovementEnum movementState;
    bool undecided;
    Vector3 initialPos;
    bool finished = false;
    Vector3 finalPos;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Opponent";
        undecided = true; //For avoiding obstacles. More info below.

        rb = GetComponent<Rigidbody>();

        initialPos = transform.position; //Initial position is recorded for the opponent's death.

        movementState = MovementEnum.FORWARD;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(movementState != MovementEnum.FINISHED)
        {
            //If the opponent goes outside of the platform, it is teleported to its initial position.
            if (rb.position.z < -7.5f || rb.position.z > 7.5f)
            {
                rb.MovePosition(initialPos);
            }

            //  HITREG
            // 3 Rays are emitted from the opponent. One directly ahead, two with 20 degrees of angle.
            // Diagram of the rays:
            //  \ | /
            //   \|/
            //    O
            //
            // Based on the hit state of these rays, opponent's direction is decided.
            RaycastHit hit;

            float hitRange = 5f;

            bool frontHit = false;
            bool leftHit = false;
            bool rightHit = false;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, hitRange))
            {
                frontHit = (hit.transform.tag == "StaticObstacle") || (hit.transform.tag == "HorizontalObstacle");
            }
            if (Physics.Raycast(transform.position, transform.TransformDirection(Quaternion.AngleAxis(-20, Vector3.up) * Vector3.forward), out hit, hitRange))
            {
                leftHit = (hit.transform.tag == "StaticObstacle") || (hit.transform.tag == "HorizontalObstacle");
            }
            if (Physics.Raycast(transform.position, transform.TransformDirection(Quaternion.AngleAxis(20, Vector3.up) * Vector3.forward), out hit, hitRange))
            {
                rightHit = (hit.transform.tag == "StaticObstacle") || (hit.transform.tag == "HorizontalObstacle");
            }

            // If only the front ray hits obstacle, one direction is selected randomly and the opponent sticks by that decision using the undecided boolean.
            // As long as only the front ray hits the obstacle, it goes to the first selected direction.
            if (frontHit)
            {
                if (undecided)
                {
                    movementState = Random.Range(0, 2) == 1 ? MovementEnum.GO_RIGHT : MovementEnum.GO_LEFT;
                    undecided = false;
                }

            }
            // Decide the direction based on the side hits.
            else if ((frontHit && leftHit) || leftHit)
            {
                movementState = MovementEnum.GO_RIGHT;
                undecided = true;
            }
            else if ((frontHit && rightHit) || rightHit)
            {
                movementState = MovementEnum.GO_LEFT;
                undecided = true;
            }
            else
            {
                movementState = MovementEnum.FORWARD;
                undecided = true;
            }

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
            rb.MovePosition(rb.position + direction * Time.deltaTime * speed); //Move the opponent in the desired direction
        }
        else
        {
            if (!finished) //As soon as the opponent finishes the race, record its position.
            {
                finalPos = transform.position;
                finished = true;
            }
            gameObject.GetComponent<Animator>().enabled = false; //Disable the animation.
            transform.position = finalPos; //Move opponent to the finished position constantly to prevent possible drifting.
        }
        
    }

    //If the opponent hits an object with "StaticObstacle" or "HorizontalObstacle" tags, teleport it to the starting position.
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
