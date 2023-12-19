using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navigation : MonoBehaviour
{
    public Transform destino;
    private NavMeshAgent kart;
    // Start is called before the first frame update
    void Start()
    {
        kart = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        kart.destination = destino.position;
    }
}
