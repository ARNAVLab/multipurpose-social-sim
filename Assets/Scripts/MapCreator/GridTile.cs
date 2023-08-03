using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    [SerializeField] private int _alpha;
    public DistributionButton _dist;
    private SpriteRenderer _renderer;
    private MapCreatorController _controller;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _controller = FindObjectOfType<MapCreatorController>();
    }
    
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

    public void SetColor(Color32 color)
    {
        color.a = (byte)_alpha;
        _renderer.color = color;
    }

    public void OnMouseEnter()
    {
        _controller.HoverTile(this);
    }

    public void OnMouseExit()
    {
        _controller.HoverTile(null);
    }
}
