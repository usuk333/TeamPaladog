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
    MainManager gameCenterManager;

    private string Lv;
    private string EXP;

    [SerializeField] private Text TLv;
    [SerializeField] private Text TEXP;

    [SerializeField] private string Userid;

    private DatabaseReference reference;

    private void Awake()
    {
        FirebaseDatabase.GetInstance("https://acrobatgames-f9ba6-default-rtdb.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }
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
        Debug.Log(Userid);

        reference.Child("users").Child(Userid).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("failed reading...");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                foreach (DataSnapshot data in snapshot.Children)
                {
                    //µñ¼Å³Ê¸® °øºÎÇØ¾ßÇÒµí. ÀÌÇØ°¡ ¾È´ï
                    Debug.LogFormat("[Database] key : {0}, value :{1}", data.Key, data.Value);
                    if (data.Key == "EXP")
                    {
                        EXP = data.Value.ToString();
                    }
                }
            }
        });

        
    }

    public void UserInfoUpdate()
    {
        TEXP.text = "EXP : " + EXP;
    }
}
