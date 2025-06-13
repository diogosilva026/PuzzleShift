using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public int correctIndex;
    public bool isPlacedCorrectly = false;
    public Vector3 spawnPosition;
    public TargetSquare currentTargetSquare;
}
