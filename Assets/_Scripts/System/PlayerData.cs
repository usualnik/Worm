using System;
using UnityEngine;
//using YG;

[System.Serializable]
public class Data
{   
    public int CurrentLevel = 0; 
}

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }    
   
    public event Action OnCurrentLevelChanged;  
   
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
        //if (YG2.saves.YandexServerData != null && YG2.saves.YandexServerData != data)           
        //{
        //   LoadPlayerData();
        //}
        //else
        //{
             InitFirstTimePlayingData();
        //}
    }

    private void InitFirstTimePlayingData()
    {
        //build new data
        data = new Data();       
    }

    private void LoadPlayerData()
    {
        //this.data = YG2.saves.YandexServerData;
    }
    private void SavePlayerDataToYandex()
    {
        //YG2.saves.YandexServerData = this.data;
        //YG2.SaveProgress();
    }

    #region Get
    public int GetCurrentLevel() => data.CurrentLevel;

    #endregion

    #region Set
   
    public void SetCurrentLevel(int value)
    {
        var temp = data.CurrentLevel;
        data.CurrentLevel = value;
       
        if (data.CurrentLevel < 0)
        {
            data.CurrentLevel = temp;
        }
        else
        {
            SavePlayerDataToYandex();

            OnCurrentLevelChanged?.Invoke();
        }           
        
    }
    #endregion

}
