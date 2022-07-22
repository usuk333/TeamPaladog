using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

using Google;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

using System.IO;
using System.Threading.Tasks;

public static class DataManager//class DataManager : MonoBehaviour 
{ 
    public static string UserID;
    /*public string userID;

    public static DataManager instance;

    public GamePlayData gamePlayData;

    public DataSnapshot dataSnapshot;

    public DatabaseReference reference;
    public FirebaseApp firebaseApp;
    public FirebaseDatabase firebaseDatabase;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            gamePlayData = new GamePlayData();
            firebaseApp = FirebaseApp.DefaultInstance;
            firebaseDatabase = FirebaseDatabase.GetInstance(firebaseApp, "https://acrobatgames-f9ba6-default-rtdb.firebaseio.com/%22)");  
            reference = FirebaseDatabase.DefaultInstance.RootReference;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(instance);
    }
    private void InitData()
    {
        reference.Child("users").Child(userID).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("failed reading...");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("컴플릿떳음");
                dataSnapshot = task.Result;
                //딕셔너리 초기화 함수 호출
                InitDictionary();
            }
        });
    }
    private void InitDictionary()
    {
        gamePlayData.PlayerData.Add(dataSnapshot.Child("Info").Key, dataSnapshot.Child("Info").Value);
        gamePlayData.PlayerData.Add(dataSnapshot.Child("Skill").Key, dataSnapshot.Child("Skill").Value);
        gamePlayData.PlayerData.Add(dataSnapshot.Child("Stage").Key, dataSnapshot.Child("Stage").Value);
        for (int i = 0; i < gamePlayData.UnitData.Length; i++)
        {
            switch (i)
            {
                case 0:
                    gamePlayData.UnitData[i].Add(dataSnapshot.Child("Unit").Child("Warrior").Key, dataSnapshot.Child("Unit").Child("Warrior").Value);
                    break;
                case 1:
                    gamePlayData.UnitData[i].Add(dataSnapshot.Child("Unit").Child("Assassin").Key, dataSnapshot.Child("Unit").Child("Assassin").Value);
                    break;
                case 2:
                    gamePlayData.UnitData[i].Add(dataSnapshot.Child("Unit").Child("Magician").Key, dataSnapshot.Child("Unit").Child("Magician").Value);
                    break;
                case 3:
                    gamePlayData.UnitData[i].Add(dataSnapshot.Child("Unit").Child("Archor").Key, dataSnapshot.Child("Unit").Child("Archor").Value);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }
    }
    /// <summary>
    /// 0 = 전사, 1 = 도적, 2 = 마법사, 3 = 궁수, key = 접근할 항목, value = 수정값
    /// </summary>
    /// <param name="unitIndex"></param>
    public void UpdateUnitData(int unitIndex, string key, float value)
    {
        float result = (float)gamePlayData.UnitData[unitIndex][key] + value;
        gamePlayData.UnitData[unitIndex][key] = result;
        reference.Child("users").Child(userID).Child("Unit").Child(gamePlayData.UnitData[unitIndex][key].ToString()).UpdateChildrenAsync(gamePlayData.UnitData[unitIndex]);
    }
    /// <summary>
    /// key = 접근할 항목, value = 수정값
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void UpdatePlayerData(string key, float value)
    {
        float result = (float)gamePlayData.PlayerData[key] + value;
        gamePlayData.PlayerData[key] = result;
        reference.Child("users").Child(userID).Child("Info").Child(gamePlayData.PlayerData[key].ToString()).UpdateChildrenAsync(gamePlayData.PlayerData);
    }
    public void UpdateSkillData(string key, float value)
    {
        float result = (float)gamePlayData.PlayerData[key] + value;
        gamePlayData.PlayerData[key] = result;
    }
    public void UpdateStageData(string key, bool value)
    {
      
    }*/
}
