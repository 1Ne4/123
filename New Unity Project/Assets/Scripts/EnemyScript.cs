using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyScript : MonoBehaviour
{
    public double viewAngle = 90d;
    public double viewDistance = 15d;
    public double hearDistance = 3d;
    public Transform enemyEye;
    public Transform target;
    public Slider stelthBar;
    private bool isWalking;
    public Transform[] points;
    private bool isPatroling;
    private static int TimeOfContact = 0;
    private static bool isWatching;

    private NavMeshAgent agent;
    private double rotationSpeed = 10d;
    private Transform agentTransform;
    [SerializeField] Animator animator;
    [SerializeField] Image death;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        rotationSpeed = agent.angularSpeed;
        agentTransform = agent.transform;
        agent.autoBraking = false;
        animator.SetBool("Is walking", true);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        var distanceToPlayer = Vector3.Distance(target.transform.position, agent.transform.position);
        //if (agent.velocity == new Vector3(0, 0, 0))
        // {
        //animator.SetBool("Is walking", false);
        //}

        if (distanceToPlayer <= hearDistance || IsInView())
        {
            if (TimeOfContact >= 100)
            {
                
                MoveToTarget();
            }

            else
            {
                isWatching = true;
                TimeOfContact += 1;
            }
        }
        else
        {
            if (TimeOfContact >= 0 && Time.timeScale.Equals(1f) && !isWatching)
            {
                TimeOfContact -= 1;
            }

            if (TimeOfContact < 1 & !isPatroling)
            {
                isPatroling = true;
                var destPoint = Random.Range(0, points.Length);
                agentTransform.LookAt(points[destPoint].position);
                agent.destination = points[destPoint].position;
            }

            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                isPatroling = false;
                isWatching = false;
            }
        }

        stelthBar.value = TimeOfContact;
    }

    private bool IsInView()
    {
        var realAngle = Vector3.Angle(enemyEye.forward, target.position - enemyEye.position);
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
        Debug.Log("иду к игроку");
        agentTransform.LookAt(new Vector3(target.position.x, target.position.y, target.position.z));
        agent.SetDestination(new Vector3(target.position.x, target.position.y, target.position.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Door")
        {
            agent.isStopped = true;
            other.GetComponent<Door>().Open();
            agent.isStopped = false;
        }

        if (other.tag == "Player")
        {
            StartCoroutine(SubsScript.MansionPlaySubtitles7());
            death.color = new Color(0, 0, 0, Mathf.Lerp(0, 255, 7));
        }
    }

}