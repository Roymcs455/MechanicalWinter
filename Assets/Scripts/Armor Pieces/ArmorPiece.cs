using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor Piece", menuName = "Armor/Piece", order = 1)]
public class ArmorPiece : ScriptableObject
{
    private string pieceName;
    [Header("Mesh model")]
    public GameObject prefab;
    [Header("Stats")]
    public float explosiveResistance = 1.0f;
    public float antiMaterialResistance = 1.0f;
    public float energyResistance = 1.0f;
    public float weight = 100.0f;

    
}
