using System;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Data
{
    public int CurrentMaxLevel = 3;
}

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }

    public event Action OnCurrentMaxLevelChanged;

    [SerializeField] private Data data;

    private string SavePath => Path.Combine(Application.persistentDataPath, "playerData.json");

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadPlayerData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadPlayerData()
    {
        if (File.Exists(SavePath))
        {
            try
            {
                string json = File.ReadAllText(SavePath);
                data = JsonUtility.FromJson<Data>(json);
                Debug.Log("Game data loaded");
            }
            catch (Exception e)
            {
                Debug.LogError($"Load error: {e.Message}");
                InitFirstTimePlayingData();
            }
        }
        else
        {
            InitFirstTimePlayingData();
        }
    }

    private void SavePlayerData()
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
            Debug.Log("Game data saved");
        }
        catch (Exception e)
        {
            Debug.LogError($"Save error: {e.Message}");
        }
    }

    private void InitFirstTimePlayingData()
    {
        data = new Data();
        SavePlayerData();
    }

    #region Get
    public int GetCurrentLevel() => data.CurrentMaxLevel;
    #endregion

    #region Set
    public void SetCurrentMaxLevel(int value)
    {
        var temp = data.CurrentMaxLevel;
        data.CurrentMaxLevel = value;

        if (data.CurrentMaxLevel < 0)
        {
            data.CurrentMaxLevel = temp;
        }
        else
        {
            SavePlayerData();
            OnCurrentMaxLevelChanged?.Invoke();
        }
    }
    #endregion

    // Для отладки - посмотреть путь сохранения
    [ContextMenu("Print Save Path")]
    private void PrintSavePath()
    {
        Debug.Log($"Save path: {SavePath}");
    }
}