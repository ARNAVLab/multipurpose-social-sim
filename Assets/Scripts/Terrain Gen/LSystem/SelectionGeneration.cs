using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SimpleVisualizer;

//Visualizer places road and structure tiles in the area selected by the user
//WIP
public class SelectionGeneration : MonoBehaviour
{
    public LSystemGenerator lsystem;
    [Tooltip("Custom starting position for town generation")]
    public int customOriginX = 0;
    [Tooltip("Custom starting position for town generation")]
    public int customOriginY = 0;

    public RoadHelper roadHelper;
    public StructureHelper structureHelper;

    [Tooltip("Size parameter for generation")]
    public int roadLength = 10; //can edit road length
    private int length = 10; 
    private float angle = 90; //can edit road angle, will stay constant for now

    public int Length {
        get {
            if (length > 0) {
                return length;
            } else {
                return 1;
            }
        }
        set => length = value;
    }

    private void Start() {
        CreateTown();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            CreateTown();
        }
    }

    public void CreateTown()
    {
        length = roadLength;
        roadHelper.Reset();
        structureHelper.Reset();
        var sequence = lsystem.GenerateSentence(); //generate LSys sentence for road generation
        VisualizeSequence(sequence);
    }

    //Generation Method, accepts LSys sequence generated in Start method
    private void VisualizeSequence(string sequence) {
        Stack<GenSysParameters> savePoints = new Stack<GenSysParameters>();
        var currentPosition = new Vector3(customOriginX, customOriginY, 0); //can edit this to generate towns in different locations

        Vector3 direction = Vector3.up;
        Vector3 tempPosition = Vector3.zero; //used for drawing roads

        //process the LSys sequence step-by-step
        //sequence instructions explained in SimpleVisualizer EncodingLetters enum
        foreach(var letter in sequence) {
            EncodingLetters encoding = (EncodingLetters)letter;
            switch (encoding) {

                case EncodingLetters.save:
                    savePoints.Push(new GenSysParameters {
                        position = currentPosition,
                        direction = direction,
                        length = Length
                    });
                    break;
                case EncodingLetters.load:
                    if(savePoints.Count > 0) {
                        var sysParameter = savePoints.Pop();
                        currentPosition = sysParameter.position;
                        direction = sysParameter.direction;
                        Length = sysParameter.length;
                    } else {
                        throw new System.Exception("Missing saved point in stack");
                    }
                    break;
                case EncodingLetters.draw:
                    tempPosition = currentPosition;
                    currentPosition += direction * length;
                    roadHelper.PlaceRoadPositions(tempPosition, Vector3Int.RoundToInt(direction), length); //determine road positions
                    Length -= 2; //shorten roads as generation iterates, can be edited
                    break;
                case EncodingLetters.turnRight:
                    direction = Quaternion.AngleAxis(angle, Vector3.forward) * direction;
                    break;
                case EncodingLetters.turnLeft:
                    direction = Quaternion.AngleAxis(-angle, Vector3.forward) * direction;
                    break;
                default:
                    break;
            }
        }
        roadHelper.FixRoad(); //place road gameObjects
        structureHelper.PlaceStructures(roadHelper.GetRoadPositions()); //place structure gameObjects based on road positions
    }
}
