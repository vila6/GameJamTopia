using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidPatrol : MonoBehaviour
{
    public GameObject skid;
    public Transform[] patrolPoint;
    private int actualPoint = 0;

    public Vector3 RequestPatrolPoint()
    {
        actualPoint = actualPoint++ % patrolPoint.Length+1;
        return patrolPoint[actualPoint-1].position;
    }
}
