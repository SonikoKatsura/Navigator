using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftTire : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            float x = Mathf.PingPong(t: Time.time, length: 10f);
            Vector3 axis = new Vector3(-x, y: 0, z: 0); 
            this.transform.Rotate(axis, angle: 1f); 
    }
    
}
