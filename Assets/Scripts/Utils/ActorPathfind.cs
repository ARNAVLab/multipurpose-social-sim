using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActorPathfind : MonoBehaviour
{
    // [SerializeField] Transform target;
    Vector3 target;

    Actor actor;
    NavMeshAgent agentMesh;
    [SerializeField] float timeSpeed;

    // Start is called before the first frame update
    void Start()
    {
        actor = GetComponent<Actor>();
        agentMesh = GetComponent<NavMeshAgent>();
		agentMesh.updateRotation = false;
		agentMesh.updateUpAxis = false;
        agentMesh.acceleration = 200000;
        agentMesh.angularSpeed = 200000;
        timeSpeed = 4;
    }

    // Update is called once per frame
    void Update()
    {

        target = new Vector3(actor.Info.currentLocation.xPos, actor.Info.currentLocation.yPos, 0.0f);
        agentMesh.speed = timeSpeed;
        agentMesh.SetDestination(target);
        
    }
}
