using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentPathfind : MonoBehaviour
{
    // [SerializeField] Transform target;
    Vector3 target;

    Agent agent;
    NavMeshAgent agentMesh;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<Agent>();
        agentMesh = GetComponent<NavMeshAgent>();
		agentMesh.updateRotation = false;
		agentMesh.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {

        target = new Vector3(agent.Info.currentLocation.xPos, agent.Info.currentLocation.yPos, 0.0f);
        agentMesh.SetDestination(target);
        
    }
}
