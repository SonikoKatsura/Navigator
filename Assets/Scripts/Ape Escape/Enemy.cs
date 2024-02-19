using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private EnemyPatrol _input;
    [SerializeField]
    private AgentAnimation _agentAnimation;


    private void Start()
    {
        _input.OnSpeedChanged += _agentAnimation.SetSpeed;
        _input.OnStartJump.AddListener(_agentAnimation.Jump);
        _agentAnimation.SetSpeed(0);
    }

}