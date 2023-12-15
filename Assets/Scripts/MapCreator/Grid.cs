using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Grid that manages distribution tiles in map creator
/// </summary>
public class Grid : MonoBehaviour
{
    [SerializeField] [Tooltip("Grid tile prefab")]
    private GameObject _tilePrefab;
    [SerializeField] [Tooltip("Space between tiles in grid")]
    private float _tilePadding;
    /// <summary>
    /// All grid tiles indexed by position
    /// </summary>
    private Dictionary<Vector2Int, GridTile> _tiles = new();
    /// <summary>
    /// Width and height of grid
    /// </summary>
    private int _width = 0, _height = 0;

    /// <summary>
    /// Sets the tile width and height of the grid
    /// </summary>
    /// <param name="width">width to set</param>
    /// <param name="height">height to set</param>
    public void SetGridSize(int width, int height)
    {
        SetGridWidth(width);
        SetGridHeight(height);
    }

    /// <summary>
    /// Sets the width of the grid and adds/removes tiles accordingly
    /// </summary>
    /// <param name="width">width to set</param>
    public void SetGridWidth(int width)
    {
        if (width == _width || width < 0)
            return;
        else if (width < _width)
        {
            for (int x = width; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Destroy(_tiles[new Vector2Int(x, y)].gameObject);
                    _tiles.Remove(new Vector2Int(x, y));
                }
            }
        }
        else
        {
            for (int x = _width; x < width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    CreateTile(x, y);
                }
            }
        }
        _width = width;
    }
    /// <summary>
    /// Sets the height of the grid and adds/removes tiles accordingly
    /// </summary>
    /// <param name="height">height to set</param>
    public void SetGridHeight(int height)
    {
        if (height == _height || height < 0)
            return;
        else if (height < _height)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = height; y < _height; y++)
                {
                    Destroy(_tiles[new Vector2Int(x, y)].gameObject);
                    _tiles.Remove(new Vector2Int(x, y));
                }
            }
        }
        else
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = _height; y < height; y++)
                {
                    CreateTile(x, y);
                }
            }
        }
        _height = height;
    }

    /// <summary>
    /// Creates a tile at the given index
    /// </summary>
    /// <param name="x">X-value of tile</param>
    /// <param name="y">Y-value of tile</param>
    private void CreateTile(int x, int y)
    {
        GameObject tileObject = Instantiate(_tilePrefab, this.transform);
        SpriteRenderer tileRenderer = tileObject.GetComponent<SpriteRenderer>();
        GridTile tileScript = tileObject.GetComponent<GridTile>();
        tileScript.SetDist(null);
        _tiles.Add(new Vector2Int(x, y), tileScript);
        tileObject.transform.position = new Vector2(
            _tilePadding + x * (tileRenderer.size.x + _tilePadding),
            _tilePadding + y * (tileRenderer.size.y + _tilePadding)
        );
    }
}
