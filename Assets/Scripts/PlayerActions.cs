using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{

    //  bullet speed, X should be moved to player controls (Power ups)
    //  vertical container width, X should be part of power ups.

    public Transform bulletCatridge;
    private float bulletSpeed;
    private bool firing;

    void Start()
    {
        firing = false;
        bulletSpeed = -15f; //WIll modify this.
    }

    // Update is called once per frame
    void Update()
    {
        if (!Operation.demoPause && Input.GetMouseButton(0) && !firing)
        {
            Transform aBullet = Instantiate(bulletCatridge, gameObject.transform.parent);
            aBullet.GetComponent<Rigidbody2D>().gravityScale = bulletSpeed;
            firing = true;
        }
        else if (!Input.GetMouseButton(0) && firing) firing = false;
    }
}
