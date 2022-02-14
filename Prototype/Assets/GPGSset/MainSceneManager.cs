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
            EXP = gameCenterManager.userdata.EXP;
            Lv = gameCenterManager.userdata.Level;

            TEXP.text = EXP.ToString();
            TLv.text = Lv.ToString();
        }
    }
}
