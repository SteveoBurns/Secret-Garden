using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// This will handle all player pick ups
/// </summary>
public class PlayerController : MonoBehaviour
{

    [Header("Petal UI Elements")]
    [SerializeField] private Image petal1_1;
    [SerializeField] private Image petal1_2;
    [SerializeField] private Image petal1_3;
    [SerializeField] private Image petal2_1;
    [SerializeField] private Image petal2_2;
    [SerializeField] private Image petal2_3;
    [SerializeField] private Image petal3_1;
    [SerializeField] private Image petal3_2;
    [SerializeField] private Image petal3_3;
    private int petalsCollected = 0;

    [Header("Handle Elements")]
    [SerializeField] private Image handle;
    private bool handleShow = false;

    [Header("Key Elements")]
    [SerializeField] private Image key;
    [SerializeField] private GameObject gameKey;
    private bool hasKey = false;

    
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //have to enable all images when I have the HUD scene.
        print("collision");
        #region Petal PickUps
        if (collision.gameObject.tag == "petal1_1")
        {
            //petal1_1.enabled = true;
            petalsCollected += 1;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "petal1_2")
        {
            //petal1_2.enabled = true;
            petalsCollected += 1;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "petal1_3")
        {
            //petal1_3.enabled = true;
            petalsCollected += 1;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "petal2_1")
        {
           // petal2_1.enabled = true;
            petalsCollected += 1;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "petal2_2")
        {
            //petal2_2.enabled = true;
            petalsCollected += 1;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "petal2_3")
        {
            //petal2_3.enabled = true;
            petalsCollected += 1;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "petal3_1")
        {
            //petal3_1.enabled = true;
            petalsCollected += 1;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "petal3_2")
        {
            //petal3_2.enabled = true;
            petalsCollected += 1;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "petal3_3")
        {
            //petal3_3.enabled = true;
            petalsCollected += 1;
            Destroy(collision.gameObject);
        }
        #endregion
        #region Handle PickUp
        if (collision.gameObject.tag=="handle")
        {
            handleShow = true;
            //handle.enabled = true;
            Destroy(collision.gameObject);
        }
        #endregion
        #region Handle Doors
        if (collision.gameObject.tag == "handle_door" && handleShow == true)
        {
            print("door collision");
            handleShow = false;
            //handle.enabled = false;
            //aminate door opening;
            Destroy(collision.gameObject);
        }
        #endregion
        #region Key Pickup
        if (petalsCollected == 3) //might have to set this to 9 if it persists though scene loads??
        {
            gameKey.SetActive(true);
        }
        
        if (collision.gameObject.tag == "key")
        {
            hasKey = true;
            gameKey.SetActive(false);
            //key.enabled = true;
            Destroy(collision.gameObject);
        }
        #endregion
        #region End Level Doors
        if (collision.gameObject.tag == "door1")
        {
            if (hasKey == true && petalsCollected == 3)//might not need petals collected variable here?
            {
                // animation door
                // play door sound
                // fade to black

                // Load next scene
            }
        }
        if (collision.gameObject.tag == "door2")
        {
            if (hasKey == true && petalsCollected == 3) //might need to be 6, not sure if value will carry through load scenes??
            {
                // animation door
                // play door sound
                // fade to black

                // Load next scene
            }
        }
        #endregion
    }


    // Start is called before the first frame update
    void Start()
    {
        gameKey.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }







}
