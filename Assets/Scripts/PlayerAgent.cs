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

    private float moveSpeed=.5f;

    private float previousDistance;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-8.6f, 19.35f), 15.30216f, Random.Range(-3.82f, 22.5f));

        target.transform.localPosition = new Vector3(Random.Range(-8.6f, 19.35f), 15.30216f, Random.Range(-3.82f, 22.5f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetPos.localPosition);
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
            planeMat.material = success;
            EndEpisode();
        }
        if (collision.gameObject.tag == "Wall")
        {
            SetReward(-1f);
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
            SetReward(.5f);
            planeMat.material = success;
        }
        if (difference < 0f)
        {
            SetReward(-.5f);
            planeMat.material = failure;
        }
    }
}
