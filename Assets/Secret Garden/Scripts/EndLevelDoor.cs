using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelDoor : MonoBehaviour
{
    [SerializeField] public Animator animator;
    
    public void OpenDoor()
    {
        animator.SetTrigger("EndGateOpen");
    }
}
