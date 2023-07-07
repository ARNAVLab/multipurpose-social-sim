using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseMap : MonoBehaviour
{
    Dictionary<int, GameObject> tileset;
    Dictionary<int, GameObject> tile_groups;
    public GameObject prefab_tile1;
    public GameObject prefab_tile2;
    public GameObject prefab_tile3;
    public GameObject prefab_tile4;

    int map_width = 50;
    int map_height = 50;

    List<List<int>> noise_grid = new List<List<int>>();
    List<List<GameObject>> tile_grid = new List<List<GameObject>>();

    //recommended between 4 and 20
    public float magnification = 7.0f;

    public int x_offset = 0; //decrease moves left, increase moves right
    public int y_offset = 0; //decrase moves down, increase moves up

    // Start is called before the first frame update
    void Start()
    {
        CreateTileset();
        CreateTileGroups();
        GenerateMap();
    }

    void Update() {
        if (Input.GetKeyDown("space")) {
            x_offset = Random.Range(0, 999);
            y_offset = Random.Range(0, 999);
            GenerateMap();
        }
    }

    // Collect and assign ID codes to tile prefabs
    //  Best ordered to match land elevation/ any other linear scale.
    void CreateTileset() {
        tileset = new Dictionary<int, GameObject>();
        tileset.Add(0, prefab_tile1);
        tileset.Add(1, prefab_tile2);
        tileset.Add(2, prefab_tile3);
        tileset.Add(3, prefab_tile4);
    }

    // Create empty gameobjects for grouping tiles of same type
    void CreateTileGroups() {
        tile_groups = new Dictionary<int, GameObject>();
        foreach(KeyValuePair<int, GameObject> prefab_pair in tileset) {
            GameObject tile_group = new GameObject(prefab_pair.Value.name);
            tile_group.transform.parent = gameObject.transform;
            tile_group.transform.localPosition = new Vector3(0,0,0);
            tile_groups.Add(prefab_pair.Key, tile_group);
        }
    }

    // Generate a 2D grid using the Perlin noise function
    // storing it as both ID values and tile gameobjects.
    void GenerateMap() {
        for(int x = 0; x < map_width; x++) {
            noise_grid.Add(new List<int>());
            tile_grid.Add(new List<GameObject>());

            for (int y = 0; y < map_height; y++) {
                int tile_id = GetIdUsingPerlin(x, y);
                noise_grid[x].Add(tile_id);
                CreateTile(tile_id, x, y);
            }
        }
    }

    // Using a grid coordinate input, generate a Perlin
    // noise value to be converted into a tile ID code.
    int GetIdUsingPerlin(int x, int y) {
       float raw_perlin = Mathf.PerlinNoise((x - x_offset) / magnification , (y - y_offset) / magnification);
       float clamp_perlin = Mathf.Clamp(raw_perlin, 0.0f, 1.0f);

       float scale_perlin = clamp_perlin * tileset.Count;
       if(scale_perlin == 4) {
        scale_perlin = 3;
       }
       return Mathf.FloorToInt(scale_perlin);
    }

    // Creates a new tile using the type id code
    // set its location and group with like tiles.
    void CreateTile(int tile_id, int x, int y) {
        GameObject tile_prefab = tileset[tile_id];
        GameObject tile_group = tile_groups[tile_id];

        GameObject tile = Instantiate(tile_prefab, tile_group.transform);
        tile.name = string.Format("tile_x{0}_y{1}", x, y);
        tile.transform.localPosition = new Vector3(x, y, 0);
        int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        tile.layer = LayerIgnoreRaycast;

        tile_grid[x].Add(tile);
    }
}
