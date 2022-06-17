using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

 [RequireComponent(typeof(NavMeshAgent))]
public class PlayerMotor : MonoBehaviour
{
    Transform target; //target to follow
    NavMeshAgent agent; // Reference to our agent

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    //To enhance performance, use a coroutine instead of Update.
    void Update() 
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
            FaceTarget();
        }
    }
    
    public void MoveToPoint(Vector3 point)
    {
        agent.SetDestination(point);
    }

    public void FollowTarget ( Interactable newTarget)
    {
        //This line causes movement to feel bad when making small moves
        // agent.stoppingDistance = newTarget.radius * 1f;
        agent.updateRotation = false;
        target = newTarget.interactionTransform;

    }

    public void StopFollowingTarget ()
    {
        //This line causes me to move if near object and function is called.
        // agent.stoppingDistance = 1f;
        agent.updateRotation = true;
        target = null;
    }

    void FaceTarget() 
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
