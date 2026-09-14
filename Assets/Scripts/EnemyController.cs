using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    float speed = 0.5f;
    //Rigidbody rb;
    Animator anim;
    NavMeshAgent agent;
    [SerializeField]
    List<Transform> waypoints = new List<Transform>();

    [SerializeField]
    float waitTimeAtPoint = 2f;
    [SerializeField]
    bool patrolInLoop = true;

    int currentWaypointIndex = 0;
    bool isWaiting = false;
    bool movingForward = true;
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        if (waypoints == null || waypoints.Count == 0) return;

        GoToCurrentWaypoint();
    }

    void GoToCurrentWaypoint()
    {
        if (waypoints.Count == 0) return;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    void SelectNextwayPoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
    }

    IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTimeAtPoint);
        SelectNextwayPoint();
        anim.SetTrigger("Walk");
        agent.speed = speed;
        GoToCurrentWaypoint();
        isWaiting = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 forwardMove = transform.forward * speed;
        //rb.linearVelocity = new Vector3(forwardMove.x, rb.linearVelocity.y, forwardMove.z);
        if (waypoints.Count == 0 || isWaiting) return;
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            anim.SetTrigger("Stop");
            agent.speed = 0;
            StartCoroutine(WaitAtWaypoint());
        }
    }
}
