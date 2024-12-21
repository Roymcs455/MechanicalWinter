using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor Piece", menuName = "Armor/Piece", order = 1)]
public class ArmorPieceSO : ScriptableObject
{
    [SerializeField]
    public string pieceName;
    [Header("Mesh model")]
    public GameObject prefab;
    [Header("Stats")]
    public float explosiveResistance = 1.0f;
    public float antiMaterialResistance = 1.0f;
    public float energyResistance = 1.0f;
    public float weight = 100.0f;
    public float pieceHealth;
    public ChasisPart chasisPart;
    public PieceLocation pieceLocation;
    public ArmorPieceSO dependantSO;

    
}
public enum ChasisPart
{
    HEAD,
    TORSO,
    LEG_L,
    LEG_R,
    ARM_L,
    ARM_R,
    TOTAL
}
public enum PieceLocation
{
    Torso,
    Hip,
    Head,
    LeftArm,
    RightArm,
    LeftForearm,
    RightForearm,
    LeftLeg,
    RightLeg,
    LeftShin,
    RightShin,
    TOTAL
}
