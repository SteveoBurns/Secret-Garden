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
    [SerializeField] public GameObject[] petalGameObjects;
    [SerializeField] static public int playerPetalIndex;

    public static int petalsCollected;

    [SerializeField] private GameObject finalFlower;

    [Header("Handle UI Elements")]
    [SerializeField] private Image handle;
    [SerializeField] private bool handleShow = false;

    [Header("Key UI Elements")]
    [SerializeField] private Image key;
    [SerializeField] private GameObject gameKey;
    [SerializeField] private bool hasKey = false;
    [SerializeField] GameObject letter;

    [SerializeField] Animator animator;

    void Start()
    {
        playerPetalIndex = 0;
        gameKey.SetActive(false);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "petal")
        {
            switch (collision.gameObject.name)
            {
                case "petal1_1":
                    playerPetalIndex = 0;
                    break;

                case "petal1_2":
                    playerPetalIndex = 1;
                    break;

                case "petal1_3":
                    playerPetalIndex = 2;
                    break;

                case "petal2_1":
                    playerPetalIndex = 3;
                    break;

                case "petal2_2":
                    playerPetalIndex = 4;
                    break;

                case "petal2_3":
                    playerPetalIndex = 5;
                    break;

                case "petal3_1":
                    playerPetalIndex = 6;
                    break;

                case "petal3_2":
                    playerPetalIndex = 7;
                    break;

                case "petal3_3":
                    playerPetalIndex = 8;
                    break;
            }

            petalGameObjects[playerPetalIndex].SetActive(false);

            UIManager.petalIndex = playerPetalIndex;
            UIManager.DisplayPetal(playerPetalIndex);
            petalsCollected++;  
        }

        #region End Level Doors

        if (collision.gameObject.tag == "End Door")
        {
            if (hasKey == true)
            {

                endDoor = collision.gameObject.GetComponent<EndLevelDoor>();
                endDoor.OpenDoor();
                //animator.SetTrigger("OpenGate");


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


                //play door sound
                // fade to black

                // Load next scene
            }
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
        
        if (collision.gameObject.tag == "key" && hasKey == false)
        {
            hasKey = true;
            gameKey.SetActive(false);
            finalFlower.SetActive(true); 
        }
        #endregion

#region Final Flower
       if (collision.gameObject.tag == "Final Flower") 
       {
           SceneManager.LoadScene("End Letter");
       }
#endregion

        #region Letter Pickup
        if (collision.gameObject.tag == "Letter")
        {
            letter.SetActive(false);
            finalFlower.SetActive(true);
        }
        #endregion

#region Level Loading
       if (collision.gameObject.tag == "EndLevel1")
        {
            UIManager.loadScene++;
            SceneManager.LoadScene(UIManager.loadScene);
        }

        if (collision.gameObject.tag == "EndLevel2")
        {
            UIManager.loadScene++;
            SceneManager.LoadScene(UIManager.loadScene);
        }
#endregion
    }
}
