using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullBox : MonoBehaviour
{

    [SerializeField] private float rayDistance = .75f;
    

    GameObject box;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, rayDistance);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.right * -transform.localScale.x, rayDistance);
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up * transform.localScale.x, rayDistance);
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down * transform.localScale.x, rayDistance);


        MoveBox(hitRight);
        MoveBox(hitLeft);
        MoveBox(hitUp);
        MoveBox(hitDown);

        

    }

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * rayDistance);
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.left * transform.localScale.x * rayDistance);
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.up * transform.localScale.x * rayDistance);
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.down * transform.localScale.x * rayDistance);
    }

}
