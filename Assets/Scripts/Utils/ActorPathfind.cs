using Anthology.SimulationManager;
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
        if (!actor.Info.destination.Equals(""))
            CalcPathFind();
    }

    private void CalcPathFind()
    {
        Location dest = SimManager.Locations[actor.Info.destination];
        target = new Vector3(dest.Coordinates.X, dest.Coordinates.Y, 0.0f);
        if (timeManager.isPaused)
        {
            agentBody.position = target;
        }
        else
        {
            timeSpeed = (float)(1.0 / timeManager.GetTickRate());
            agentMesh.speed = timeSpeed;
            agentMesh.SetDestination(target);
        }
    }
}
