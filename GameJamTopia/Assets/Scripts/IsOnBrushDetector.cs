using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsOnBrushDetector : MonoBehaviour
{
    void OnTriggerEnter()
    {
        PlayerController.instance.SetIsOnBrush(true);
    }

    void OnTriggerExit()
    {
        PlayerController.instance.SetIsOnBrush(false);
    }
}
