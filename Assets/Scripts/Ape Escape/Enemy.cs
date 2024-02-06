using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] Vector3 min, max;
    Vector3 destination;

    private bool _onNavMeshLink = false;
    [SerializeField] private float _jumpDuration = 0.8f;
    private Transform player;
    [SerializeField] float playerDistanceDetection;
    [SerializeField] float visionAngle;
    [SerializeField] float playerAttackDistance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        RandomDestination();
        StartCoroutine("Patrol");
        StartCoroutine("Alert");
    }

    // Update is called once per frame
    void Update()
    {
        

        if (GetComponent<NavMeshAgent>().isOnOffMeshLink && _onNavMeshLink == false)
        {
            StartNavMeshLinkMovement();
        }
        if (_onNavMeshLink)
        {
            FaceTarget(GetComponent<NavMeshAgent>().currentOffMeshLinkData.endPos);
        }

    }

    public void RandomDestination()
    {
        destination = new Vector3(Random.Range(min.x, max.x), 0, Random.Range(min.z, max.z));
        GetComponent<NavMeshAgent>().SetDestination(destination);
        float speed = GetComponent<NavMeshAgent>().velocity.magnitude;
        GetComponent<Animator>().SetFloat("vel", speed);
    }

    IEnumerator Patrol()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, destination) < 1.5f) 
            {
               
                GetComponent<Animator>().SetFloat("vel", 0);
                yield return new WaitForSeconds(Random.Range(1f, 3f));
                RandomDestination();
            }
            yield return new WaitForEndOfFrame();
        }
        
    }

    IEnumerator Alert()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, player.position) < playerDistanceDetection)
            {
                Vector3 vectorPlayer = player.position - transform.position;
                if (Vector3.Angle(vectorPlayer.normalized, transform.forward) < visionAngle)
                { 
                    StopCoroutine("Patrol");
                    StartCoroutine("Attack");
                    break;
                }
            }
        }
        yield return new WaitForEndOfFrame() ;
    }

    IEnumerator Attack() 
    {
        StopCoroutine("Alert");
        while (true) 
        {
            if (Vector3.Distance(transform.position, player.position) < playerDistanceDetection) 
            {
                StartCoroutine("Patrol");
                StartCoroutine("Alert");
                StopCoroutine("Attack");

            }
            if (Vector3.Distance(transform.position, player.position) < playerAttackDistance)
            {
                GetComponent<NavMeshAgent>().SetDestination(transform.position);
                GetComponent<NavMeshAgent>().velocity = Vector3.zero;
                GetComponent<Animator>().SetTrigger("attack");
            }
            else { GetComponent<NavMeshAgent>().SetDestination(transform.position); }
            yield return new WaitForEndOfFrame();
        }
        
    }   

    private void StartNavMeshLinkMovement()
    {
        _onNavMeshLink = true;
        NavMeshLink link = (NavMeshLink)GetComponent<NavMeshAgent>().navMeshOwner;
        Spline spline = link.GetComponentInChildren<Spline>();

        PerformJump(link, spline);
    }

    private void PerformJump(NavMeshLink link, Spline spline)
    {
        GetComponent<Animator>().SetTrigger("jump");
        bool reverseDirection = CheckIfJumpingFromEndToStart(link);
        StartCoroutine(MoveOnOffMeshLink(spline, reverseDirection));

    }

    private bool CheckIfJumpingFromEndToStart(NavMeshLink link)
    {
        Vector3 startPosWorld
            = link.gameObject.transform.TransformPoint(link.startPoint);
        Vector3 endPosWorld
            = link.gameObject.transform.TransformPoint(link.endPoint);

        float distancePlayerToStart
            = Vector3.Distance(GetComponent<NavMeshAgent>().transform.position, startPosWorld);
        float distancePlayerToEnd
            = Vector3.Distance(GetComponent<NavMeshAgent>().transform.position, endPosWorld);


        return distancePlayerToStart > distancePlayerToEnd;
    }

    private IEnumerator MoveOnOffMeshLink(Spline spline, bool reverseDirection)
    {
        float currentTime = 0;
        Vector3 agentStartPosition = GetComponent<NavMeshAgent>().transform.position;

        while (currentTime < _jumpDuration)
        {
            currentTime += Time.deltaTime;

            float amount = Mathf.Clamp01(currentTime / _jumpDuration);
            amount = reverseDirection ? 1 - amount : amount;

            GetComponent<NavMeshAgent>().transform.position =
                reverseDirection ?
                spline.CalculatePositionCustomEnd(amount, agentStartPosition)
                : spline.CalculatePositionCustomStart(amount, agentStartPosition);

            yield return new WaitForEndOfFrame();
        }

        GetComponent<NavMeshAgent>().CompleteOffMeshLink();

        yield return new WaitForSeconds(0.1f);
        _onNavMeshLink = false;

    }

    void FaceTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation
            = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        transform.rotation
            = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }
}
