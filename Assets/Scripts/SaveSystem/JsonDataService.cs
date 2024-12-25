using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class JsonDataService : IDataService
{
    

    public bool SaveData<T>(string relativePath, T data)
    {
        string path = Application.persistentDataPath + relativePath;

        try
        {
            if (File.Exists(path))
            {
                Debug.Log($"Deleting {path}");
                File.Delete(path);
            }
            using FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, JsonConvert.SerializeObject(data,Formatting.Indented));
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }
        
        
    }
    public T LoadData<T>(string relativePath)
    {
        string path = Application.persistentDataPath+ relativePath;
        if(!File.Exists(path))
        {
            Debug.LogError($"File {path} not found");
            throw new FileNotFoundException($"{path} does not exists");
        }
        try
        {
            T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError (e);
            throw (e);
        }
        
    }
    public string[] GetSavedFiles()
    {
        string path = Application.persistentDataPath;
        Debug.Log(path);
        if (Directory.Exists(path+"/") ) 
        {
            string[] files = Directory.GetFiles(path,"*.json");
            return files;
        }

        return new string[0];
    }
    public bool CreateFolder(string folderName)
    {
        string folderPath = Path.Combine (Application.persistentDataPath, folderName);
        if (!Directory.Exists(folderPath))
        {
            // Crea la carpeta
            Directory.CreateDirectory(folderPath);
            Debug.Log($"Carpeta creada en: {folderPath}");
            return false;
        }

        return true;
    }
    

}
