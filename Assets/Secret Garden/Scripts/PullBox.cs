using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PullBox : MonoBehaviour
{
    // The distance from the player the raycast travels
    [SerializeField] private float rayDistance = .55f;
    
    // the box the raycast is touching
    private GameObject box;


   
    // Update is called once per frame
    void Update()
    {
        #region Cast Rays
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, rayDistance);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.right * -transform.localScale.x, rayDistance);
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up * transform.localScale.x, rayDistance);
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down * transform.localScale.x, rayDistance);
        #endregion

        MoveBox(hitRight);
        MoveBox(hitLeft);
        MoveBox(hitUp);
        MoveBox(hitDown);
        // Might have to nest these within if statements to stop getting two boxes at one time???
        

    }

    /// <summary>
    /// Handles the movement of push/pull boxes
    /// </summary>
    /// <param name="ray">The raycast that is touching the box</param>
    private void MoveBox(RaycastHit2D ray)
    {
        if (ray.collider != null && ray.collider.tag == "Box" && Input.GetButtonDown("Space"))
        {

            box = ray.collider.gameObject;
            box.GetComponent<FixedJoint2D>().enabled = true;
            box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
        }
        else if (Input.GetButtonUp("Space"))
        {
            box.GetComponent<FixedJoint2D>().enabled = false;
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
