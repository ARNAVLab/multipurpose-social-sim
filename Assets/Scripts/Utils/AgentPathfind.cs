using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentPathfind : MonoBehaviour
{
    // [SerializeField] Transform target;
    Vector3 target;

    UAgent agent;
    NavMeshAgent agentMesh;
    [SerializeField] float timeSpeed;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UAgent>();
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

        target = new Vector3(agent.Info.currentLocation.xPos, agent.Info.currentLocation.yPos, 0.0f);
        agentMesh.speed = timeSpeed;
        agentMesh.SetDestination(target);
        
    }
}
