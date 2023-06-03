using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PlayerAgent : Agent
{

    [SerializeField] private Vector3 initalPos;
    [SerializeField] private Transform targetPos;

    private float moveSpeed=2f;

    private float previousDistance;

    public override void OnEpisodeBegin()
    {
        transform.position = initalPos;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(targetPos.position);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float vertical = actions.ContinuousActions[0] * Time.deltaTime * moveSpeed;
        float horizontal = actions.ContinuousActions[1] * Time.deltaTime * moveSpeed;

        transform.Translate(vertical, 0, horizontal);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Goal")
        {
            SetReward(1f);
            EndEpisode();
        }
        if (collision.gameObject.tag == "Wall")
        {
            SetReward(-2f);
            EndEpisode();
        }
    }



    private void Update()
    {
        previousDistance = Vector3.Distance(gameObject.transform.position, targetPos.position);
    }

    private void LateUpdate()
    {
        float difference = previousDistance - Vector3.Distance(gameObject.transform.position, targetPos.position);

        if (difference >= 0)
        {
            AddReward(.5f);
        }
        else
        {
            AddReward(-.5f);
        }
    }
}
