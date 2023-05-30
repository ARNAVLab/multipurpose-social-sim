using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager
{
    private static SelectionManager _instance;
    public static SelectionManager Instance
    {
        get
        { 
            if (_instance == null)
            {
                _instance = new SelectionManager();
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    public HashSet<Selectable> Selected = new HashSet<Selectable>();
    public HashSet<Selectable> Hovered = new HashSet<Selectable>();
    
    private int _selectionIndex = 0;

    public void SelectOne()
    {
        if (Hovered.Count <= 0)
        {
            DeselectAll();
            return;
        }
        _selectionIndex %= Hovered.Count;
        Selectable toSelect = null;
        int index = 0;
        foreach (Selectable h in Hovered)
        {
            if (_selectionIndex == index++)
            {
                toSelect = h;
                break;
            }
        }
        DeselectAll();
        Select(toSelect);
        _selectionIndex++;
    }

    public void SelectAnother()
    {
        if (Hovered.Count <= 0)
        {
            return;
        }
        _selectionIndex %= Hovered.Count;
        Selectable toSelect = null;
        int index = 0;
        foreach (Selectable h in Hovered)
        {
            if (_selectionIndex == index++)
            {
                toSelect = h;
                break;
            }
        }
        Select(toSelect);
        _selectionIndex++;
    }

    public void SelectHovered()
    {
        foreach (Selectable s in Hovered)
        {
            Select(s);
        }
    }

    public void SelectMoreHovered()
    {
        DeselectAll();
        foreach (Selectable s in Hovered)
        {
            Select(s);
        }
    }

    public void Select(Selectable toSelect)
    {
        toSelect.Select();
        Selected.Add(toSelect);
    }
    public void Deselect(Selectable toDeselect)
    {
        toDeselect.Deselect();
        Selected.Remove(toDeselect);
    }
    public void Hover(Selectable toHover)
    {
        toHover.Hover();
        Debug.Log("Adding to Hovered list");
        Hovered.Add(toHover);
    }
    public void Unhover(Selectable toUnhover)
    {
        toUnhover.Unhover();
        Debug.Log("Removing from Hovered list");
        Hovered.Remove(toUnhover);
    }

    public void DeselectAll()
    {
        List<Selectable> toDeselect = new List<Selectable>();
        foreach (Selectable s in Selected)
        {
            toDeselect.Add(s);
        }
        foreach (Selectable s in toDeselect)
        {
            Deselect(s);
        }
    }
}
