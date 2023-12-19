using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navigationcheckpoints : MonoBehaviour
{
    public Transform routeFather;
    int indexChildren;
    Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        destination = routeFather.GetChild(indexChildren).position;
        GetComponent<NavMeshAgent>().SetDestination(destination);
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
    }
}
