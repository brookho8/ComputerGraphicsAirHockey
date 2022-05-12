using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalArea : MonoBehaviour
{

    public int team;
    public GameObject puckObject;
    public GameObject teamMember;
    public GameObject opponent;

    private Rigidbody puck;

    // Start is called before the first frame update
    void Start()
    {
        puck = puckObject.GetComponent<Rigidbody>();
    }

    void Reset(){
        puck.position = new Vector3(4, 1, -5);
        puck.velocity = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (puck.position[0] > 30 | puck.position[0] < -30 | puck.position[2] > 30 | puck.position[2] < -30){
            Reset();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Puck")
        {
            Reset();

            AIPlayer aiComponentTeam = teamMember.GetComponent<AIPlayer>();
            if (aiComponentTeam){
                aiComponentTeam.Score(-1);
            }

            AIPlayer aiComponentEnemy = opponent.GetComponent<AIPlayer>();
            if (aiComponentEnemy){
                aiComponentEnemy.Score(1);
            }
        }
    }
}