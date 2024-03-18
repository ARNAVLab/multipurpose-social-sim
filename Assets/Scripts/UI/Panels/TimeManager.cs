using SimManager.SimulationManager;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [Header("--- General ---")]
    [SerializeField] private TextMeshProUGUI tickCountDisplay;
    [SerializeField] private int tickIncrMin = 1;
    [SerializeField] private int tickIncrMax = 100;
    [SerializeField] private InputField tickIncrFld;
    private int tickIncrement = 1;

    [Header("--- Manual ---")]
    [SerializeField] private Button jumpBtn;
    
    [Header("--- Automatic ---")]
    public bool isPaused = true;
    [SerializeField] private Button pauseBtn;
    [SerializeField] private Button[] playBtns;
    [SerializeField] private float[] presetTickRates;
    private float tickRate = -1;

    private void Start()
    {
        StartCoroutine(TickLoop());
        SetPlayMode(0);
    }

    public void TickJump()
    {
        Tick(tickIncrement);
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

    public void SetTickIncrement(string input)
    {
        int newPlayIncrement;

        if (!int.TryParse(input, out newPlayIncrement))
        {
            // Input field is not a valid int; roll back text
            tickIncrFld.text = tickIncrement.ToString();
            return;
        }

        // Provided string is a valid int
        tickIncrement = Mathf.Clamp(newPlayIncrement, tickIncrMin, tickIncrMax);
        tickIncrFld.text = tickIncrement.ToString();
    }

    private void Tick(int tickNum)
    {
        SimEngine.GetIteration(tickNum);
        tickCountDisplay.text = SimEngine.NumIterations.ToString();
        WorldManager.simUpdated.Invoke();
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

            Tick(tickIncrement);
            yield return new WaitForSeconds(tickRate);
        }
    }
}
