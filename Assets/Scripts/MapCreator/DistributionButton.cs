using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DistributionButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameLabel;
    
    public AgentDistribution Distribution;
    public HashSet<GridTile> GridTiles = new HashSet<GridTile>();
    private MapCreator _mapCreator;
    private Image _image;
    private Color32 _paintColor;
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

    public void SelectDist()
    {
        _mapCreator.SelectDist(this);
    }

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
