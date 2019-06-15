using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsOnBrushDetector : MonoBehaviour
{
    public bool rightSide;

    void Update()
    {
        Debug.DrawLine(this.transform.position, this.transform.position + Vector3.down * 0.4f, Color.blue);
        Debug.DrawLine(this.transform.position - new Vector3(0.25f, 0, 0), this.transform.position - new Vector3(0.25f, 0, 0) + Vector3.down * 0.4f, Color.blue);
        Debug.DrawLine(this.transform.position + new Vector3(0.25f, 0, 0), this.transform.position + new Vector3(0.25f, 0, 0) + Vector3.down * 0.4f, Color.blue);
        if(Physics.Raycast(this.transform.position - new Vector3(0.25f, 0, 0), Vector3.down, 0.2f) || Physics.Raycast(this.transform.position + new Vector3(0.25f, 0, 0), Vector3.down, 0.2f))
        {
            if(rightSide)
                PlayerController.instance.SetIsOnBrushRight(true);
            else
                PlayerController.instance.SetIsOnBrushLeft(true);
        }
        else
        {
            if(rightSide)
                PlayerController.instance.SetIsOnBrushRight(false);
            else
                PlayerController.instance.SetIsOnBrushLeft(false);
        }
    }
}
