using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyScript : MonoBehaviour
{
    public double viewAngle = 90d;
    public double viewDistance = 15d;
    public double hearDistance = 3d;
    public Transform enemyEye;
    public Transform target;

    private NavMeshAgent agent;
    private double rotationSpeed=10d;
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
    void Update()
    {
        double distanceToPlayer = Vector3.Distance(target.transform.position, agent.transform.position);
        if (distanceToPlayer <= hearDistance || IsInView())
        {
            MoveToTarget();
        }
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
        agentTransform.LookAt(target);
        agent.SetDestination(target.position);
    }
    
    
}