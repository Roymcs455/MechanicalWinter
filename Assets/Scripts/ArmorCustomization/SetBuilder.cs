using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SetBuilder : MonoBehaviour
{
    public ArmorPieceSO[] armorPieceSOs;
    private ProxySet activeSet;
    private ProxySet[] setList; 
    [SerializeField]
    private SetPiece[] spawnPoints;

    private SetBuildingUI ui;
    // Start is called before the first frame update
    void Start()
    {
        ui = GetComponent<SetBuildingUI>();
        //LoadAllArmorPieceSO();
        //PrintArmorPieceSO();
        //ArmorPieceSO[] newArray = SortByPiecePart(ChasisPart.HEAD);
        //Debug.Log($"Piece parts {SortByPiecePart(ChasisPart.TORSO)}");
        LoadAllArmorPieceSO();
        LoadAllSets();
        ui.ListSets(setList);
        activeSet = setList[0];
        ShowAllPieces();

    }

    private void LoadAllSets()
    {
        setList = Resources.LoadAll<ProxySet>("");
    }
    public void ChangeActiveSet(int index)
    {
        activeSet = setList[index];
        ShowAllPieces();
    }
    public void ShowAllPieces()
    {
        foreach (var piece in activeSet.components)
        {
            if (piece != null && piece.prefab!= null)
                ShowModel(piece.pieceLocation,piece.prefab);    
        }
    }
    public void ShowModel(PieceLocation pieceLocation, GameObject modelPrefab)
    {
        //Debug.Log($"Trying to show {pieceLocation}");
        SetPiece currentSpawnPoint = spawnPoints.Where(obj => obj.pieceLocation == pieceLocation).ToArray()[0];
        if (currentSpawnPoint != null)
        {
            //Debug.Log("currentSpawnPoint isn´t null");
            Transform currentTransform = currentSpawnPoint.transform;
            foreach (Transform child in currentTransform)
            {
                Destroy (child.gameObject);
            }
            Instantiate (modelPrefab, currentTransform);
        }
    }
    public void ListBy( ChasisPart  chasisPart)
    {
        
        ArmorPieceSO[] subarray = SortByPiecePart(chasisPart);
        ui.ShowButtons(subarray);
    }

    //public void CreateSet(string setName)
    //{
    //    ProxySet newProxySet = ProxySet.CreateInstance<ProxySet>();
    //    newProxySet.setName = "setName";
    //    AssetDatabase.CreateAsset(newProxySet, $"Assets/Resources/ScriptableObjects/Armorsets/{setName}.asset");
    //}
    public void LoadAllArmorPieceSO()
    {
        armorPieceSOs = Resources.LoadAll<ArmorPieceSO>("");
        //foreach (ArmorPieceSO piece in armorPieceSOs)
        //    Debug.Log($"Part: {piece.chasisPart}, {piece.pieceName}, {piece.chasisPart}, {piece.pieceLocation}");
    }
    public void PrintArmorPieceSO(ArmorPieceSO[] arrayArmors)
    {
        foreach (ArmorPieceSO piece in arrayArmors)
        {
            Debug.Log($" {piece.pieceName}");
        }
    }
    public ArmorPieceSO[] SortByPiecePart(ChasisPart part)
    {
        ArmorPieceSO[] sortedArray = armorPieceSOs.Where(obj => (int)obj.chasisPart == (int)part && obj.pieceHealth != 0).ToArray();
        //foreach (ArmorPieceSO piece in sortedArray)
        //    Debug.Log($"Part: {piece.chasisPart}, {piece.pieceName}, {piece.chasisPart}, {piece.pieceLocation}");
        return sortedArray;
    }

    public void SetPiece(ArmorPieceSO armorPieceSO)
    {
        activeSet.components[(int)armorPieceSO.pieceLocation] = armorPieceSO;
        ShowModel(armorPieceSO.pieceLocation, armorPieceSO.prefab);
        if(armorPieceSO.dependantSO != null)
        {
            Debug.Log("Has dependant ");
            SetPiece(armorPieceSO.dependantSO);
        }
    }
    public void SaveSetOnslot()
    {

    }
}
