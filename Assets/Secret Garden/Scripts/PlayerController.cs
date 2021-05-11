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
    //Classes
    private HandleGate gate;
    private EndLevelDoor endDoor;

    [Header("Player Animator")]
    [SerializeField]private Animator animator;

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

    [Header("Handle UI Elements")]
    [SerializeField] private Image handle;
    [SerializeField] private bool hasHandle = false;

    [Header("Key UI Elements")]
    [SerializeField] private Image key;
    [SerializeField] private GameObject gameKey;
    [SerializeField] private bool hasKey = false;

    [Header("Level 3 objects")]
    [SerializeField] private GameObject finalFlower;
    [SerializeField] private GameObject letter;


    /// <summary>
    /// This handles all the collider based pickups and triggers for the player character.
    /// </summary>
    /// <param name="collision">The object the character interacts with</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //have to enable all images when I have the HUD scene.
        
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
        if (collision.gameObject.tag == "handle")
        {
            hasHandle = true;
            //handle.enabled = true;
            Destroy(collision.gameObject);
        }
        #endregion
        #region Handle Doors
        // This is for the animation for the door facing down
        if (collision.gameObject.tag == "handle_door" && hasHandle == true)
        {
            
            hasHandle = false;
            //handle.enabled = false;
            gate = collision.gameObject.GetComponent<HandleGate>();
            gate.OpenGate();
            animator.SetTrigger("OpenHandleUp");
            
        }
        // This is because the animation for the door needs to be the correct one
        if (collision.gameObject.tag == "handle_door_right" && hasHandle == true)
        {

            hasHandle = false;
            //handle.enabled = false;
            gate = collision.gameObject.GetComponent<HandleGate>();
            gate.OpenGate();
            animator.SetTrigger("OpenHandleRight");

        }
        #endregion
        #region Key Pickup
        if (petalsCollected == 3 && hasKey == false) 
        {
            gameKey.SetActive(true);
        }
        
        if (collision.gameObject.tag == "key")
        {
            hasKey = true;
            gameKey.SetActive(false);
            if (letter == null)
                return;
            else
                letter.SetActive(true);
            
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
                animator.SetTrigger("OpenGate");
                
                
                // play door sound
                // fade to black

                // Load next scene
            }
        }
        if (collision.gameObject.tag == "End Door1")
        {
            if (hasKey == true)
            {

                endDoor = collision.gameObject.GetComponent<EndLevelDoor>();
                endDoor.OpenDoor();
                animator.SetTrigger("OpenGateDown");


                // play door sound
                // fade to black

                // Load next scene
            }
        }

        #endregion
        #region Letter Pickup
        if (collision.gameObject.tag == "Letter")
        {
            letter.SetActive(false);
            finalFlower.SetActive(true);
        }
        #endregion
        #region Final Flower
        if (collision.gameObject.tag == "Final Flower") 
        {
            SceneManager.LoadScene("End Letter");
        }
        #endregion
        #region Level Loading
        if (collision.gameObject.tag == "EndLevel1")
            SceneManager.LoadScene("Level 2 Test");
        if (collision.gameObject.tag == "EndLevel2")
            SceneManager.LoadScene("Level 3 Test");
        #endregion



    }

    

    // Start is called before the first frame update
    void Start()
    {
        gameKey.SetActive(false);

        // Null checking for levels 1 & 2
        if (letter == null)
            return;
        else
            letter.SetActive(false);
        
        if (finalFlower == null)
            return;
        else
            finalFlower.SetActive(false);
        
    }

}
