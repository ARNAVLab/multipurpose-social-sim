using Anthology.SimulationManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [Header("--- Incremental ---")]
    [SerializeField] private Button jumpBtn;
    private int tickJump = 1;
    private int tickJumpMin = 1;
    private int tickJumpMax = 100;
    [SerializeField] private InputField tickJumpFld;
    
    private bool isPaused = true;
    [Header("--- Continuous ---")]
    [SerializeField] private Button pauseBtn;
    [SerializeField] private Button playBtn;
    private float tickRate = 1;
    private float ticksPerSecMin = 1;
    private float ticksPerSecMax = 60;
    private float ticksPerSec = 1 / 60;
    [SerializeField] private InputField tickRateFld;

    private void Start()
    {
        StartCoroutine(TickLoop());
        SetPaused(true);
    }

    public void TickJump()
    {
        Tick(tickJump);
    }

    public void SetPaused(bool newVal)
    {
        isPaused = newVal;
        jumpBtn.interactable = isPaused;
        pauseBtn.gameObject.SetActive(!isPaused);
        playBtn.gameObject.SetActive(isPaused);
    }

    public void SetTickRate(string input)
    {
        float newTicksPerSec;

        if (!float.TryParse(input, out newTicksPerSec))
        {
            // Input field is not a valid float; roll back text
            tickRateFld.text = ticksPerSec.ToString();
            return;
        }

        // Provided string is a valid float

        newTicksPerSec = Mathf.Clamp(newTicksPerSec, ticksPerSecMin, ticksPerSecMax);
        tickRateFld.text = newTicksPerSec.ToString();
        ticksPerSec = newTicksPerSec;
        tickRate = 1 / ticksPerSec;
    }

    public void SetTickJump(string input)
    {
        int ticksPerJump;

        if (!int.TryParse(input, out ticksPerJump))
        {
            // Input field is not a valid int; roll back text
            tickJumpFld.text = tickJump.ToString();
            return;
        }

        // Provided string is a valid int

        ticksPerJump = Mathf.Clamp(ticksPerJump, tickJumpMin, tickJumpMax);
        tickJumpFld.text = ticksPerJump.ToString();
        tickJump = ticksPerJump;
    }

    private void Tick(int tickNum)
    {
        SimManager.GetIteration(tickNum);
        GetComponent<Panel>().SetTitle("Time: " + SimManager.NumIterations);
        WorldManager.actorsUpdated.Invoke();
    }

    IEnumerator TickLoop()
    {
        while (true)
        {
            if (isPaused)
            {
                // If time is paused, wait a frame before trying again
                yield return null;
                continue;
            } 

            Tick(tickJump);
            yield return new WaitForSeconds(tickRate);
        }
    }
}
