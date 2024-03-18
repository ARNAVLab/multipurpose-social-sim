using Anthology.Models;
using SimManager.SimulationManager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JournalDisplay : MonoBehaviour
{
    private Panel journalPanel;
    [SerializeField] private TextMeshProUGUI journalTextGUI;

    private void Start()
    {
    }

    public void DisplayActorJournal(string journalText)
    {
        journalTextGUI.text = journalText;
    }
}
