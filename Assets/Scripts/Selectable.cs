using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    [SerializeField] private Color _colorHover;
    [SerializeField] private Color _colorSelect;

    public bool isHovered {get; private set;} = false;
    public bool isSelected {get; private set;} = false;

    public void Select()
    {
        isSelected = true;
        OnSelect();
    }
    public void Deselect()
    {
        isSelected = false;
        OnDeselect();
    }

    public void Hover()
    {
        isHovered = true;
        OnHover();
    }
    public void Unhover()
    {
        isHovered = false;
        OnUnhover();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "SelectionBox")
        {
            SelectionManager.Instance.Hover(this);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "SelectionBox")
        {
            SelectionManager.Instance.Unhover(this);
        }
    }

    private void OnDisable()
    {
        if (isSelected) SelectionManager.Instance.Selected.Remove(this);
        if (isHovered) SelectionManager.Instance.Hovered.Remove(this);
    }

    public virtual void OnHover(){}
    public virtual void OnUnhover(){}
    public virtual void OnSelect(){}
    public virtual void OnDeselect(){}
}
