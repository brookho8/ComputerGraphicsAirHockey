using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UserPuck : MonoBehaviour
{

    private Vector3 mousePosition;
    public float speed;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Plane plane = new Plane(Vector3.up, new Vector3(0, 1, 0));
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            mousePosition = ray.GetPoint(distance);
            mousePosition[1] = 1;

            if( !(mousePosition[0] < 15.0 & mousePosition[0] > -9.0 & mousePosition[2] < 3.6 & mousePosition[2] > -13)){
                mousePosition = new Vector3(-5, 1, -5);
            }
            rb.MovePosition(transform.position + (mousePosition - transform.position) * Time.fixedDeltaTime * speed);
        }
    }

}
