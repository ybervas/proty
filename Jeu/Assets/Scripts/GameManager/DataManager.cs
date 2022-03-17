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
            File.Delete(Application.persistentDataPath + "/LevelData.lp");
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
        data = new SavableData(levelDisplays.GetDisplay().Count, levelDisplays.GetVersion());

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

        if (data.GetVersion() == 0 || data.GetVersion() < levelDisplays.GetVersion())
        {
            data.Update(levelDisplays.GetDisplay().Count, levelDisplays.GetVersion());
            SaveData();
        }


    }

    /// <summary>
    /// Unlocks a particular level
    /// </summary>
    /// <param name="levelNumber"></param>
    public void UnlockLevel(int levelNumber)
    {
        data.UnlockLevel(levelNumber);
    }



    public void SaveTime(int levelNumber, float time)
    {
        data.SaveTime(levelNumber, time);
        SaveData();
    }

    #region Getters
    public List<Level> GetLevels()
    {
        return data.GetLevels();
    }

    public float GetTime(int levelNumber)
    {
        return data.GetTime(levelNumber);
    }

    public void AddDeath()
    {
        data.AddDeath();
        SaveData();
    }

    public void AddCoin()
    {
        data.AddCoin();
        SaveData();
    }

    public int GetDeath()
    {
        return data.GetDeath();
    }

    public int GetCoin()
    {
        return data.GetCoin();
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
    [SerializeField]
    private float version;
    [SerializeField]
    private int nbDeath;
    [SerializeField]
    private int nbCoin;

    /// <summary>
    /// Creates a new List of Level
    /// </summary>
    /// <param name="size">The number of levels in the game</param>
    public SavableData(int size, float version)
    {
        allLevels = new List<Level>();
        for (int i = 0; i < size; i++)
        {
            allLevels.Add(new Level(i));
        }
        allLevels[0].Unlock();
        this.version = version;
        nbDeath = 0;
        nbCoin = 0;
    }

    public void Update(int newSize, float version)
    {
        if (newSize >= allLevels.Count)
        {
            for (int i = allLevels.Count; i < newSize; i++)
            {
                allLevels.Add(new Level(i));
            }
        }
        else
        {
            for (int i = newSize; i < allLevels.Count; i++)
            {
                allLevels.Remove(allLevels[i]);
            }
        }
        this.version = version;
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

    public void SaveTime(int levelNumber, float time)
    {
        allLevels[levelNumber].SaveTime(time);
    }

    public float GetTime(int levelNumber)
    {
        return allLevels[levelNumber].GetTime();
    }

    public float GetVersion()
    {
        return version;
    }

    public void AddDeath() { nbDeath++; }

    public int GetDeath() { return nbDeath; }

    public void AddCoin() { nbCoin++; }

    public int GetCoin() { return nbCoin; }

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
    private float bestTime;
    [SerializeField]
    private int[] stars;

    private int death;

    public Level(int levelNumber)
    {
        this.levelNumber = levelNumber;
        isUnlocked = false;
        bestTime = 0;
        death = 0;
    }

    #region Getters and Setters
    public int GetNumber() { return levelNumber; }

    public bool GetIsUnlocked() { return isUnlocked; }

    // Setter
    public void Unlock() { isUnlocked = true; }

    public void SaveTime(float time)
    {
        if (bestTime == 0f || time < bestTime)
        {
            bestTime = time;
            Debug.Log("Meilleur temps : " + time);
        }
    }

    public int GetTime() { return bestTime; }

    public void SetDeath()
    {
        death++;
    }

    public int GetDeath() { return death; }
    #endregion
}