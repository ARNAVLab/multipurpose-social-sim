using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StructureType : MonoBehaviour
{
    [SerializeField]
    private GameObject[] prefabs; //Unity prefabs of structure types

    public int sizeRequired;
    public int quantity;
    public int quantityAlreadyPlaced;
    
    public GameObject GetPrefab()
    {
        quantityAlreadyPlaced++;
        if(prefabs.Length > 1)
        {
            
        }
        return null;
    }
}
