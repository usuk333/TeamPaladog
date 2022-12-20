using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

using UnityEngine.SceneManagement;

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

public class GameManager : MonoBehaviour
{
    public class Info
    {
        public int ArchorPoints;
        public int AssassinPoints;
        public int EXP;
        public int Gold;
        public int Level = 1;
        public int MagicianPoints;
        public string Nickname;
        public string UID;
        public int WarriorPoints;

        public Info(string uID, string nickname)
        {
            UID = uID;
            Nickname = nickname;
        }
    }

    public class Skill
    {
        public int HP = 1;
        public int MP = 1;
        public int Attack = 1;
        public int Barrior = 1;
        public int Heal = 1;
        public int PowerUp = 1;
        public int SkillPoints;
    }

    public class Stage
    {
        public bool S1EClear;
        public int S1EStar;
        public bool S1HClear;
        public int S1HStar;
        public bool S1NClear;
        public int S1NStar;

        public bool S2EClear;
        public int S2EStar;
        public bool S2HClear;
        public int S2HStar;
        public bool S2NClear;
        public int S2NStar;

        public bool S3EClear;
        public int S3EStar;
        public bool S3HClear;
        public int S3HStar;
        public bool S3NClear;
        public int S3NStar;

        public bool S4EClear;
        public int S4EStar;
        public bool S4HClear;
        public int S4HStar;
        public bool S4NClear;
        public int S4NStar;
    }

    public class Unit
    {
        public int ArchorATK = 380;
        public int ArchorEXP;
        public int ArchorHP = 800;
        public int ArchorLevel = 1;
        public int AssassinATK = 400;
        public int AssassinEXP;
        public int AssassinHP = 1000;
        public int AssassinLevel =1;
        public int MagicianATK= 400;
        public int MagicianEXP;
        public int MagicianHP = 800;
        public int MagicianLevel =1 ;
        public int WarriorATK = 380;
        public int WarriorEXP;
        public int WarriorHP = 1200;
        public int WarriorLevel = 1;
    }

    private string version;

    private static GameManager instance;

    public static GameManager Instance { get => instance; }

    //Auth용 instance
    FirebaseAuth auth = null;

    //사용자 계정
    FirebaseUser user = null;

    //기기연동이 되어있는 상태인지 체크하는 변수
    private bool signedIn = false;

    public string FireBaseId = string.Empty;

    private char[] notFullKoreanArray = { 'ㄱ', 'ㄴ', 'ㄷ', 'ㄹ', 'ㅁ', 'ㅂ', 'ㅅ', 'ㅇ', 'ㅈ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };
    private string[] swearWordArray = { "간나", "갈보", "년", "놈", "느개비", "느금마", "니미", "니기미", "닥쳐", "등신", "또라이", "똘추", "미친", "창녀", "창놈", 
        "병신", "보지", "불알", "부랄", "빨통", "새끼", "씨발", "씹", "새끼", "엠창", "육변기", "자지", "젠장", "좆", "지랄" };


    public PlayerData playerdata;

    [SerializeField] private GameObject[] createAccountPopUpObj;


    //데이터베이스 reference
    public DatabaseReference reference;

    //테스트
    [SerializeField] private string Usersid;

    private FirebaseData firebaseData;

    public FirebaseData FirebaseData { get => firebaseData; }
    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(instance);

        version = Application.version;

        //초기화 auth
        auth = FirebaseAuth.DefaultInstance;

        //유저의 로그인 정보에 변경점을 체크하면 이벤트를 걸어줌
        //auth.StateChanged += AuthStateChanged;
        //AuthStateChanged(this, null);
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        .RequestServerAuthCode(false /* Don't force refresh */)
        .RequestIdToken()
        .RequestEmail()
        .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        StartCoroutine(Co_WarningMessage(Social.localUser.authenticated.ToString()));

        //첫 시작시 Off
        //LoginPanel.SetActive(false);
        //LoadingPanel.SetActive(false);

        //Firebase reference 경로 설정
        FirebaseDatabase.GetInstance("https://acrobatgames-f9ba6-default-rtdb.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.GetReference("users");

        //Debug.Log(reference.Child("Version").GetValueAsync().Result.Value);
    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //구글 로그인 버튼
    public void GoogleLoginBtn()
    {
        GoogleLogin();
        return;
        //연동 상태가 아니라면
        if (!signedIn)
        {
            StartCoroutine(Co_WarningMessage("구글 계정이 연동되어 있지 않습니다!"));
        }
        else
        {
            GoogleLogin();
        }
    }


    private void GoogleLogin()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success) StartCoroutine(TryFirebaseLogin());
            else StartCoroutine(Co_WarningMessage("구글 연동 실패!"));
        });
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

            PlayerDataSet(Usersid);
            //UIDRigister(user.UserId, "Google");
            //InitializeFirebase();
        });
    }

    private void PlayerDataSet(string uid)
    {

        reference.Child(uid).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if(task.Result.Value != null) //기존 데이터 확인
            {
                if (task.IsCompleted)   
                {
                    Debug.Log("이미 DB에 있는 아이디");
                    firebaseData = new FirebaseData(uid);
                    StartCoroutine(firebaseData.Co_InitData());
                    StartCoroutine(Co_InitData());
                    Debug.Log(task.Result);
                }
                else if (task.IsFaulted)                 
                {
                    Debug.Log("데이터 로딩 실패");
                }
                else if (task.IsCanceled)
                {
                    Debug.Log("DB 접근 취소");
                }
            }
            else //기존 데이터가 없고, 기초 데이터 설치
            {
                if (task.IsCompleted)  
                {
                    Debug.Log("새로 생성한 아이디");
                    createAccountPopUpObj[0].SetActive(true);
                    SetInputField();
                }
                else if (task.IsFaulted)                  
                {
                    Debug.Log("데이터 로딩 실패");
                }
                else if (task.IsCanceled)
                {
                    Debug.Log("DB 접근 취소");
                    return;
                }
            }
        });
        //NextScene();
    }
    private IEnumerator Co_InitData()
    {
        yield return new WaitUntil(() => firebaseData.dataLoadComplete);

        Debug.Log("데이터 로딩 완료");
        LoadingSceneController.LoadScene("Main");
        SoundManager.Instance.SetBGM(1);
    }
    private void CreateNewUserData(string uid, string nickname)
    {
        Info info = new Info(uid, nickname);
        Skill skill = new Skill();
        Stage stage = new Stage();
        Unit unit = new Unit();

        string infoJson = JsonUtility.ToJson(info);
        string skillJson = JsonUtility.ToJson(skill);
        string stageJson = JsonUtility.ToJson(stage);
        string unitJson = JsonUtility.ToJson(unit);

        reference.Child(uid).Child("Info").SetRawJsonValueAsync(infoJson);
        reference.Child(uid).Child("Skill").SetRawJsonValueAsync(skillJson);
        reference.Child(uid).Child("Stage").SetRawJsonValueAsync(stageJson);
        reference.Child(uid).Child("Unit").SetRawJsonValueAsync(unitJson);

        reference.Child("AllNicknames").Child(nickname).SetValueAsync(true);

        firebaseData = new FirebaseData(uid);

        StartCoroutine(firebaseData.Co_InitData());

        StartCoroutine(Co_InitData());
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
    public void BtnEvt_CreateNewAccount()
    {
        CreateAccount();
    }
    private void SetInputField()
    {
        InputField input = createAccountPopUpObj[0].transform.GetComponentInChildren<InputField>();
        input.onValueChanged.AddListener((word) => input.text = Regex.Replace(word, @"[^ㄱ-ㅎ가-힣]", ""));
    }
    private void CreateAccount()
    {
        InputField input = createAccountPopUpObj[0].transform.GetComponentInChildren<InputField>();
        if (input.text.Length < 2)
        {
            StartCoroutine(Co_WarningMessage("닉네임은 2글자 이상이어야 합니다!"));
            return;
        }

        foreach (var item in notFullKoreanArray)
        {
            if (input.text.Contains(item.ToString()))
            {
                StartCoroutine(Co_WarningMessage("잘못된 닉네임입니다!"));
                return;
            }
        }
        foreach (var item in swearWordArray)
        {
            if (input.text.Contains(item))
            {
                StartCoroutine(Co_WarningMessage("잘못된 닉네임입니다!"));
                return;
            }
        }
        reference.Child("AllNicknames").Child(input.text).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                if(task.Result.Value != null)
                {
                    Debug.Log(task.Result.Value);
                    StartCoroutine(Co_WarningMessage("이미 존재하는 닉네임입니다!"));
                }
                else
                {
                    CreateNewUserData(Usersid, input.text);
                    createAccountPopUpObj[0].SetActive(false);
                }
            }
            else if (task.IsFaulted)
            {
                Debug.Log("DB 연결 실패");
            }
        });
    }
    private IEnumerator Co_WarningMessage(string message)
    {
        createAccountPopUpObj[1].GetComponentInChildren<Text>().text = message;
        createAccountPopUpObj[1].SetActive(true);
        yield return new WaitForSeconds(1f);
        createAccountPopUpObj[1].SetActive(false);
    }
    public void BtnEvt_LoginTest()
    {
        PlayerDataSet(Usersid);
    }

}
