using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a grid tile in the map creator grid,
/// can be painted with a distribution
/// </summary>
public class GridTile : MonoBehaviour
{
    [SerializeField] [Tooltip("Alpha color value of grid tile")] 
    private int _alpha;
    /// <summary>
    /// Distribution associated with grid tile
    /// </summary>
    public DistributionButton _dist;
    /// <summary>
    /// Sprite renderer of grid tile
    /// </summary>
    private SpriteRenderer _renderer;
    /// <summary>
    /// Reference to MapCreatorController
    /// </summary>
    private MapCreatorController _controller;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _controller = FindObjectOfType<MapCreatorController>();
    }
    
    /// <summary>
    /// Sets the distribution associated with the grid tile
    /// </summary>
    /// <param name="dist">Distribution button to associate</param>
    public void SetDist(DistributionButton dist)
    {
        if (dist == null)
        {
            SetColor(new Color32(255, 255, 255, (byte)_alpha));
        }
        else
        {
            SetColor(dist.PaintColor);
        }
        _dist?.GridTiles.Remove(this);
        _dist = dist;
        _dist?.GridTiles.Add(this);
    }

    /// <summary>
    /// Sets the color of the tile
    /// </summary>
    /// <param name="color">Color to set</param>
    public void SetColor(Color32 color)
    {
        color.a = (byte)_alpha;
        _renderer.color = color;
    }

    /// <summary>
    /// Hovers the grid tile when mouse intersects
    /// </summary>
    public void OnMouseEnter()
    {
        _controller.HoverTile(this);
    }

    /// <summary>
    /// Unhovers the grid tile when mouse intersects
    /// </summary>
    public void OnMouseExit()
    {
        _controller.HoverTile(null);
    }
}
