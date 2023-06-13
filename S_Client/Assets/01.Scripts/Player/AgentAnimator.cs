using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AgentAnimator : MonoBehaviour
{
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }
}
