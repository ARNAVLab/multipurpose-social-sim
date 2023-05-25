using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private static int nextUnusedID = 0;

    [SerializeField] private GameObject selectionOutline;
    private bool focused = false;
    public int AgentID { get; set; }
    public AgentInfo Info;
    public Color displayColor;

    private void Start()
    {
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
        GetComponent<SpriteRenderer>().color = displayColor;
    }

    public void ToggleOutline(bool show)
    {
        selectionOutline.SetActive(show);
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
