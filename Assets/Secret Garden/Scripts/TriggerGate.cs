using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGate : MonoBehaviour
{
    private bool isOpen;
    [Header("Gates")]
    [SerializeField]
    private GameObject openGate;
    [SerializeField]
    private GameObject closegate;

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        #region Set Gates
        closegate.SetActive(true);
        openGate.SetActive(false);
        isOpen = false;
        Debug.Log("set Gates");
        #endregion

    }




    // OnCollisionEnter2D is called when this collider2D/rigidbody2D has begun touching another rigidbody2D/collider2D (2D physics only)
    /// <summary>
    /// When the Player or a Box is touching the trigger the Gate Opens.
    /// </summary>
    /// <param name="_collider">Player or Box touching trigger</param>
    private void OnTriggerStay2D(Collider2D _collider)
    {
        Debug.Log("has touched");
        if(isOpen == false)
        {

            if (_collider.gameObject.tag == "Player" || _collider.gameObject.tag == "Box")
            {
                isOpen = true;
                closegate.SetActive(false);
                openGate.SetActive(true);
                Debug.Log("its open"); 
            }
        }
        
    }


    /// <summary>
    /// When the player or a box is taken off the trigger the gate closes
    /// </summary>
    /// <param name="_collider">Player or Box Collider</param>
    private void OnTriggerExit2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag == "Player" || _collider.gameObject.tag == "Box")
        {
            isOpen = false;
            closegate.SetActive(true);
            openGate.SetActive(false);
        }
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
