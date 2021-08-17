using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalObsController : MonoBehaviour
{
    int direction;
    float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        //Assign a initial direction at random.
        direction = Random.Range(0, 2) == 1 ? 1 : -1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Change direction if the obstacle reaches the end of the platform horizontally.
        if(Mathf.Abs(transform.position.z) >= 7)
        {
            direction = direction * -1; 
        }

        Vector3 position = new Vector3(0f, 0f, 1f * direction);
        transform.Translate(position * Time.deltaTime * speed);
    }
}
