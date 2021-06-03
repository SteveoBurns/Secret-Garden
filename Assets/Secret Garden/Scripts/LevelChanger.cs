using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChanger : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "EndLevel1" || collision.gameObject.tag == "EndLevel2")
        {
            UIManager.loadScene++;
            UIManager.instance.Continue();
        }

    }
}
