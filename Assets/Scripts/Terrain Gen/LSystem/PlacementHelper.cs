using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlacementHelper
{
    public static List<Direction> findNeighbor(Vector3Int position, ICollection<Vector3Int> collection) {

        List<Direction> neighborDirections = new List<Direction>();
        if(collection.Contains(position + Vector3Int.right)) {
            neighborDirections.Add(Direction.Right);
        }
        if(collection.Contains(position - Vector3Int.right)) {
            neighborDirections.Add(Direction.Left);
        }
        if(collection.Contains(position + Vector3Int.up)) {
            neighborDirections.Add(Direction.Up);
        }
        if(collection.Contains(position - Vector3Int.up)) {
            neighborDirections.Add(Direction.Down);
        }
        return neighborDirections;
    }

    internal static Vector3Int GetOffsetFromDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return new Vector3Int(0,1,0);
            case Direction.Down:
                return new Vector3Int(0,-1,0);
            case Direction.Left:
                return Vector3Int.left;
            case Direction.Right:
                return Vector3Int.right;
            default:
                break;
        }
        throw new System.Exception("No direction such as" + direction);
    }

    public static Direction GetReverseDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
            default:
                break;
        }
        throw new System.Exception("No direction such as " + direction);
    }
}
