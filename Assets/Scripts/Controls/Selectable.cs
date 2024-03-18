using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public bool isHovered { get; protected set; } = false;
    public bool isSelected { get; protected set; } = false;
    public bool isFocused { get; protected set; } = false;

    [SerializeField] protected SpriteRenderer selectionOutline;

    [SerializeField] protected Color colorHover;
    [SerializeField] protected Color colorSelect;
    [SerializeField] protected Color colorFocus;

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
        if (other.CompareTag("Cursor"))
        {
            SelectionManager.Instance.Hover(this);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Cursor"))
        {
            SelectionManager.Instance.Unhover(this);
        }
    }

    private void OnDisable()
    {
        if (isSelected) SelectionManager.Instance.Selected.Remove(this);
        if (isHovered) SelectionManager.Instance.Hovered.Remove(this);
    }

    protected enum OutlinePreset { NONE, HOVER, SELECT, FOCUS }
    protected void SetOutline(OutlinePreset preset)
    {
        switch (preset)
        {
            case OutlinePreset.NONE:
                {
                    //Debug.Log("SetOutline to NONE");
                    selectionOutline.gameObject.SetActive(false);
                    break;
                }
            case OutlinePreset.HOVER:
                {
                    //Debug.Log("SetOutline to HOVER");
                    selectionOutline.gameObject.SetActive(true);
                    selectionOutline.GetComponent<SpriteRenderer>().color = colorHover;
                    break;
                }
            case OutlinePreset.SELECT:
                {
                    //Debug.Log("SetOutline to SELECT");
                    selectionOutline.gameObject.SetActive(true);
                    selectionOutline.GetComponent<SpriteRenderer>().color = colorSelect;
                    break;
                }
            case OutlinePreset.FOCUS:
                {
                    //Debug.Log("SetOutline to FOCUS");
                    selectionOutline.gameObject.SetActive(true);
                    selectionOutline.GetComponent<SpriteRenderer>().color = colorFocus;
                    break;
                }
        }
    }

    public virtual void OnHover()
    {
        // There is never a situation where isFocused is true AND isSelected isn't.
        if (!isSelected)
            SetOutline(OutlinePreset.HOVER);
    }

    public virtual void OnUnhover()
    {
        if (!isSelected)
            SetOutline(OutlinePreset.NONE);
    }

    public virtual void OnSelect()
    {
        SetOutline(OutlinePreset.SELECT);
    }

    public virtual void OnDeselect()
    {
        SetOutline(OutlinePreset.NONE);
    }

    public virtual void Focus()
    {
        isFocused = true;
        SetOutline(OutlinePreset.FOCUS);
    }

    public virtual void Unfocus()
    {
        isFocused = false;
        // Debug.Log("I'm " + isSelected + " isSelected");
        SetOutline(isSelected ? OutlinePreset.SELECT : OutlinePreset.NONE);
    }
}
