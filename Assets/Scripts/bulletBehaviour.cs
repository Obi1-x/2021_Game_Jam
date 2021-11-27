using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1); //Destruction time should be a function of bullet speed.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
