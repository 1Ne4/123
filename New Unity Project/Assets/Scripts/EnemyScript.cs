using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyScript : MonoBehaviour
{
    public double viewAngle = 90d;
    public double viewDistance = 15d;
    public double hearDistance = 3d;
    public Transform enemyEye;
    public Transform target;
    public Slider stelthBar;

    private NavMeshAgent agent;
    private double rotationSpeed = 10d;
    private Transform agentTransform;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        rotationSpeed = agent.angularSpeed;
        agentTransform = agent.transform;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        double distanceToPlayer = Vector3.Distance(target.transform.position, agent.transform.position);
        if (distanceToPlayer <= hearDistance || IsInView())
        {
            if (PlayerController.timeOfContact > 100)
                MoveToTarget();

            else
            {
                PlayerController.timeOfContact += 1;
            }
        }
        else
        {
            if (PlayerController.timeOfContact >= 0 && Time.timeScale.Equals(1f))
                PlayerController.timeOfContact -= 1;
        }

        stelthBar.value = PlayerController.timeOfContact;
    }

    private bool IsInView()
    {
        double realAngle = Vector3.Angle(enemyEye.forward, target.position - enemyEye.position);
        RaycastHit hit;
        if (Physics.Raycast(enemyEye.transform.position, target.position - enemyEye.position, out hit,
            (float) viewDistance))
        {
            if (realAngle < viewAngle / 2f && Vector3.Distance(enemyEye.position, target.position) <= viewDistance &&
                hit.transform == target.transform)
            {
                return true;
            }
        }

        return false;
    }

    private void MoveToTarget()
    {
        agentTransform.LookAt(new Vector3(target.position.x, 1, target.position.z));
        agent.SetDestination(new Vector3(target.position.x, 1, target.position.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Door")
        {
            agent.isStopped = true;
            other.GetComponent<Door>().Open();
            agent.isStopped = false;
        }
    }
}