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

using System;
using System.IO;
using System.Threading.Tasks;
using TMPro;

public class StartManager : MonoBehaviour
{
    public class Info
    {
        public int ArchorPoints { get; set; }
        public int AssassinPoints { get; set; }
        public int EXP { get; set; }
        public int Gold { get; set; }
        public int HP { get; set; }
        public int Level { get; set; }
        public int MP { get; set; }
        public int MagicianPoints { get; set; }
        public string Nickname { get; set; }
        public string UID { get; set; }
        public int WarriorPoints { get; set; }

        public Info(int ArchorPoints, int AssassinPoints, int EXP, int Gold, int HP, int Level, int MP,
            int MagicianPoints, string Nickname, string UID, int WarriorPoints)
        {
            this.ArchorPoints = ArchorPoints;
            this.AssassinPoints = AssassinPoints;
            this.EXP = EXP;
            this.Gold = Gold;
            this.HP = HP;
            this.Level = Level;
            this.MP = MP;
            this.MagicianPoints = MagicianPoints;
            this.Nickname = Nickname;
            this.UID = UID;
            this.WarriorPoints = WarriorPoints;
        }
    }

    public class Skill
    {
        public int Attack { get; set; }
        public int Barrior { get; set; }
        public int Heal { get; set; }
        public int PowerUp { get; set; }
        public int SkillPoints { get; set; }

        public Skill(int Attack, int Barrior, int Heal, int PowerUp, int SkillPoints)
        {
            this.Attack = Attack;
            this.Barrior = Barrior;
            this.Heal = Heal;
            this.PowerUp = PowerUp;
            this.SkillPoints = SkillPoints;
        }
    }

    public class Stage
    {
        public bool S1EClear { get; set; }
        public int S1EStar { get; set; }
        public bool S1HClear { get; set; }
        public int S1HStar { get; set; }
        public bool S1NClear { get; set; }
        public int S1NStar { get; set; }

        public bool S2EClear { get; set; }
        public int S2EStar { get; set; }
        public bool S2HClear { get; set; }
        public int S2HStar { get; set; }
        public bool S2NClear { get; set; }
        public int S2NStar { get; set; }

        public bool S3EClear { get; set; }
        public int S3EStar { get; set; }
        public bool S3HClear { get; set; }
        public int S3HStar { get; set; }
        public bool S3NClear { get; set; }
        public int S3NStar { get; set; }

        public bool S4EClear { get; set; }
        public int S4EStar { get; set; }
        public bool S4HClear { get; set; }
        public int S4HStar { get; set; }
        public bool S4NClear { get; set; }
        public int S4NStar { get; set; }

        public Stage(bool S1EClear, int S1EStar, bool S1HClear, int S1HStar, bool S1NClear, int S1NStar,
            bool S2EClear, int S2EStar, bool S2HClear, int S2HStar, bool S2NClear, int S2NStar,
            bool S3EClear, int S3EStar, bool S3HClear, int S3HStar, bool S3NClear, int S3NStar,
            bool S4EClear, int S4EStar, bool S4HClear, int S4HStar, bool S4NClear, int S4NStar)
        {
            this.S1EClear = S1EClear;
            this.S1EStar = S1EStar;
            this.S1HClear = S1HClear;
            this.S1HStar = S1HStar;
            this.S1NClear = S1NClear;
            this.S1NStar = S1NStar;

            this.S2EClear = S2EClear;
            this.S2EStar = S2EStar;
            this.S2HClear = S2HClear;
            this.S2HStar = S2HStar;
            this.S2NClear = S2NClear;
            this.S2NStar = S2NStar;

            this.S3EClear = S3EClear;
            this.S3EStar = S3EStar;
            this.S3HClear = S3HClear;
            this.S3HStar = S3HStar;
            this.S3NClear = S3NClear;
            this.S3NStar = S3NStar;

            this.S4EClear = S4EClear;
            this.S4EStar = S4EStar;
            this.S4HClear = S4HClear;
            this.S4HStar = S4HStar;
            this.S4NClear = S4NClear;
            this.S4NStar = S4NStar;
        }
    }

    public class Unit
    {
        public int ArchorATK { get; set; }
        public int ArchorEXP { get; set; }
        public int ArchorHP { get; set; }
        public int ArchorLevel { get; set; }
        public int AssassinATK { get; set; }
        public int AssassinEXP { get; set; }
        public int AssassinHP { get; set; }
        public int AssassinLevel { get; set; }
        public int MagicianATK { get; set; }
        public int MagicianEXP { get; set; }
        public int MagicianHP { get; set; }
        public int MagicianLevel { get; set; }
        public int WarriorATK { get; set; }
        public int WarriorEXP { get; set; }
        public int WarriorHP { get; set; }
        public int WarriorLevel { get; set; }

        public Unit(int ArchorATK, int ArchorEXP, int ArchorHP, int ArchorLevel,
            int AssassinATK, int AssassinEXP, int AssassinHP, int AssassinLevel,
            int MagicianATK, int MagicianEXP, int MagicianHP, int MagicianLevel,
            int WarriorATK, int WarriorEXP, int WarriorHP, int WarriorLevel)
        {
            this.ArchorATK = ArchorATK;
            this.ArchorEXP = ArchorEXP;
            this.ArchorHP = ArchorHP;
            this.ArchorLevel = ArchorLevel;
            this.AssassinATK = AssassinATK;
            this.AssassinEXP = AssassinEXP;
            this.AssassinHP = AssassinHP;
            this.AssassinLevel = AssassinLevel;
            this.MagicianATK = MagicianATK;
            this.MagicianEXP = MagicianEXP;
            this.MagicianHP = MagicianHP;
            this.MagicianLevel = MagicianLevel;
            this.WarriorATK = WarriorATK;
            this.WarriorEXP = WarriorEXP;
            this.WarriorHP = WarriorHP;
            this.WarriorLevel = WarriorLevel;
        }
    }

    public class Newuser
    {
        public Info info;
        public Skill skill;
        public Stage stage;
        public Unit unit;

        public Newuser(Info info, Skill skill, Stage stage, Unit unit)
        {
            this.info = info;
            this.skill = skill;
            this.stage = stage;
            this.unit = unit;
        }
    }

    private static StartManager instance;

    public static StartManager Instance { get => instance; }

    //Auth용 instance
    FirebaseAuth auth = null;

    //사용자 계정
    FirebaseUser user = null;

    //기기연동이 되어있는 상태인지 체크하는 변수
    private bool signedIn = false;

    public string FireBaseId = string.Empty;

    public PlayerData playerdata;

    //데이터
    DataSnapshot snapshot;

    //데이터베이스 reference
    public DatabaseReference reference;

    //테스트
    [SerializeField] private string Usersid;

    private FirebaseData firebaseData;

    public FirebaseData FirebaseData { get => firebaseData; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(instance);

        firebaseData = new FirebaseData(FireBaseId);
        StartCoroutine(firebaseData.InitData());
        //초기화 auth
        auth = FirebaseAuth.DefaultInstance;

        //유저의 로그인 정보에 변경점을 체크하면 이벤트를 걸어줌
        auth.StateChanged += AuthStateChanged;
        //AuthStateChanged(this, null);

        //첫 시작시 Off
        //LoginPanel.SetActive(false);
        //LoadingPanel.SetActive(false);

        //Firebase reference 경로 설정
        FirebaseDatabase.GetInstance("https://acrobatgames-f9ba6-default-rtdb.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .RequestServerAuthCode(false /* Don't force refresh */)
            .RequestIdToken()
            .RequestEmail()
            .EnableSavedGames()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (!firebaseData.dataLoadComplete) return;

            firebaseData.SaveData("Info", "Nickname", "백엔드는 어려워");
        }
    }

    //구글 로그인 버튼
    public void GoogleLoginBtn()
    {
        //연동 상태가 아니라면
        if (!signedIn)
        {
            //LoginPanel.SetActive(true);
        }
        else
        {
            CheckUID();
        }
    }

    //파일저장소 확인 후, 로그인 기록 살펴서 로그인할지말지
    private void CheckUID()
    {
        if (File.Exists(Application.persistentDataPath + "/Userdata.json"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/Userdata.json");
            playerdata = JsonUtility.FromJson<PlayerData>(json);

            Usersid = playerdata.UID;

            reference.Child("users").Child(Usersid).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("failed reading...");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    Debug.Log("CheckUID 파이어베이스 접근 완료");
                }
            });
        }
        else
        {
            //LoginPanel.SetActive(true);
            GoogleLogin();
        }
    }

    private void GoogleLogin()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate(success => 
            {
                if (success)
                {
                    Debug.Log("TryFirebaseLogin �Լ� ����");
                    StartCoroutine(TryFirebaseLogin());
                }
                else
                {
                    Debug.Log("����!");
                }
            });
        }
    }

    public IEnumerator TryFirebaseLogin()
    {
        while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
            yield return null;

        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
        string accessToken = null;

        Credential credential = GoogleAuthProvider.GetCredential(idToken, accessToken);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => { //ContinueWithOnMainThread
            if (task.IsCanceled)        //취소
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            user = task.Result;     //newuser 인식
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                user.DisplayName, user.UserId);


            Usersid = user.UserId;
            DataManager.UserID = Usersid;
            PlayerDataSet(Usersid);
            //UIDRigister(user.UserId, "Google");
            //InitializeFirebase();
        });
    }

    private void PlayerDataSet(string uid)
    {
        reference.Child("users").Child(uid).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)   //기존 데이터 확인
            {
                LoadingSceneController.LoadScene("StartScene");
                /*DataSnapshot snapshot = task.Result;
                Debug.Log("InitializeFirebase ���ٿϷ�");

                foreach (DataSnapshot data in snapshot.Children)
                {
                    Debug.LogFormat("[Database] key : {0}, value :{1}", data.Key, data.Value);
                }*/
            }
            else                    //기존 데이터가 없고, 기초 데이터 설치
            {
                //데이터 빌딩 (합작)
                /*Newuser newuser = new Newuser();

                Info info = new Info(0, 0, 0, 0, 100, 1, 20, 0, "defalut", Usersid, 0);
                Skill skill = new Skill(1, 1, 1, 1, 0);
                Stage stage = new Stage(false, 0, false, 0, false, 0, false, 0, false, 0, false, 0,
                    false, 0, false, 0, false, 0, false, 0, false, 0, false, 0);
                Unit unit = new Unit(90, 0, 100, 1, 100, 0, 100, 1, 100, 0, 80, 1, 100, 0, 110, 1);

                string json = JsonUtility.ToJson(newuser);
                reference.Child("users").Child(Usersid).SetRawJsonValueAsync(json);*/
            }
        });
    }

    public void test()
    {
        Info info = new Info(0, 0, 0, 0, 100, 1, 20, 0, "defalut", Usersid, 0);
        Skill skill = new Skill(1, 1, 1, 1, 0);
        Stage stage = new Stage(false, 0, false, 0, false, 0, false, 0, false, 0, false, 0,
            false, 0, false, 0, false, 0, false, 0, false, 0, false, 0);
        Unit unit = new Unit(90, 0, 100, 1, 100, 0, 100, 1, 100, 0, 80, 1, 100, 0, 110, 1);

        Newuser newuser = new Newuser(info, skill, stage, unit);
        

        string json = JsonUtility.ToJson(newuser);
        reference.Child("users").Child(Usersid).SetRawJsonValueAsync(json);

        Debug.Log(json);
    }

    //계정 로그인에 어떠한 변경점 발생 시 진입
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            //연동된 계정과 기기의 계정이 같다면 true 리턴
            signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if (!signedIn && user != null)
            {
                Debug.Log("Signed out" + user.UserId);
            }

            user = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("Signed in" + user.UserId);
            }
        }
    }
    public void BtnEvt_LoadStartScene()
    {
        NextScene();
    }
    private void NextScene()
    {
        LoadingSceneController.LoadScene("StartScene");
    }
}
