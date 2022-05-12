using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NiaveDefender : MonoBehaviour
{

    public GameObject puck;
    private Rigidbody puckBody;

    public float speed;
    public Vector3 goalPosition;
    private Rigidbody selfBody;

    // Start is called before the first frame update
    void Start()
    {
        puckBody = puck.GetComponent<Rigidbody>();
        selfBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 moveToward = (puckBody.position + goalPosition ) / 2;
        selfBody.MovePosition(selfBody.position + (moveToward - selfBody.position) * Time.fixedDeltaTime * speed);
    }
}
