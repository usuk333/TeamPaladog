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

public class FirebaseDataManager : MonoBehaviour
{
    //유닛 레벨
    private string Assassin_level = string.Empty;
    [SerializeField] private Text tAssassin_level;
    private string Warrior_level = string.Empty;
    [SerializeField] private Text tWarrior_level;
    private string Archor_level = string.Empty;
    [SerializeField] private Text tArchor_level;
    private string Magician_level = string.Empty;
    [SerializeField] private Text tMagician_level;

    //유닛 체력
    private string AssassinHP = string.Empty;
    [SerializeField] private Text tAssassinHP;

    private string WarriorHP = string.Empty;
    [SerializeField] private Text tWarriorHP;

    private string ArchorHP = string.Empty;
    [SerializeField] private Text tArchorHP;

    private string MagicianHP = string.Empty;
    [SerializeField] private Text tMagicianHP;

    //유닛 공격력
    private string AssassinATK = string.Empty;
    [SerializeField] private Text tAssassinATK;

    private string WarriorATK = string.Empty;
    [SerializeField] private Text tWarriorATK;

    private string ArchorATK = string.Empty;
    [SerializeField] private Text tArchorATK;

    private string MagicianATK = string.Empty;
    [SerializeField] private Text tMagicianATK;

    //스킬
    private string Heal_Level = string.Empty;
    private string Barrior_Level = string.Empty;
    private string PowerUp_Level = string.Empty;
    private string Attack_Level = string.Empty;

    //플레이어 정보
    private string PlayerHP = string.Empty;
    private string PlayerMP = string.Empty;
    private string Player_Level = string.Empty;
    private string Player_EXP = string.Empty;

    //파이어베이스 앱 주소
    FirebaseApp firebaseApp;
    FirebaseDatabase firebaseDatabase;

    [SerializeField] private string Userid;
    //NaIxowYCsaSqdaYaWtWbIYErkqM2

    private DatabaseReference reference;

    public DataSnapshot Datasnapshot;



    void Awake()
    {
        firebaseApp = FirebaseApp.DefaultInstance;
        firebaseDatabase = FirebaseDatabase.GetInstance(firebaseApp, "https://acrobatgames-f9ba6-default-rtdb.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

    }

    // Start is called before the first frame update
    void Start()
    {
        reference.Child("users").Child(Userid).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("failed reading...");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Complete");
                Datasnapshot = task.Result;
                Debug.Log("모든 데이터 snapshot 읽어오기");

                StartCoroutine(InitData());
            }
        });
    }
    private IEnumerator InitData()
    {
        yield return null;

        if (Datasnapshot  == null)
        {
            Start();
        }
        Debug.Log("Data 설치");
    }
}
