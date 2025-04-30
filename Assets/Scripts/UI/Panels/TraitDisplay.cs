using Anthology.Models;
using SimManager.SimulationManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TraitDisplay : MonoBehaviour
{
    [SerializeField] private GameObject traitPrefab;
    private Panel traitPanel;
    private List<GameObject> traitsList = new List<GameObject>();
    [SerializeField] private Transform traitContainer;

    [SerializeField] private TraitTooltipUI tooltip;

    private NPC selectedCharacter;

    private void Start()
    {
        traitPanel = GetComponent<Panel>();
        //traitContainer = GetComponent<Panel>().content.transform;
        //WorldManager.simUpdated.AddListener(SimUpdateListener);
    }

    //private void SimUpdateListener()
    //{
    //    if (relationPanel.IsShown)
    //    {
    //        SimManager.NPCs.
    //    }
    //}

    public void DisplayTraits(NPC npc)
    {
        selectedCharacter = npc;

        foreach (GameObject traitObj in traitsList)
        {
            Destroy(traitObj);
        }
        traitsList.Clear();
        foreach (Trait t in npc.traits.trait)
        {
            if (t.HasTrait)
            {
                AddTrait(t.TraitName);
            }
            
        }
    }

    public void AddTrait(string traitName)
    {
        GameObject newEntryObj = Instantiate(traitPrefab);
        TraitUI newEntry = newEntryObj.GetComponent<TraitUI>();
        newEntry.Setup(traitName, DisplayTrait);
        newEntryObj.transform.SetParent(traitContainer);
        //newEntry.transform.SetAsLastSibling();
        newEntryObj.transform.localScale = Vector3.one;
        traitsList.Add(newEntryObj);
    }

    public void DisplayTrait(string traitName) 
    {
        //selectedCharacter.RemoveTrait(traitName);
        //DisplayTraits(selectedCharacter);
        Trait trait = selectedCharacter.traits.trait.Find(t => t.TraitName == traitName);

        string result = $"<b>{trait.TraitName}</b> Effects:\n";
        foreach (var effect in trait.effects)
        {
            string sign = effect.effectPositive ? "when gaining" : "when losing";
            result += $"{effect.effectStrength} {sign} {effect.effectMotive}\n";
        }

        Debug.Log(result);
        tooltip.Show(result);
    }
}
