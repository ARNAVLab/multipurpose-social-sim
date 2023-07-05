using Anthology.SimulationManager;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [Header("--- General ---")]
    [SerializeField] private int tickIncrMin = 1;
    [SerializeField] private int tickIncrMax = 100;

    [Header("--- Manual ---")]
    [SerializeField] private InputField jumpIncrFld;
    [SerializeField] private Button jumpBtn;
    private int jumpIncrement = 1;
    
    [Header("--- Automatic ---")]
    public bool isPaused = true;
    [SerializeField] private InputField playIncrFld;
    [SerializeField] private Button pauseBtn;
    [SerializeField] private Button[] playBtns;
    [SerializeField] private float[] presetTickRates;
    private int playIncrement = 1;
    private float tickRate = -1;

    private void Start()
    {
        StartCoroutine(TickLoop());
        SetPlayMode(0);
    }

    public void TickJump()
    {
        Tick(jumpIncrement);
    }

    public float GetTickRate()
    {
        return tickRate;
    }

    public void SetPlayMode(int playSpeedPreset)
    {
        isPaused = playSpeedPreset == 0;
        jumpBtn.interactable = isPaused;
        pauseBtn.interactable = !isPaused;

        int presetChosen = playSpeedPreset - 1;
        for (int btnIdx = 0; btnIdx < playBtns.Length; btnIdx++)
            playBtns[btnIdx].interactable = btnIdx != presetChosen;

        if (!isPaused)
            tickRate = presetTickRates[presetChosen];
    }

    public void SetPlayIncrement(string input)
    {
        int newPlayIncrement;

        if (!int.TryParse(input, out newPlayIncrement))
        {
            // Input field is not a valid int; roll back text
            playIncrFld.text = playIncrement.ToString();
            return;
        }

        // Provided string is a valid int
        playIncrement = Mathf.Clamp(newPlayIncrement, tickIncrMin, tickIncrMax);
        playIncrFld.text = playIncrement.ToString();
    }

    public void SetJumpIncrement(string input)
    {
        int newJumpIncrement;

        if (!int.TryParse(input, out newJumpIncrement))
        {
            // Input field is not a valid int; roll back text
            jumpIncrFld.text = jumpIncrement.ToString();
            return;
        }

        // Provided string is a valid int
        jumpIncrement = Mathf.Clamp(newJumpIncrement, tickIncrMin, tickIncrMax);
        jumpIncrFld.text = jumpIncrement.ToString();
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

            Tick(playIncrement);
            yield return new WaitForSeconds(tickRate);
        }
    }
}
