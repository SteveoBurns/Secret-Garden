using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleGate : MonoBehaviour
{
    [SerializeField] public Animator animator;

    public void OpenGate()
    {
        animator.SetTrigger("GateOpen");
    }
}
