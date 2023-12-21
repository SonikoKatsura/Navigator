using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] GameObject obstacle;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (obstacle != null)
            {
                obstacle.SetActive(!obstacle.activeSelf);
            }
        }
    }
    private void Start()
    {
        obstacle.SetActive(false);
    }

}
