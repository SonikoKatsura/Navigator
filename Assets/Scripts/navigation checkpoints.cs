using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class navigationcheckpoints : MonoBehaviour
{
    public Transform routeFather;
    int indexChildren;
    Vector3 destination;

    private bool _onNavMeshLink = false;
    [SerializeField] private float _jumpDuration = 0.8f;

    // Start is called before the first frame update
    void Start()
        {
            GetComponent<NavMeshAgent>().autoTraverseOffMeshLink = false;

            destination = routeFather.GetChild(indexChildren).position;
            GetComponent<NavMeshAgent>().SetDestination(destination);
            AudioManager.instance.PlayMusic("Car");

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, destination) < 0.5f)
        {
            indexChildren++;
            if (indexChildren >= routeFather.childCount)
            indexChildren = 0;
            destination = routeFather.GetChild(indexChildren).position;
            GetComponent <NavMeshAgent>().SetDestination(destination);
        }

        if (GetComponent<NavMeshAgent>().isOnOffMeshLink && _onNavMeshLink == false)
        {
            StartNavMeshLinkMovement();
        }
        if (_onNavMeshLink)
        {
            FaceTarget(GetComponent<NavMeshAgent>().currentOffMeshLinkData.endPos);
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
        AudioManager.instance.PlaySFX("Jump");
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
