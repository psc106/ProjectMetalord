
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PulleyButton : MonoBehaviour
{
    public Pulley pulley;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            pulley.isActivated = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            pulley.isActivated = false;
        }
    }

}
