using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    private int currentTimeStep = 0;

    [Header("--- Incremental ---")]
    [SerializeField] private Button jumpBtn;
    private int tickJump = 1;
    [SerializeField] private InputField tickJumpFld;
    
    private bool isPaused = true;
    [Header("--- Continuous ---")]
    [SerializeField] private Button pauseBtn;
    [SerializeField] private Button playBtn;
    private float tickRate = 1;
    private float ticksPerSec = 1 / 60;
    [SerializeField] private InputField tickRateFld;

    private void Start()
    {
        StartCoroutine(TickLoop());
        SetPaused(true);
    }

    public void TickJump()
    {
        for (int i = 0; i < tickJump; i++)
        {
            Tick();
        }
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
        Debug.Log("set TickRate to " + input);

        float ticksPerSec;

        if (!float.TryParse(input, out ticksPerSec))
        {
            tickRateFld.text = this.ticksPerSec.ToString();
            return;
        }

        // Provided string is a valid float
        this.ticksPerSec = ticksPerSec;
        tickRate = 1 / ticksPerSec;
    }

    public void SetTickJump(string input)
    {
        Debug.Log("set TickJump to " + input);

        int ticksPerJump;

        if (!int.TryParse(input, out ticksPerJump))
        {
            tickJumpFld.text = tickJump.ToString();
            return;
        }

        // Provided string is a valid int
        tickJump = ticksPerJump;
    }

    private void Tick()
    {
        currentTimeStep++;
        GetComponent<Panel>().SetTitle("Time: " + currentTimeStep.ToString());
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

            Tick();
            yield return new WaitForSeconds(tickRate);
        }
    }
}
