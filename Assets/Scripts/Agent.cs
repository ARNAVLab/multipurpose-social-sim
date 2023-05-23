using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private static int nextUnusedID = 0;

    [SerializeField] private SpriteRenderer selectionOutline;
    private bool focused = false;
    public int AgentID { get; set; }
    

    private void Start()
    {
        // Make selection outline transparent
        selectionOutline.color *= new Color(1, 1, 1, 0);

        // Assign a unique identifier to this Agent
        AgentID = nextUnusedID;
        nextUnusedID++;

        // Register this Agent with the AgentManager (this will add it to a static Dictionary, keyed by ID)
        AgentManager.RegisterAgent(this);
    }

    private void Update()
    {
        //if ()
        //{

        //}
    }

    public void Focus(bool focus)
    {
        selectionOutline.color = focus ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);
        focused = focus;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Cursor"))
        {
            AgentManager.FocusAgent(AgentID);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Cursor"))
        {
            AgentManager.UnfocusAgent(AgentID);
        }
    }
}
