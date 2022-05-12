using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AIPlayer : Agent
{

    public GameObject puckObject;
    public GameObject enemyObject;
    public float maxSpeed;

    private Rigidbody puckBody;
    private Vector3 moveDirection = new Vector3(15, 1, -5);
    private Rigidbody selfBody;
    private Rigidbody enemyBody;
    private float speed = 0;
    private int puckHits = 0;

    public override void Initialize()
    {
        base.Initialize();
        puckBody = puckObject.GetComponent<Rigidbody>();
        selfBody = GetComponent<Rigidbody>();
        enemyBody = enemyObject.GetComponent<Rigidbody>();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        //sensor.AddObservation(worldSpaceToNorm(puckBody.position));
        sensor.AddObservation(worldSpaceToNorm(selfBody.position));
        sensor.AddObservation(Vector3.Normalize(puckBody.velocity));
        sensor.AddObservation(Vector3.Normalize(selfBody.velocity));
        sensor.AddObservation(Vector3.Normalize(enemyBody.velocity));

        sensor.AddObservation(Vector3.Normalize(puckBody.position - selfBody.position));
        sensor.AddObservation(Vector3.Normalize(enemyBody.position - selfBody.position));
        sensor.AddObservation(Vector3.Normalize(new Vector3(15, 1, -5) - selfBody.position)); //Red Goal
        sensor.AddObservation(Vector3.Normalize(new Vector3(-8, 1, -5) - selfBody.position)); //Blue Goal


        sensor.AddObservation( Vector3.Normalize(puckBody.position - new Vector3(15, 1, -5))); //Red goal
        sensor.AddObservation( Vector3.Normalize(puckBody.position - new Vector3(-8, 1, -5))); //Blue goal
        //sensor.AddObservation(worldSpaceToNorm(enemyBody.position));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Debug.Log(actions.ContinuousActions.Array[actions.ContinuousActions.Offset]);
        // Debug.Log(actions.ContinuousActions.Array[actions.ContinuousActions.Offset+1]);
        // Debug.Log(actions.ContinuousActions.Array[actions.ContinuousActions.Offset+2]);
        // Debug.Log("============================================================");
        base.OnActionReceived(actions);
        moveDirection = new Vector3(actions.ContinuousActions.Array[actions.ContinuousActions.Offset], 0, actions.ContinuousActions.Array[actions.ContinuousActions.Offset+1]);
        //moveDirection = normToWorldSpace(moveDirection);
        speed = actions.ContinuousActions.Array[actions.ContinuousActions.Offset + 2];
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        Vector3 moveToward = (puckBody.position + new Vector3(13, 0, -5) ) / 2;
        actionsOut.ContinuousActions.Array[actionsOut.ContinuousActions.Offset]   = moveToward.x;
        //actionsOut.ContinuousActions.Array[actionsOut.ContinuousActions.Offset+1] = moveToward.y;
        actionsOut.ContinuousActions.Array[actionsOut.ContinuousActions.Offset+1] = moveToward.z;        
    }

    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
        puckBody.position = new Vector3(4, 1, -5);
        puckBody.velocity = new Vector3(0, 0, 0);

        selfBody.position = new Vector3(13, 1, -5);
        Debug.Log("Puck Hits: " + puckHits);
        puckHits = 0;
    }

    public void Score(int score){
        if(score > 0){
            AddReward(15);
        }
        else{
            AddReward(-15);
        }
        EndEpisode();
    }

    private Vector3 worldSpaceToNorm(Vector3 toNormalize){
        float x = toNormalize.x;
        float z = toNormalize.z;

        x += 8;
        x /= (23/2);
        x -= 1;

        z += 12;
        z /= (13/2);
        z -= 1;

        Vector3 toReturn = new Vector3(x, 1, z);
        return toReturn;
    }

    private Vector3 normToWorldSpace(Vector3 toUnNormalize){
        float x = toUnNormalize.x;
        float z = toUnNormalize.z;

        x += 1;
        x *= (23/2);
        x -= 8;

        z += 1;
        z *= (13/2);
        z -= 12;

        Vector3 toReturn = new Vector3(x, 1, z);
        return toReturn;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Puck"){
            AddReward(3.5f);
            puckHits += 1;
        }
    }

    void FixedUpdate(){
        //AddReward(Time.fixedDeltaTime * -.5f);

        if (selfBody.position[0] < -10 | selfBody.position[0] > 15 | selfBody.position[2] < -12 | selfBody.position[2] > 1){
            AddReward(-1f * Time.fixedDeltaTime);
        }
        else{ // In Bounds
            AddReward((10 - Vector3.Magnitude(puckBody.position - selfBody.position)) * Time.fixedDeltaTime / 10);
            if (selfBody.position.x > puckBody.position.x){
                //AddReward(.5f * Time.fixedDeltaTime); //Between Puck And Goal
            }
            else{
                AddReward(-.5f * Time.fixedDeltaTime); //Not Between
            }
        }
        selfBody.MovePosition(selfBody.position + moveDirection * Time.fixedDeltaTime * Mathf.Abs(speed) * maxSpeed);
    }
}
