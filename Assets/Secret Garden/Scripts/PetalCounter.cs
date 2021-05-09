using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetalCounter : MonoBehaviour
{
    public void OncollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
