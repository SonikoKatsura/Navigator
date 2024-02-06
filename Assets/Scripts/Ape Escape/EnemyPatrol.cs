using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] Vector3 min, max, destination;
    public event Action<Vector3> OnGetDestination;
    [SerializeField] Transform player;
    [SerializeField] float playerDistanceDetection;
    private float visionangle;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        RandomDestination(); 
        StartCoroutine("Patrol");
        StartCoroutine("Alert");


    }
    public Vector3 RandomDestination()
    {
        destination = new Vector3(Random.Range(min.x, max.x), 0, Random.Range(min.z, max.z));
        GetComponent<NavMeshAgent>().SetDestination(destination);
        return destination;
       
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
                OnGetDestination?.Invoke(destination);
                Debug.Log($"Selected position is {destination}");
            }
        }
    }
    IEnumerator Alert()
    {
        while(true)
        {
            if (Vector3.Distance(transform.position, player.position) < playerDistanceDetection)
            {
               Vector3 vectorPlayer = player.position  - transform.position;
                if (Vector3.Angle(vectorPlayer.normalized, transform.forward) < visionangle)
                {
                    StopCoroutine("Patrol");
                    GetComponent<NavMeshAgent>().SetDestination(player.position);

                }
                else 
                {
                    StartCoroutine("Patrol");
                }
            }
            else 
            {
                StartCoroutine("Patrol");
            }
            yield return new WaitForEndOfFrame();
        }
       
    }
}
