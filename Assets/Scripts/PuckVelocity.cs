using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckVelocity : MonoBehaviour
{

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(-8, 0, 0);
        //rb.velocity = new Vector3(0, 0, 0);
        Debug.Log("Done");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
