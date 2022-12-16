using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;



public class MainManager : MonoBehaviour
{
    [Header("변경되는 UI 요소")]
    [SerializeField] private Text[] nicknameText;
    [SerializeField] private Slider[] expSlider;
    [SerializeField] private Text[] levelText;
    [SerializeField] private Text[] expText;
    [SerializeField] private GameObject exitPopUpObj;

    private void Start()
    {
        ChangePlayerInfo();
    }
    public void BtnEvt_ExitGame()
    {
        exitPopUpObj.SetActive(!exitPopUpObj.activeSelf);
    }
    public void BtnEvt_Exit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    public void BtnEvt_ActiveObj(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }
    public void BtnEvt_LoadUnitScene()
    {
        LoadingSceneController.LoadScene("UnitShop");
    }
    public void BtnEvt_LoadSkillScene()
    {
        LoadingSceneController.LoadScene("Skill");
    }
    public void BtnEvt_LoadStageSectionScene()
    {
        LoadingSceneController.LoadScene("StageSection");
        SoundManager.Instance.SetBGM(2);
    }
    private void ChangePlayerInfo()
    {
        for (int i = 0; i < nicknameText.Length; i++)
        {
            nicknameText[i].text = GameManager.Instance.FirebaseData.InfoDictionary["Nickname"].ToString();
            levelText[i].text = GameManager.Instance.FirebaseData.InfoDictionary["Level"].ToString();
            expSlider[i].maxValue = DataEquation.PlayerMaxExpToLevel();
            expSlider[i].value = Convert.ToInt32(GameManager.Instance.FirebaseData.InfoDictionary["EXP"]);
            expText[i].text = $"{expSlider[i].value} / {expSlider[i].maxValue}";
        }
    }
    ////Auth용 instance
    //FirebaseAuth auth = null;

    ////사용자 계정
    //FirebaseUser user = null;

    ////로그인 패널
    //public GameObject LoginPanel;

    ////로딩 화면
    //public GameObject LoadingPanel;

    ////닉네임 설정 화면
    //public GameObject NicknamePanel;

    ////기기연동이 되어있는 상태인지 체크하는 변수
    //private bool signedIn = false;

    //public string FireBaseId = string.Empty;

    //public DatabaseReference reference;        //데이터베이스 reference

    //[SerializeField] public string Usersid;

    ////User user;

    //// 임시저장클래스
    ////private User.userLoginData.LoginType tempLoginType = User.userLoginData.LoginType.None;
    //private string tempemail = string.Empty;
    //private string temppw = string.Empty;


    //public bool StartTouch;
    //[SerializeField] private Text StartText;

    //public PlayerData playerdata;

    //private static string Nickname;
    //[SerializeField] private TextMeshProUGUI tNickname;
    //private string TankerPoints = string.Empty;
    //[SerializeField] private TextMeshProUGUI tTankerPoints;
    //private string WarriorPoints = string.Empty;
    //[SerializeField] private TextMeshProUGUI tWarriorPoints;
    //private string ADPoints = string.Empty;
    //[SerializeField] private TextMeshProUGUI tADPoints;
    //private string MagePoints = string.Empty;
    //[SerializeField] private TextMeshProUGUI tMagePoints;

    //private string Gold = string.Empty;
    //[SerializeField] private TextMeshProUGUI tGold;

    //[SerializeField] private Text tLv;
    //[SerializeField] private TextMeshProUGUI tEXP;
    //private string EXP = string.Empty;
    //private string Lv = string.Empty;

    //[SerializeField] private Image EXPbar;

    //[SerializeField] private GameObject UserInfoPanel;

    //DataSnapshot snapshot;

    //[SerializeField] private GameObject SettingPanel;

    //private void Awake()
    //{
    //    //초기화 auth
    //    auth = FirebaseAuth.DefaultInstance;

    //    //유저의 로그인 정보에 변경점을 체크하면 이벤트를 걸어줌
    //    auth.StateChanged += AuthStateChanged;
    //    //AuthStateChanged(this, null);

    //    //첫 시작시 Off
    //    LoginPanel.SetActive(false);
    //    LoadingPanel.SetActive(false);

    //    //Firebase reference 경로 설정
    //    FirebaseDatabase.GetInstance("https://acrobatgames-f9ba6-default-rtdb.firebaseio.com/");
    //    reference = FirebaseDatabase.DefaultInstance.RootReference;
    //}
    //void Start()
    //{
    //    PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
    //        .RequestServerAuthCode(false /* Don't force refresh */)
    //        .RequestIdToken()
    //        .RequestEmail()
    //        .EnableSavedGames()
    //        .Build();

    //    PlayGamesPlatform.InitializeInstance(config);
    //    PlayGamesPlatform.DebugLogEnabled = true;
    //    PlayGamesPlatform.Activate();
    //    //auth = FirebaseAuth.DefaultInstance;

    //    //StartTouch = true;
    //    //GameStart();

    //    //실험용

    //    //Usersid = "NaIxowYCsaSqdaYaWtWbIYErkqM2";

    //    reference.Child("users").Child(Usersid).Child("Info").GetValueAsync().ContinueWithOnMainThread(task =>
    //    {
    //        if (task.IsFaulted)
    //        {
    //            Debug.LogError("failed reading...");
    //        }
    //        else if (task.IsCompleted)
    //        {
    //            snapshot = task.Result;
    //            Debug.Log("데이터 접속");

    //            StartCoroutine(Startuiupdate());
    //        }
    //    });
    //}

    //private IEnumerator Startuiupdate()
    //{
    //    yield return null;

    //    if(snapshot == null)
    //    {
    //        Start();
    //    }

    //    Nickname = snapshot.Child("Nickname").Value.ToString();
    //    Lv = snapshot.Child("Level").Value.ToString();
    //    EXP = snapshot.Child("EXP").Value.ToString();

    //    Debug.Log(Nickname);

    //    //tNickname.text = Nickname;
    //    tLv.text = "Lv: " + Lv;
    //    tEXP.text = EXP + "/" + (800 + (int.Parse(Lv) / 5) * 100);

    //    Gold = snapshot.Child("Gold").Value.ToString();
    //    TankerPoints = snapshot.Child("Points").Child("TankerPoints").Value.ToString();
    //    WarriorPoints = snapshot.Child("Points").Child("WarriorPoints").Value.ToString();
    //    ADPoints = snapshot.Child("Points").Child("ADPoints").Value.ToString();
    //    MagePoints = snapshot.Child("Points").Child("MagePoints").Value.ToString();

    //    tGold.text = "Gold : " + Gold;
    //    tTankerPoints.text = "x " + TankerPoints;
    //    tWarriorPoints.text = "x " + WarriorPoints;
    //    tADPoints.text = "x " + ADPoints;
    //    tMagePoints.text = "x " + MagePoints;

    //    float EXPs = float.Parse(EXP);
    //    EXPbar.fillAmount = EXPs / 100f;

    //}

    //public void InfoOpen()
    //{
    //    UserInfoPanel.SetActive(true);
    //}

    //public void InfoClose()
    //{
    //    UserInfoPanel.SetActive(false);
    //}


    ////계정 로그인에 어떠한 변경점 발생 시 진입
    //void AuthStateChanged(object sender, System.EventArgs eventArgs)
    //{
    //    if(auth.CurrentUser != user)
    //    {
    //        //연동된 계정과 기기의 계정이 같다면 true 리턴
    //        signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

    //        if(!signedIn && user != null)
    //        {
    //            Debug.Log("Signed out" + user.UserId);
    //        }

    //        user = auth.CurrentUser;

    //        if (signedIn)
    //        {
    //            Debug.Log("Signed in" + user.UserId);
    //        }
    //    }
    //}

    ////로그인 선택패널을 열며 로그인 user가 있는지 확인
    ////없으면 계정 생성 시작
    //public void LoginCheck()
    //{
    //    //연동 상태가 아니라면
    //    if (!signedIn)
    //    {
    //        LoginPanel.SetActive(true);
    //    }
    //    else
    //    {
    //        //StartCoroutine(CurrentUserDataGet());
    //    }
    //}

    //private void Update()
    //{
    //    if(StartTouch == true && Input.GetMouseButton(0))
    //    {
    //        StartText.gameObject.SetActive(false);
    //        StartTouch = false;

    //        CheckUID();
    //    }
    //}

    ////파일저장소 확인 후, 로그인 기록 살펴서 로그인할지말지
    //private void CheckUID()
    //{
    //    if(File.Exists(Application.persistentDataPath + "/Userdata.json"))
    //    {
    //        string json = File.ReadAllText(Application.persistentDataPath + "/Userdata.json");
    //        playerdata = JsonUtility.FromJson<PlayerData>(json);

    //        Usersid = playerdata.UID;

    //        reference.Child("users").Child(Usersid).GetValueAsync().ContinueWithOnMainThread(task =>
    //        {
    //            if (task.IsFaulted)
    //            {
    //                Debug.LogError("failed reading...");
    //            }
    //            else if (task.IsCompleted)
    //            {
    //                DataSnapshot snapshot = task.Result;

    //                Debug.Log("CheckUID 파이어베이스 접근 완료");
    //            }
    //        });
    //    }
    //    else
    //    {
    //        LoginPanel.SetActive(true);
    //    }
    //}

    //public void GoogleLoginv()
    //{
    //    if (!Social.localUser.authenticated)
    //    {
    //        Social.localUser.Authenticate(success => // �α��� �õ�
    //        {
    //            if (success) // �����ϸ�
    //            {
    //                Debug.Log("TryFirebaseLogin �Լ� ����");
    //                //StartCoroutine(TryFirebaseLogin());
    //            }
    //            else // �����ϸ�
    //            {
    //                Debug.Log("����!");
    //            }
    //        });
    //    }
    //}

    //public void GoogleLogout()
    //{
    //    if (Social.localUser.authenticated)
    //    {
    //        PlayGamesPlatform.Instance.SignOut();
    //        auth.SignOut();
    //        Debug.Log("auth.SignOut �Ϸ�");
    //    }
    //}

    ////익명 로그인
    //public void GuestLogin()
    //{
    //    //LoadingPanel.SetActive(true);

    //    auth.SignInAnonymouslyAsync().ContinueWith(task => {
    //        if (task.IsCanceled)
    //        {
    //            Debug.LogError("SignInAnonymouslyAsync was canceled.");
    //            return;
    //        }
    //        if (task.IsFaulted)
    //        {
    //            Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
    //            return;
    //        }
    //        Debug.Log("익명 로그인 컴플리트");

    //        FirebaseUser newUser = task.Result;
    //        Debug.LogFormat("User signed in successfully: {0} ({1})",
    //            newUser.DisplayName, newUser.UserId);

    //        Usersid = newUser.UserId;
    //        //Debug.Log("writeNewUser �Լ� �ߵ�");
    //        //writeNewUser(newUser.UserId);

    //        Debug.Log("UIDRigister 작동");
    //        UIDRigister(newUser.UserId, "Guest");
    //        InitializeFirebase();
    //    });

    //    //신규 유저 데이터 임시 저장 함수
    //    userDataInit();
    //    //userDataTempSave(User.userLoginData.LoginType.anony);

    //    //LoadingPanel.SetActive(false);
    //}

    ////구글 로그인
    ///*public void GoogleLogin()
    //{
    //    //LoadingPanel.SetActive(true);

    //    try
    //    {
    //        //구글로그인 처리 부분
    //        //구글 로그인 팝업창이 꺼지면 실행할 콜백 함수 선언
    //        TryFirebaseLogin(new System.Action<bool>((bool chk) =>
    //        {
    //            if (chk)
    //            {
    //                //신규 유저 데이터 임시 저장
    //                userDataInit();
    //                userDataTempSave(User.userLoginData.LoginType.google);

    //                //LoadingPanel.SetActive(false);
    //                //닉네임 창 켜주기
    //                //NicknamePanel.SetActive(true);
    //            }
    //            else
    //            {
    //                LoadingPanel.SetActive(false);
    //            }
    //        }));
    //    }
    //    catch (System.Exception err)
    //    {
    //        Debug.LogError(err);
    //        LoadingPanel.SetActive(false);
    //    }
    //}*/


    ////구글 로그인 구동
    //public IEnumerator TryFirebaseLogin(System.Action<bool> callback)
    //{
    //    while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
    //        yield return null;

    //    string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
    //    string accessToken = null;

    //    Credential credential = GoogleAuthProvider.GetCredential(idToken, accessToken);
    //    auth.SignInWithCredentialAsync(credential).ContinueWith(task => { //ContinueWithOnMainThread
    //        if (task.IsCanceled)        //취소
    //        {
    //            Debug.LogError("SignInWithCredentialAsync was canceled.");
    //            callback(false);
    //            return;
    //        }
    //        if (task.IsFaulted)
    //        {
    //            Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
    //            callback(false);
    //            return;
    //        }

    //        user = task.Result;     //newuser 인식
    //        Debug.LogFormat("User signed in successfully: {0} ({1})",
    //            user.DisplayName, user.UserId);

    //        callback(true);

    //        Usersid = user.UserId;

    //        UIDRigister(user.UserId, "Google");
    //        InitializeFirebase();
    //    });
    //}

    ////유저 닉네임 입력 시작
    //private void InsertNewUserData()
    //{
    //    var nickname = NicknamePanel.transform.Find("InputField").Find("NickNameInput")
    //        .GetComponent<Text>().text;

    //    if(nickname.Length > 0)
    //    {
    //        //신규 유저 데이터 입력
    //        //loginDataSave(tempLoginType, nickname, tempemail, temppw);
    //    }
    //    else
    //    {
    //        Debug.Log("닉네임을 입력해주세요");
    //        return;
    //    }
    //}

    ////유저데이터 임시 초기화
    //private void userDataInit()
    //{
    //    //tempLoginType = User.userLoginData.LoginType.None;
    //    tempemail = string.Empty;
    //    temppw = string.Empty;
    //}

    //// 유저데이터 임시 저장
    ///*private void userDataTempSave(User.userLoginData.LoginType loginType, string email = null, string pw = null)
    //{
    //    tempLoginType = loginType;
    //    tempemail = email;
    //    temppw = pw;
    //}*/

    ////신규 유저 데이터 입력
    ///*private void loginDataSave(User.userLoginData.LoginType loginType, string nickname = null,
    //    string email = null, string pw = null)
    //{
    //    // ���� ������
    //    var newUser = new User.userLoginData();

    //    // DB�� ������ ���������� �ʱ�ȭ
    //    newUser.loginType = tempLoginType;
    //    newUser.nickname = nickname;
    //    newUser.uid = user.UserId;
    //    newUser.email = email;
    //    newUser.pw = pw;
    //    newUser.deviceModel = SystemInfo.deviceModel;
    //    newUser.deviceName = SystemInfo.deviceName;
    //    newUser.deviceType = SystemInfo.deviceType;
    //    newUser.deviceOS = SystemInfo.operatingSystem;
    //    newUser.createDate = auth.CurrentUser.Metadata.CreationTimestamp;
    //    string json = JsonUtility.ToJson(newUser);


    //    // ���� ������
    //    var newUserInventory = new User.userGoodsData();

    //    // DB�� ������ ���������� �ʱ�ȭ
    //    newUserInventory.uid = user.UserId;
    //    newUserInventory.coin = 0;

    //    LoadingPanel.SetActive(true);
    //    LoginPanel.SetActive(false);
    //    NicknamePanel.SetActive(false);

    //    // ���� ������ json�� ������ ���� DB�� �����Ѵ�.
    //    // ���ο� ������ ���� �����͸� DB�� ������.
    //   /* Server.Instance.NewUserJsonDBSave(json, () => {
    //        // DB�� ���� �� �����̽� user�������� �����Ѵ�.
    //        User.Instance.mainUser = newUser;

    //        // ���ο� ������ �κ��丮 �����͸� DB�� ������.
    //        Server.Instance.NewUserInventoryJsonDBSave(JsonUtility.ToJson(newUserInventory), () => {
    //            // DB�� ���� �� �����̽� user�������� �����Ѵ�.
    //            User.Instance.mainInventory = newUserInventory;
    //            // ���������� �̵�
    //            NextSecne();
    //        });
    //    });
    //}*/

    //public void GuestLogout()
    //{
    //    if(auth == null)
    //    {
    //        Debug.Log("auth ������");
    //    }
    //    else
    //        auth.SignOut();
    //}

    //public void GoBattle()
    //{
    //    LoadingSceneController.LoadScene("StageSection");
    //}


    //private void InitializeFirebase()
    //{
    //    reference.GetValueAsync().ContinueWithOnMainThread(task =>
    //    {
    //        if (task.IsCompleted)
    //        {
    //            DataSnapshot snapshot = task.Result;
    //            Debug.Log("InitializeFirebase ���ٿϷ�");

    //            foreach (DataSnapshot data in snapshot.Children)
    //            {
    //                Debug.LogFormat("[Database] key : {0}, value :{1}", data.Key, data.Value);
    //            }
    //        }
    //    });
    //}

    //public void UIDRigister(string uid, string LastLogin)
    //{
    //    Debug.Log("UIDRigister 함수 실행");

    //    if(File.Exists(Application.persistentDataPath + "/Userdata.json"))  //�̹� ���� �����Ͱ� ���� �����ҿ�
    //    {                                                                   //���� �Ѵٸ�
    //        Debug.Log("UID인식 후 데이터 접근");

    //        string json = File.ReadAllText(Application.persistentDataPath + "/Userdata.json");
    //        //userdata = JsonUtility.FromJson<UserData>(json);
    //    }
    //    else
    //    {
    //        Debug.Log("UID 기초값 생성");

    //        playerdata.UID = uid;

    //        playerdata.Gold = 0;
    //        playerdata.SkillPoints = 0;
    //        playerdata.UnitPoints = 0;




    //        string json = JsonUtility.ToJson(playerdata);
    //        string key = Usersid;

    //        File.WriteAllText(Application.persistentDataPath + "/Userdata.json", JsonUtility.ToJson(playerdata, true));
    //        reference.Child("users").Child(key).SetRawJsonValueAsync(json);
    //    }

    //    /*Debug.Log("UID �����ϱ�");

    //    userdata.user_name = "������";
    //    userdata.Area1 = false;
    //    userdata.Area2 = false;
    //    userdata.Area3 = false;
    //    userdata.Area4 = false;
    //    userdata.A1Stage1_achievement = 0;
    //    userdata.A1Stage2_achievement = 0;
    //    userdata.A1Stage3_achievement = 0;
    //    userdata.Skill1_Level = 1;
    //    userdata.Skill2_Level = 1;
    //    userdata.Skill3_Level = 1;
    //    userdata.uid = uid;

    //    string json = JsonUtility.ToJson(userdata, true);
    //    string key = uid;

    //    reference.Child("users").Child(key).SetRawJsonValueAsync(json);

    //    Debug.Log(json);*/
    //}

    ////리딩 슈타이너
    //public void ReadingDbUserInfo()
    //{
    //    Debug.Log("리딩 슈타이너 발동");
    //    reference.Child("users").Child(Usersid).GetValueAsync().ContinueWithOnMainThread(task =>
    //    {
    //        if (task.IsFaulted)
    //        {
    //            Debug.LogError("Error Database");
    //        }
    //        else if (task.IsCompleted)
    //        {
    //            Debug.Log("리딩슈타이너 컴플릿");

    //            DataSnapshot snapshot = task.Result;

    //            //������ ������ snapshot�� json���� ��ȯ ����
    //            //string json = JsonUtility.ToJson(snapshot);

    //            foreach (DataSnapshot userdata in snapshot.Children)
    //            {
    //                Debug.Log(userdata.Value);
    //            }
    //        }
    //    });
    //}

    ////���۰������� ��ȯ
    //public void onAnonyToGoogle()
    //{
    //    Social.localUser.Authenticate((bool success) =>
    //    {
    //        if (success)
    //        {
    //            string authCode = PlayGamesPlatform.Instance.GetServerAuthCode();

    //            FirebaseAuth auth = FirebaseAuth.DefaultInstance;
    //            Credential credential = PlayGamesAuthProvider.GetCredential(authCode);

    //            if (auth.CurrentUser != null)
    //            {
    //                auth.CurrentUser.LinkWithCredentialAsync(credential).ContinueWith(task =>
    //                {
    //                    if (task.IsCanceled)
    //                    {
    //                        Debug.LogError("LinkWithCredentialAsync was canceled.");
    //                        return;
    //                    }

    //                    if (task.IsFaulted)
    //                    {
    //                        Debug.LogError("LinkWithCredentialAsync encountered an error: " + task.Exception);
    //                        return;
    //                    }

    //                    FirebaseUser newUser = task.Result;
    //                    Debug.LogFormat("User signed in successfully: {0} ({1})",
    //  newUser.DisplayName, newUser.UserId);
    //                });
    //            }
    //        }
    //    });
    //}

    //Shop������ �̵�

    //public void GoSetting()
    //{
    //    SettingPanel.SetActive(true);
    //}

    //public void SettingClose()
    //{
    //    SettingPanel.SetActive(false);
    //}


    public void SaveData()
    {
        /*if (!Directory.Exists(Application.persistentDataPath + "/������ ���� �̸�"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/������ ���� �̸�");
        }*/

        /*[�ȵ����̵� External]
        Application.persistentDataPath : / mnt / sdcard / Android / data / com.YourProductName.YourCompanyName / files[���� �б� / ���� ����]
        Application.dataPath : / data / app / �����̸� - ��ȣ.apk
        Application.streamingAssetsPath : jar: file:///data/app/�����̸�.apk!/assets [������ �ƴ� WWW�� �б� ����]

        [�ȵ����̵� Internal]
        Application.persistentDataPath : / Android / data / com.YourProductName.YourCompanyName / files[���� �б� / ���� ����]
        Application.dataPath : / data / app / �����̸� - ��ȣ.apk
        Application.streamingAssetsPath : jar: file:///data/app/�����̸�.apk!/assets [������ �ƴ� WWW�� �б� ����]*/
    }
}
