using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelDoor : MonoBehaviour
{
    [SerializeField] public Animator animator;
    
    public void OpenDoor()
    {
        //Plays the animation for the door opening
        animator.SetTrigger("EndGateOpen");
    }
}
