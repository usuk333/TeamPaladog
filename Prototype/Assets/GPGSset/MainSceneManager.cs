using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SceneManagement;
using Firebase.Database;
using System.IO;

public class MainSceneManager : MonoBehaviour
{
    GameCenterManager gameCenterManager;

    private int Lv;
    private int EXP;

    [SerializeField] private Text TLv;
    [SerializeField] private Text TEXP;

    // Start is called before the first frame update
    void Start()
    {
        Loading();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Loading()
    {
        if(gameCenterManager.userdata != null)
        {
            gameCenterManager.reference.Child("users").Child(gameCenterManager.Usersid)
                .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("failed reading...");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    Debug.Log("InitializeFirebase 접근완료");

                    foreach (DataSnapshot data in snapshot.Children)
                    {
                        IDictionary userinfo = (IDictionary)data.Value;
                        //딕셔너리 공부해야할듯. 이해가 안댐
                        Debug.LogFormat("[Database] key : {0}, value :{1}", data.Key, data.Value);
                    }
                    
                }
            });
        }
    }
}
