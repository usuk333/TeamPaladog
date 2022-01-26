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
    MyUserData userdata;

    private void Start()
    {
        userdata = new MyUserData();
    }
    public class MyUserData
    {
        public string uid;
        public string user_name;        //����Ʈ����
        public bool Area1;       //��ô��
        public bool Area2;
        public bool Area3;
        public bool Area4;
        public int A1Stage1_achievement;
        public int A1Stage2_achievement;
        public int A1Stage3_achievement;
        public int Skill1_Level;
        public int Skill2_Level;
        public int Skill3_Level;
    }

    public void UIDSave(string uid)
    {
        Debug.Log("UID �����ϱ�");

        userdata.user_name = "������";
        userdata.Area1 = false;
        userdata.Area2 = false;
        userdata.Area3 = false;
        userdata.Area4 = false;
        userdata.A1Stage1_achievement = 0;
        userdata.A1Stage2_achievement = 0;
        userdata.A1Stage3_achievement = 0;
        userdata.Skill1_Level = 1;
        userdata.Skill2_Level = 1;
        userdata.Skill3_Level = 1;
        userdata.uid = uid;

        string json = JsonUtility.ToJson(userdata, true);

        Debug.Log(json);
    }

    public void DefaultData()
    {

    }
}
