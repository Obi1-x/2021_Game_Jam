using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SourcecodeBehaviour : MonoBehaviour
{
    private bool triggered, isBug, isLastByte, toDestroy;

    public bool IsBug { get => isBug; set => isBug = value; }
    public bool IsLastByte { get => isLastByte; set => isLastByte = value; }

    void Start()
    {
        triggered = false;
        toDestroy = false;
    }

    void Update()
    {
        if (Operation.aboutToChange && !triggered && !toDestroy)
        {
            float xPos = gameObject.GetComponent<RectTransform>().localPosition.x;
            gameObject.GetComponent<RectTransform>().localPosition = new Vector3(xPos - 64, -1.1f, 0); //Debugger posistion = -232
            triggered = true;
        }
        else if (!Operation.aboutToChange && triggered) triggered = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //toDestroy = true;  //Try making the bullet faster or so.
        if (col.gameObject.tag == "bullet") Destroy(gameObject, 0.3f);
    }
}
