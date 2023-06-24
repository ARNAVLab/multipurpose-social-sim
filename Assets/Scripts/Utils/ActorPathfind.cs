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

    Transform agentBody;
    [SerializeField] float timeSpeed;

    TimeManager timeManager; 

    // Start is called before the first frame update
    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        actor = GetComponent<Actor>();
        agentMesh = GetComponent<NavMeshAgent>();
        agentBody = GetComponent<Transform>();
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
        if(timeManager.isPaused) {
            agentBody.position = target;
        } else {
            timeSpeed = (float)(1.0 / timeManager.tickRate);
            agentMesh.speed = timeSpeed;
            agentMesh.SetDestination(target);
        }
        
        
    }
}
