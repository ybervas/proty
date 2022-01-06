﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Saves the level and loads it back
/// Made by Léo PUYASTIER
/// </summary>
public class DataManager : MonoBehaviour
{
    [SerializeField]
    private SavableData data;
    [SerializeField]
    private LevelDisplayHolder levelDisplays;

    private void Awake()
    {
        if (File.Exists(Application.persistentDataPath + "/LevelData.lp"))
        {
            GetData();
        }
        else
        {
            CreateData();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveData();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            GetData();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("Suppression des données");
            File.Delete(Application.persistentDataPath + "/LevelData.json");
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("Triche détectée, votre tentative sera signalée à la police >:(");
            for (int i = 0; i < data.GetLevels().Count; i++)
            {
                UnlockLevel(i);
            }
            SaveData();
        }
    }

    /// <summary>
    /// Creates a new set of data
    /// </summary>
    private void CreateData()
    {
        data = new SavableData(levelDisplays.GetDisplay().Count);

        SaveData();
    }

    /// <summary>
    /// Saves the data to a json file
    /// </summary>
    public void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/LevelData.lp";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);

        stream.Close();
    }

    /// <summary>
    /// Loads the data from the file
    /// </summary>
    public void GetData()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/LevelData.lp";
        FileStream stream = new FileStream(path, FileMode.Open);

        data = formatter.Deserialize(stream) as SavableData;

        stream.Close();

    }

    /// <summary>
    /// Unlocks a particular level
    /// </summary>
    /// <param name="levelNumber"></param>
    public void UnlockLevel(int levelNumber)
    {
        data.UnlockLevel(levelNumber);
    }



    public void SaveTime(int levelNumber, int time)
    {
        data.SaveTime(levelNumber, time);
        SaveData();
    }

    #region Getters
    public List<Level> GetLevels()
    {
        return data.GetLevels();
    }
    #endregion

}


/// <summary>
/// Holds all the data that needs to be saved
/// </summary>
[System.Serializable]
public class SavableData
{
    [SerializeField]
    private List<Level> allLevels;

    /// <summary>
    /// Creates a new List of Level
    /// </summary>
    /// <param name="size">The number of levels in the game</param>
    public SavableData(int size)
    {
        allLevels = new List<Level>();
        for (int i = 0; i < size; i++)
        {
            allLevels.Add(new Level(i));
        }
        allLevels[0].Unlock();
    }

    #region Getter & Setter
    public List<Level> GetLevels() { return allLevels; }


    /// <summary>
    /// Unlocks a particular level
    /// </summary>
    /// <param name="levelNumber"></param>
    public void UnlockLevel(int levelNumber)
    {
        allLevels[levelNumber].Unlock();
    }

    public void SaveTime(int levelNumber, int time)
    {
        allLevels[levelNumber].SaveTime(time);
    }

    #endregion
}

/// <summary>
/// Creates a level that is displayable
/// </summary>
[System.Serializable]
public class Level
{
    [SerializeField]
    private int levelNumber;
    [SerializeField]
    private bool isUnlocked;
    [SerializeField]
    private int bestTime;
    [SerializeField]
    private int[] stars;

    public Level(int levelNumber)
    {
        this.levelNumber = levelNumber;
        isUnlocked = false;
        bestTime = 0;
    }

    #region Getters and Setters
    public int GetNumber() { return levelNumber; }

    public bool GetIsUnlocked() { return isUnlocked; }

    // Setter
    public void Unlock() { isUnlocked = true; }

    public void SaveTime(int time)
    {
        if (bestTime == 0 || time < bestTime)
        {
            bestTime = time;
            Debug.Log("Meilleur temps : " + time);
        }
    }

    public int GetTime() { return bestTime; }
    #endregion
}