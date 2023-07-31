using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LocationData : MonoBehaviour
{
    //Holds location data needed for saving to JSON
    public string Name;
    public float X;
    public float Y;
    public HashSet<string> Tags = new HashSet<string>();
    public Dictionary<string, float> ConnectedLocations = new Dictionary<string, float>();
}
