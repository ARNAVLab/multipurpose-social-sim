using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Road Helper class takes generated LSys statement and turns it into roads
public class RoadHelper : MonoBehaviour
{
    public GameObject roadStraight, roadCorner, road3way, road4way, roadEnd;
    Dictionary<Vector3Int, GameObject> roadDictionary = new Dictionary<Vector3Int, GameObject>(); //Road data is stored in this dictionary
    HashSet<Vector3Int> fixRoadCandidates = new HashSet<Vector3Int>(); //set of road objects that may need to be changed into a corner or intersection. handled in fixRoads method

    //Helper method used in structure generation
    public List<Vector3Int> GetRoadPositions()
    {
        return roadDictionary.Keys.ToList();
    }

    //Places road position, default road is the straight road prefab
    //Stores the positions and gameObjects in the road Dictionary
    public void PlaceRoadPositions(Vector3 startPosition, Vector3Int direction, int length)
    {
        var rotation = Quaternion.Euler(0, 0, 90);

        if (direction.x == 0)
        {
            rotation = Quaternion.identity;
        }
        for (int i = 0; i < length; i++)
        {
            var position = Vector3Int.RoundToInt(startPosition + direction * i);
            if (roadDictionary.ContainsKey(position))
            {
                continue;
            }
            var road = Instantiate(roadStraight, position, rotation, transform);
            roadDictionary.Add(position, road);
            if (i == 0 || i == length - 1)
            {
                fixRoadCandidates.Add(position); //automatically adds first and last roads into the fixCandidate set
            }
        }
    }

    //This super long method just makes sure that the proper road sprite is used
    //This method is responsible for placing corner, road end, and intersection tiles
    public void FixRoad()
    {
        foreach (var position in fixRoadCandidates)
        {
            List<Direction> neighborDirections = PlacementHelper.findNeighbor(position, roadDictionary.Keys);

            Quaternion rotation = Quaternion.identity; //Default sprite rotation

            if (neighborDirections.Count == 1)
            {
                //create road end
                Destroy(roadDictionary[position]);
                if (neighborDirections.Contains(Direction.Right))
                {
                    rotation = Quaternion.Euler(0, 0, 90);
                }
                if (neighborDirections.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, 0, -90);
                }
                if (neighborDirections.Contains(Direction.Up))
                {
                    rotation = Quaternion.Euler(0, 0, 180);
                }

                roadDictionary[position] = Instantiate(roadEnd, position, rotation, transform);

            }
            else if (neighborDirections.Count == 2)
            {
                if (neighborDirections.Contains(Direction.Up) && neighborDirections.Contains(Direction.Down)
                    || neighborDirections.Contains(Direction.Right) && neighborDirections.Contains(Direction.Left))
                {
                    //straight road
                    continue;
                }

                Destroy(roadDictionary[position]);
                if (neighborDirections.Contains(Direction.Down) && neighborDirections.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, 0, -90);
                }
                if (neighborDirections.Contains(Direction.Up) && neighborDirections.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, 0, 180);
                }
                if (neighborDirections.Contains(Direction.Up) && neighborDirections.Contains(Direction.Right))
                {
                    rotation = Quaternion.Euler(0, 0, 90);
                }

                roadDictionary[position] = Instantiate(roadCorner, position, rotation, transform);

            }
            else if (neighborDirections.Count == 3)
            {
                //3 way
                Destroy(roadDictionary[position]);
                if (neighborDirections.Contains(Direction.Down) && neighborDirections.Contains(Direction.Right)
                    && neighborDirections.Contains(Direction.Up))
                {
                    rotation = Quaternion.Euler(0, 0, -90);
                }
                if (neighborDirections.Contains(Direction.Right) && neighborDirections.Contains(Direction.Left) 
                    && neighborDirections.Contains(Direction.Down))
                {
                    rotation = Quaternion.Euler(0, 0, 180);
                }
                if (neighborDirections.Contains(Direction.Up) && neighborDirections.Contains(Direction.Left)
                    && neighborDirections.Contains(Direction.Down))
                {
                    rotation = Quaternion.Euler(0, 0, 90);
                }

                roadDictionary[position] = Instantiate(road3way, position, rotation, transform);
            }
            else
            {
                //4 way
                Destroy(roadDictionary[position]);
                roadDictionary[position] = Instantiate(road4way, position, rotation, transform);
            }
        }
    }

    public void Reset()
    {
        foreach (var item in roadDictionary.Values)
        {
            Destroy(item);
        }
        roadDictionary.Clear();
        fixRoadCandidates = new HashSet<Vector3Int>();
    }
}
