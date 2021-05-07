using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/// <summary>
/// This will handle all player pick ups
/// </summary>
public class PlayerController : MonoBehaviour
{
    private HandleGate gate;
    private EndLevelDoor endDoor;

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

    [SerializeField] private GameObject finalFlower;

    [Header("Handle UI Elements")]
    [SerializeField] private Image handle;
    [SerializeField] private bool handleShow = false;

    [Header("Key UI Elements")]
    [SerializeField] private Image key;
    [SerializeField] private GameObject gameKey;
    [SerializeField] private bool hasKey = false;

    
    
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
            gate = collision.gameObject.GetComponent<HandleGate>();
            gate.OpenGate();
           
            
        }
        #endregion
        #region Key Pickup
        if (petalsCollected == 3) 
        {
            gameKey.SetActive(true);
        }
        
        if (collision.gameObject.tag == "key")
        {
            hasKey = true;
            gameKey.SetActive(false);
            finalFlower.SetActive(true);
            //key.enabled = true;
            
        }
        #endregion
        #region End Level Doors
        if (collision.gameObject.tag == "End Door")
        {
            if (hasKey == true)
            {

                endDoor = collision.gameObject.GetComponent<EndLevelDoor>();
                endDoor.OpenDoor();
                
                // play door sound
                // fade to black

                // Load next scene
            }
        }

        #endregion
        #region Final Flower
        if (collision.gameObject.tag == "Final Flower") 
        {
            SceneManager.LoadScene("End Letter");
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
