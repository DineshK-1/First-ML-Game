using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PlayerAgent : Agent
{

    [SerializeField] private GameObject target;

    [SerializeField] private Vector3 initalPos;
    [SerializeField] private Transform targetPos;

    [SerializeField] private Renderer planeMat;

    [SerializeField] private Material success;
    [SerializeField] private Material failure;

    private float moveSpeed=5f;

    private float previousDistance = 0;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-8f, 19f), 15.30216f, Random.Range(-3f, 22f));
        target.transform.localPosition = new Vector3(Random.Range(-8f, 19f), 15.30216f, Random.Range(-3f, 22f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetPos.localPosition);

        Vector3 distanceVector = new Vector3(transform.localPosition.x - targetPos.transform.localPosition.x, transform.localPosition.y - targetPos.transform.localPosition.y, transform.localPosition.z - targetPos.transform.localPosition.z);
        sensor.AddObservation(distanceVector);
        sensor.AddObservation(distanceVector.magnitude);
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
            SetReward(10f);
            planeMat.material = success;
            EndEpisode();
        }
        if (collision.gameObject.tag == "Wall")
        {
            SetReward(-10f);
            planeMat.material = failure;
            EndEpisode();
        }
    }
    private void Update()
    {
        previousDistance = Vector3.Distance(gameObject.transform.localPosition, targetPos.localPosition);
    }

    private void LateUpdate()
    {
        float difference = previousDistance - Vector3.Distance(gameObject.transform.localPosition, targetPos.localPosition);
        if (difference > 0f)
        {
            AddReward(1f);
        }
        if (difference < 0f)
        {
            AddReward(-1f);
        }
    }
}
