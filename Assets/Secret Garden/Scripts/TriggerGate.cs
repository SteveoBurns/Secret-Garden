using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGate : MonoBehaviour
{
    private bool isOpen;
    [SerializeField]
    private GameObject openGate;
    [SerializeField]
    private GameObject closegate;

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        closegate.SetActive(true);
        openGate.SetActive(false);
        isOpen = false;
        Debug.Log("set things");

    }




    // OnCollisionEnter2D is called when this collider2D/rigidbody2D has begun touching another rigidbody2D/collider2D (2D physics only)
    private void OnTriggerEnter2D(Collider2D _collider)
    {
        Debug.Log("has touched");
        if(isOpen == false)
        {

            if (_collider.gameObject.tag == "Player" || _collider.gameObject.tag == "Box")
            {
                isOpen = true;
                closegate.SetActive(false);
                openGate.SetActive(true);
                Debug.Log("its happening"); 
            }
        }
        else
        {
            if (_collider.gameObject.tag == "Player" || _collider.gameObject.tag == "Box")
            {
                isOpen = false;
                closegate.SetActive(true);
                openGate.SetActive(false);
            }
        }
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
