using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleVisualizer : MonoBehaviour
{
    public LSystemGenerator lsystem;
    List<Vector3> positions = new List<Vector3>();
    public GameObject prefab;
    public Material lineMaterial;

    private int length = 10; //edit road length, will decrease over time
    private float angle = 90; //edit road angle, stays constant for now

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

    public enum EncodingLetters {
        unknown = '1',
        save = '[',
        load = ']',
        draw = 'F',
        turnRight = '+',
        turnLeft = '-'
    }

    private void Start() {
        var sequence = lsystem.GenerateSentence();
        VisualizeSequence(sequence);
    }

    private void VisualizeSequence(string sequence) {
        Stack<GenSysParameters> savePoints = new Stack<GenSysParameters>();
        var currentPosition = Vector3.zero; //can edit this to generate multiple towns in different locations

        Vector3 direction = Vector3.up;
        Vector3 tempPosition = Vector3.zero; //used when drawing roads

        positions.Add(currentPosition);

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
                    DrawLine(tempPosition, currentPosition, Color.red);
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

        foreach (var position in positions) {
            Instantiate(prefab, position, Quaternion.identity);
        }
    }

    private void DrawLine(Vector3 start, Vector3 end, Color color) {
        GameObject line = new GameObject("line");
        line.transform.position = start;
        var lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.SetPosition(0, end);
        lineRenderer.SetPosition(1, start);
    }

}
