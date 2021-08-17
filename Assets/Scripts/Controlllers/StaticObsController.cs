using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObsController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //For some reason collider didn't work after being instantiated. Moving the object just a little bit solves the problem.
        Vector3 posReset = this.transform.localPosition;
        posReset.y -= 0.04f;
        this.transform.localPosition = posReset;
    }

}
