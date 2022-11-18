using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

using Google;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

using UnityEngine.SceneManagement;
using System.IO;
using System.Threading.Tasks;
using TMPro;

public class FirebaseData
{
    private DatabaseReference reference;
    FirebaseDatabase firebaseDatabase;
    FirebaseApp firebaseApp;

    public enum LoginType
    {
        None = 0,
        google = 1
    }

    public string UID;
    public string Nickname;

    public int PlayerHP;
    public int PlayerMP;

    public int PlayerLevel;
    public int PlayerEXP;

    public int HealLevel;
    public int BarriorLevel;
    public int PowerUpLevel;
    public int AttackLevel;


    public bool Stage1EasyClear;
    public bool Stage1NormalClear;
    public bool Stage1HardClear;

    public bool Stage2EasyClear;
    public bool Stage2NormalClear;
    public bool Stage2HardClear;

    public bool Stage3EasyClear;
    public bool Stage3NormalClear;
    public bool Stage3HardClear;

    public bool Stage4EasyClear;
    public bool Stage4NormalClear;
    public bool Stage4HardClear;


    public int Stage1EasyStar;
    public int Stage1NormalStar;
    public int Stage1HardStar;

    public int Stage2EasyStar;
    public int Stage2NormalStar;
    public int Stage2HardStar;

    public int Stage3EasyStar;
    public int Stage3NormalStar;
    public int Stage3HardStar;

    public int Stage4EasyStar;
    public int Stage4NormalStar;
    public int Stage4HardStar;

    public int Gold;
    public int WarriorPoints;
    public int AssassinPoints;
    public int ArchorPoints;
    public int MagacianPoints;
    public int SkillPoints;

    public int WarriorLevel;
    public int WarriorEXP;
    public int WarriorHP;
    public int WarriorATK;

    public int AssassinLevel;
    public int AssassinEXP;
    public int AssassinHP;
    public int AssassinATK;

    public int ArchorLevel;
    public int ArchorEXP;
    public int ArchorHP;
    public int ArchorATK;

    public int MagicianLevel;
    public int MagicianEXP;
    public int MagicianHP;
    public int MagicianATK;


    public FirebaseData(string UID)
    {
        this.UID = UID;
        LoadData();
    }

    DataSnapshot snapshot;

    DataSnapshot Infosnapshot;
    DataSnapshot Unitsnapshot;
    DataSnapshot Skillsnapshot;
    DataSnapshot Stagesnapshot;

    private void LoadData()
    {
        firebaseApp = FirebaseApp.DefaultInstance;
        firebaseDatabase = FirebaseDatabase.GetInstance(firebaseApp, "https://acrobatgames-f9ba6-default-rtdb.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        reference.Child("users").Child(UID).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("failed reading...");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Complete");
                snapshot = task.Result;
                Debug.Log("모든 데이터 snapshot 읽어오기");

                Infosnapshot = snapshot.Child("Info");
                Unitsnapshot = snapshot.Child("Unit");
                Skillsnapshot = snapshot.Child("Skill");
                Stagesnapshot = snapshot.Child("Stage");

                InitData();
            }
        });
    }

    public void InitData()
    {
        Nickname = Infosnapshot.Child("Nickname").Value.ToString();
        PlayerHP = System.Convert.ToInt32(Infosnapshot.Child("HP").Value.ToString());
        PlayerMP = System.Convert.ToInt32(Infosnapshot.Child("MP").Value.ToString());
        Debug.Log(PlayerHP);
    }
}
