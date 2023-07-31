using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGenToJson : MonoBehaviour
{
    //Get StructureHelper (child of TerrainManager), acess StructureHelper class
    //Access structuresDictionary in StructureHelper
    //Loop through the dictionary, getting the gameObject value, then accessing the GOs
    //LocationData script
    //Add that data to the save data
    //save all the data for all of the locations?

    public List<LocationData> saveData = new List<LocationData>();
    public GameObject StructureHelper;
    public Dictionary<Vector3Int, GameObject> structuresDict = new Dictionary<Vector3Int, GameObject>();
    void Start()
    {
        StructureHelper = GameObject.Find("StructureHelper");
        structuresDict = StructureHelper.GetComponent<StructureHelper>().structuresDictionary;
        GetSaveData();
    }

    public void GetSaveData()
    {
        foreach (var structure in structuresDict)
        {
            GameObject structGameObject = structure.Value;
        }
    }

    public void SaveToJson()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
