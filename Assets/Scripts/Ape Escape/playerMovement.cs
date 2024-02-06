using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class playerMovement : MonoBehaviour
{
    [SerializeField] float speed;

    private Animator animator;
    private NavMeshAgent agent;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
    }

    private void LateUpdate()
    {
        speed = agent.velocity.magnitude;
        animator.SetFloat("vel", speed);
    }
}

