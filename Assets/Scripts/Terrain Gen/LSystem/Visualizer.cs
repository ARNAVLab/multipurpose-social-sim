using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SimpleVisualizer;

//Visualizer places road and structure tiles in the scene, starting from 0,0
public class Visualizer : MonoBehaviour
{
    public LSystemGenerator lsystem;
    List<Vector3> positions = new List<Vector3>();

    public RoadHelper roadHelper;
    public StructureHelper structureHelper;

    private int length = 10; //can edit road length
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
        var sequence = lsystem.GenerateSentence(); //generate LSys sentence for road generation
        VisualizeSequence(sequence);
    }

    //Generation Method, accepts LSys sequence generated in Start method
    private void VisualizeSequence(string sequence) {
        Stack<GenSysParameters> savePoints = new Stack<GenSysParameters>();
        var currentPosition = Vector3.zero; //can edit this to generate multiple towns in different locations, by default cities start generation at pos (0,0,0)

        Vector3 direction = Vector3.up;
        Vector3 tempPosition = Vector3.zero; //used for drawing roads

        positions.Add(currentPosition);

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
                    positions.Add(currentPosition);
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
