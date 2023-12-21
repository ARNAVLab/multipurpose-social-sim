using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Button associated with an agent distribution,
/// contains ratios of agent presets
/// </summary>
public class DistributionButton : MonoBehaviour
{
    [SerializeField] [Tooltip("Text component of distribution name")]
    private TMP_Text _nameLabel;
    
    /// <summary>
    /// Agent distribution associated with button
    /// </summary>
    public AgentDistribution Distribution;
    /// <summary>
    /// Set of grid tiles associated with distribution
    /// </summary>
    public HashSet<GridTile> GridTiles = new HashSet<GridTile>();
    /// <summary>
    /// Reference to map creator
    /// </summary>
    private MapCreator _mapCreator;
    /// <summary>
    /// Image displaying distribution color
    /// </summary>
    private Image _image;
    /// <summary>
    /// Color associated with distribution
    /// </summary>
    private Color32 _paintColor;
    /// <summary>
    /// Sets or gets the paint color of the distribution
    /// </summary>
    public Color32 PaintColor
    {
        get
        {
            return _paintColor;
        }
        set
        {
            _paintColor = value;
            _image.color = value;
            foreach (GridTile gt in GridTiles)
            {
                gt.SetColor(_paintColor);
            }
        }
    }
    /// <summary>
    /// Sets of gets the name of the distribution
    /// </summary>
    public string Name {
        get
        {
            return Distribution.Name;
        }
        set
        {
            Distribution.Name = value;
            _nameLabel.text = value;
        }
    }

    private void Awake()
    {
        _mapCreator = FindObjectOfType<MapCreator>();
        _image = GetComponent<Image>();
    }

    /// <summary>
    /// Selects the button's distribution
    /// </summary>
    public void SelectDist()
    {
        _mapCreator.SelectDist(this);
    }

    /// <summary>
    /// Gets the sum of all agent preset densities within the distribution
    /// </summary>
    /// <returns>ratio consequent value</returns>
    public float GetConsequent()
    {
        float total = 0;
        foreach (KeyValuePair<AgentPresetButton,float> pair in Distribution.AgentWeights)
        {
            total += pair.Value;
        }
        return total;
    }
}
