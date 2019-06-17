using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPauseBehaviour : MonoBehaviour
{
    public void _OnHover()
    {
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 175);
    }

    public void _OnExitHover()
    {
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 150);
    }
}
