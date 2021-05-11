using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PullBox : MonoBehaviour
{
    // The distance from the player the raycast travels
    [SerializeField] private float rayDistance = .55f;

    [Header("Player Animator")]
    [SerializeField] private Animator animator;
    
    // the box the raycast is touching
    private GameObject box;


   
    // Update is called once per frame
    void Update()
    {
        //Casting the rays from the player in 4 directions.
        #region Cast Rays
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, rayDistance);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.right * -transform.localScale.x, rayDistance);
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up * transform.localScale.x, rayDistance);
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down * transform.localScale.x, rayDistance);
        #endregion

        //If a ray is hitting an object run the MoveBox function.
        #region Check Ray hits
        if (hitRight.collider != null)
        {            
            MoveBox(hitRight, "hitRight");
        }
        else if (hitLeft.collider != null)
        {
            MoveBox(hitLeft, "hitLeft");
        }
        else if (hitUp.collider != null)
        {
            MoveBox(hitUp, "hitUp");
        }
        else if (hitDown.collider != null)
        {
            MoveBox(hitDown, "hitDown");
        }
        #endregion


    }

    /// <summary>
    /// Handles the movement of push/pull boxes
    /// </summary>
    /// <param name="ray">The raycast that is touching the box</param>
    private void MoveBox(RaycastHit2D ray, string rayName)
    {
        switch (rayName)
        {
            case "hitRight":
                if (ray.collider != null && ray.collider.tag == "Box" && Input.GetButtonDown("Space"))
                {
                    animator.SetFloat("xInputPush", 1);
                    animator.SetBool("isPushing", true);
                    box = ray.collider.gameObject;
                    // Enable the fixed joint between the player and the object when holding space bar.
                    box.GetComponent<FixedJoint2D>().enabled = true;
                    box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();

                }
                else if (Input.GetButtonUp("Space"))
                {
                    // Disables the fixed joint when releasing space bar.
                    box.GetComponent<FixedJoint2D>().enabled = false;
                    animator.SetBool("isPushing", false);
                    animator.SetFloat("xInputPush", 0);
                }
                break;
            case "hitLeft":
                if (ray.collider != null && ray.collider.tag == "Box" && Input.GetButtonDown("Space"))
                {
                    animator.SetFloat("xInputPush", -1);
                    animator.SetBool("isPushing", true);
                    box = ray.collider.gameObject;
                    // Enable the fixed joint between the player and the object when holding space bar.
                    box.GetComponent<FixedJoint2D>().enabled = true;
                    box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();

                }
                else if (Input.GetButtonUp("Space"))
                {
                    // Disables the fixed joint when releasing space bar.
                    box.GetComponent<FixedJoint2D>().enabled = false;
                    animator.SetBool("isPushing", false);
                    animator.SetFloat("xInputPush", 0);
                }
                break;
            case "hitUp":
                if (ray.collider != null && ray.collider.tag == "Box" && Input.GetButtonDown("Space"))
                {
                    animator.SetFloat("yInputPush", 1);
                    animator.SetBool("isPushing", true);
                    box = ray.collider.gameObject;
                    // Enable the fixed joint between the player and the object when holding space bar.
                    box.GetComponent<FixedJoint2D>().enabled = true;
                    box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();

                }
                else if (Input.GetButtonUp("Space"))
                {
                    // Disables the fixed joint when releasing space bar.
                    box.GetComponent<FixedJoint2D>().enabled = false;
                    animator.SetBool("isPushing", false);
                    animator.SetFloat("yInputPush", 0);
                }
                break;
            case "hitDown":
                if (ray.collider != null && ray.collider.tag == "Box" && Input.GetButtonDown("Space"))
                {
                    animator.SetFloat("yInputPush", -1);
                    animator.SetBool("isPushing", true);
                    box = ray.collider.gameObject;
                    // Enable the fixed joint between the player and the object when holding space bar.
                    box.GetComponent<FixedJoint2D>().enabled = true;
                    box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();

                }
                else if (Input.GetButtonUp("Space"))
                {
                    // Disables the fixed joint when releasing space bar.
                    box.GetComponent<FixedJoint2D>().enabled = false;
                    animator.SetBool("isPushing", false);
                    animator.SetFloat("yInputPush", 0);
                }
                break;            
        }
              
    }
   

    //Draws the rays from the player so we can see them.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * rayDistance);
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.left * transform.localScale.x * rayDistance);
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.up * transform.localScale.x * rayDistance);
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.down * transform.localScale.x * rayDistance);
    }

    
}
