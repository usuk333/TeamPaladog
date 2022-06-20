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

using DG.Tweening;
using TMPro;

public class StageManager : MonoBehaviour
{


    //파이어베이스
    FirebaseDatabase firebaseDatabase;
    FirebaseApp firebaseApp;
    private DatabaseReference reference;
    DataSnapshot snapshot;
    DataSnapshot snapshots;
    DataSnapshot snapshotss;
    [SerializeField] private string Userid;

    private bool panelonoff;

    //스테이지 패널 관련
    [SerializeField] private GameObject StagePanel;
    [SerializeField] private string[] StageTopText = new string[6];

    private bool[] panelsonoff = new bool[6];

    //타워 오브젝트
    [SerializeField] private GameObject Tower;

    //마지막으로 사용한 유닛
    [SerializeField] private string[] UnitSetting = new string[4];

    //마지막으로 사용한 스킬
    [SerializeField] private GameObject[] SkillSettingIcon = new GameObject[4];
    [SerializeField] private string[] SkillSetting = new string[4];

    //유닛, 스킬 아이콘
    [SerializeField] private Sprite TankerIcon;
    [SerializeField] private Sprite MeleeIcon;
    [SerializeField] private Sprite ADIcon;
    [SerializeField] private Sprite MageIcon;

    private string Gold = string.Empty;
    [SerializeField] private TextMeshProUGUI tGold;

    //계열 포인트
    private string TankerPoints = string.Empty;
    [SerializeField] private TextMeshProUGUI tTankerPoints;
    private string WarriorPoints = string.Empty;
    [SerializeField] private TextMeshProUGUI tWarriorPoints;
    private string ADPoints = string.Empty;
    [SerializeField] private TextMeshProUGUI tADPoints;
    private string MagePoints = string.Empty;
    [SerializeField] private TextMeshProUGUI tMagePoints;

    [SerializeField] private Sprite[] SkillIcon = new Sprite[7];

    //층 설명과, 보상 개체 선언
    [SerializeField] private Text StageIntroduction;
    [SerializeField] private Text StageCompensation;

    //유닛 스킬 선택
    [SerializeField] private GameObject SkillSelectPanel;
    [SerializeField] private GameObject UnitSelectPanel;

    private int nowsetting;
    private int nowStage;

    [SerializeField] private GameObject[] Skilllist = new GameObject[7];
    [SerializeField] private string[] tSkilllist = new string[7];
    public GameObject[] BossSection = new GameObject[4];


    void Awake()
    {
        firebaseApp = FirebaseApp.DefaultInstance;
        firebaseDatabase = FirebaseDatabase.GetInstance(firebaseApp, "https://acrobatgames-f9ba6-default-rtdb.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        panelsonoff[0] = false;
        panelsonoff[1] = false;
        panelsonoff[2] = false;
        panelsonoff[3] = false;
        panelsonoff[4] = false;
        panelsonoff[5] = false;

        tSkilllist[0] = "Skill1";
        tSkilllist[1] = "Skill2";
        tSkilllist[2] = "Skill3";
        tSkilllist[3] = "Skill4";
        tSkilllist[4] = "Skill5";
        tSkilllist[5] = "Skill6";
        tSkilllist[6] = "Skill7";



        StageTopText[0] = "1층 부숴진 갑옷 \n 부숴진 갑옷 어쩌고저쩌고";
        StageTopText[1] = "2층 저쩌고보스\n (스토리 설명)";
        StageTopText[2] = "3층 어쩌고 보스\n (스토리 설명)";
        StageTopText[3] = "4층 어쩌고 보스\n (스토리 설명)";
        StageTopText[4] = "5층 어쩌고 보스\n (스토리 설명)";
        StageTopText[5] = "6층 어쩌고 보스\n (스토리 설명)";

        panelonoff = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        reference.Child("users").Child(Userid).Child("Info").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("failed reading...");
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                Debug.Log("데이터 접속");

                reference.Child("users").Child(Userid).Child("Stage").GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.LogError("failed reading...");
                    }
                    else if (task.IsCompleted)
                    {
                        Debug.Log("컴플릿떳음");
                        snapshots = task.Result;

                        reference.Child("users").Child(Userid).Child("Unit").GetValueAsync().ContinueWithOnMainThread(task =>
                        {
                            if (task.IsFaulted)
                            {
                                Debug.LogError("failed reading...");
                            }
                            else if (task.IsCompleted)
                            {
                                Debug.Log("컴플릿떳음");
                                snapshotss = task.Result;

                                StartCoroutine(UIupdate());
                            }
                        });
                    }
                });
            }
        });

        /*reference.Child("users").Child(Userid).Child("Stage").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("failed reading...");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("컴플릿떳음");
                snapshot = task.Result;

                StartCoroutine(UIupdate());
            }
        });

        reference.Child("users").Child(Userid).Child("Unit").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("failed reading...");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("컴플릿떳음");
                snapshots = task.Result;

                
            }
        });*/
    }

    private IEnumerator UIupdate()
    {
        yield return null;

        Debug.Log("UI업데이트시작");

        if(snapshot == null && snapshots == null && snapshotss == null)
        {
            Start();
        }
        Gold = snapshot.Child("Gold").Value.ToString();
        TankerPoints = snapshot.Child("Points").Child("TankerPoints").Value.ToString();
        WarriorPoints = snapshot.Child("Points").Child("WarriorPoints").Value.ToString();
        ADPoints = snapshot.Child("Points").Child("ADPoints").Value.ToString();
        MagePoints = snapshot.Child("Points").Child("MagePoints").Value.ToString();

        tGold.text = Gold;
        tTankerPoints.text = TankerPoints;
        tWarriorPoints.text = WarriorPoints;
        tADPoints.text = ADPoints;
        tMagePoints.text = MagePoints;

        SkillSetting[0] = snapshots.Child("LastPick").Child("Skill").Child("Skill1").Value.ToString();
        SkillSetting[1] = snapshots.Child("LastPick").Child("Skill").Child("Skill2").Value.ToString();
        SkillSetting[2] = snapshots.Child("LastPick").Child("Skill").Child("Skill3").Value.ToString();
        SkillSetting[3] = snapshots.Child("LastPick").Child("Skill").Child("Skill4").Value.ToString();

        for(int i = 0; i < SkillSetting.Length; i++)
        {
            for(int p = 0; p < tSkilllist.Length; p++)
            {
                if(SkillSetting[i] == tSkilllist[p])
                {
                    SkillSettingIcon[i].GetComponent<Image>().sprite = SkillIcon[p];
                }
            }
        }

        UnitSetting[0] = snapshots.Child("LastPick").Child("Unit").Child("Tanker").Value.ToString();
        UnitSetting[1] = snapshots.Child("LastPick").Child("Unit").Child("Melee").Value.ToString();
        UnitSetting[2] = snapshots.Child("LastPick").Child("Unit").Child("AD").Value.ToString();
        UnitSetting[3] = snapshots.Child("LastPick").Child("Unit").Child("Mage").Value.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StageInfo(int i)
    {
        Tower.transform.DOMoveX(350, 1);

        StageIntroduction.text = StageTopText[i];

        nowStage = i;
        StagePanel.SetActive(true);
    }

    public void UnitSelect()
    {

    }

    public void SkillSelect(int i)
    {
        SkillSelectPanel.SetActive(true);
        nowsetting = i;
        //SkillSettingIcon[i];
    }

    //SkillSelect패널에 있는 놈들
    public void SkillChange(int i)
    {
        for (int p = 0; p < SkillSetting.Length; p++)
        {
            if(SkillSetting[p] == tSkilllist[i])
            {
                SkillSetting[p] = SkillSetting[nowsetting];
                //SkillSetting[]
                SkillSettingIcon[p].GetComponent<Image>().sprite = SkillSettingIcon[nowsetting].GetComponent<Image>().sprite;
            }
        }
        SkillSetting[nowsetting] = tSkilllist[i];
        SkillSettingIcon[nowsetting].GetComponent<Image>().sprite = SkillIcon[i];

        SkillSelectPanel.SetActive(false);
    }

    public void GoBattleScene()
    {
        BossManager.BossSection = BossSection[nowStage];

        LoadingSceneController.LoadScene("Stage");
    }

    public void Return()
    {
        LoadingSceneController.LoadScene("StartScene");
    }
}
