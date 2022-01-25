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

public class DataController : MonoBehaviour
{
    public class UserData
    {
        public string uid;
        public string user_name;        //디폴트네임
        public bool Area1;       //진척도
        public bool Area2;
        public bool Area3;
        public bool Area4;
        public int A1Stage1_achievement;
        public int A1Stage2_achievement;
        public int A1Stage3_achievement;
        public int Skill1_Level;
        public int Skill2_Level;
        public int Skill3_Level;

        public UserData()
        {
            this.user_name = "defalut";
            this.Area1 = false;
            this.Area2 = false;
            this.Area3 = false;
            this.Area4 = false;
            this.A1Stage1_achievement = 0;
            this.A1Stage1_achievement = 0;
            this.A1Stage1_achievement = 0;
            this.Skill1_Level = 1;
            this.Skill2_Level = 1;
            this.Skill3_Level = 1;
        }
    }

    public void UIDSave(string uid)
    {
        Debug.Log("UID 저장하기");

        
    }
}
