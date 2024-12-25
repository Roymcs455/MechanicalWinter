using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    private JsonDataService dataService = new JsonDataService();
    string position = "pos";
    [SerializeField]
    private string[] armorPieceSOs;
    private void Awake()
    {
        dataService.CreateFolder("presets");
        string[] savedFilenames = dataService.GetSavedFiles();
        Debug.Log($"Awake de Savedata {savedFilenames.Length}");
        foreach (var item in savedFilenames)
        {
            Debug.Log(item);
        }
    }
    public void SerializeJson()
    {
        if (dataService.SaveData("/set123.json",armorPieceSOs) )
        {

        }
        else
        {
            Debug.Log("Error");
        }
    }
}
