using System;
using UnityEngine;
using YG;

[System.Serializable]
public class Data
{   
    public int CurrentMaxLevel = 5; 
}

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }    
   
    public event Action OnCurrentMaxLevelChanged;  
   
    [SerializeField] private Data data;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            gameObject.transform.SetParent(null, false);
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        if (YG2.saves.YandexServerData != null && YG2.saves.YandexServerData != data)
        {
            LoadPlayerData();
        }
        else
        {
            InitFirstTimePlayingData();
        }
    }

    private void InitFirstTimePlayingData()
    {
        //build new data
        data = new Data();       
    }

    private void LoadPlayerData()
    {
        this.data = YG2.saves.YandexServerData;
    }
    private void SavePlayerDataToYandex()
    {
        YG2.saves.YandexServerData = this.data;
        YG2.SaveProgress();
    }

    #region Get
    public int GetCurrentMaxLevel() => data.CurrentMaxLevel;

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
            SavePlayerDataToYandex();

            OnCurrentMaxLevelChanged?.Invoke();
        }           
        
    }
    #endregion

}
