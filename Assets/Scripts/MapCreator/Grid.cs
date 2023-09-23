using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private float _tilePadding;
    private Dictionary<Vector2Int, GridTile> _tiles = new Dictionary<Vector2Int, GridTile>();
    private int _width = 0, _height = 0;

    public void SetGridSize(int width, int height)
    {
        SetGridWidth(width);
        SetGridHeight(height);
    }

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
