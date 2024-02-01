using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place : Selectable
{
    public string placeName;
    [SerializeField] private SpriteRenderer iconRenderer;
    [SerializeField] private SpriteRenderer mainSprite;
    [SerializeField] private SpriteRenderer innerSprite;

    [SerializeField] private List<Sprite> icons = new List<Sprite>();

    [SerializeField] private Color buildingColorMain;
    [SerializeField] private Color buildingColorInner;
    [SerializeField] private Color parkColorMain;
    [SerializeField] private Color parkColorInner;
    [SerializeField] private Color forestColorMain;
    [SerializeField] private Color forestColorInner;
    [SerializeField] private Color roadColorMain;
    [SerializeField] private Color roadColorInner;

    public enum Preset
    { 
        BUILDING,
        PARK,
        FOREST,
        ROAD
    }

    public void ChangeColorToPreset(Preset preset)
    {
        switch (preset)
        {
            case Preset.BUILDING:
                {
                    iconRenderer.sprite = icons[0];
                    mainSprite.color = buildingColorMain;
                    innerSprite.color = buildingColorInner;
                    break;
                }
            case Preset.PARK:
                {
                    iconRenderer.sprite = icons[1];
                    mainSprite.color = parkColorMain;
                    innerSprite.color = parkColorInner;
                    break;
                }
            case Preset.FOREST:
                {
                    iconRenderer.sprite = icons[1];
                    mainSprite.color = forestColorMain;
                    innerSprite.color = forestColorInner;
                    break;
                }
            case Preset.ROAD:
                {
                    iconRenderer.sprite = icons[2];
                    mainSprite.color = roadColorMain;
                    innerSprite.color = roadColorInner;
                    break;
                }
        }
    }
}
