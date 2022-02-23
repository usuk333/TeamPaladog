using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

using Google;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

using UnityEngine.SceneManagement;
using System.IO;
using System.Threading.Tasks;


public class GameCenterManager : MonoBehaviour
{
    //Auth용 instance
    FirebaseAuth auth = null;

    //사용자 계정
    FirebaseUser user = null;

    //로그인 패널
    public GameObject LoginPanel;

    //로딩 화면
    public GameObject LoadingPanel;

    //닉네임 설정 화면
    public GameObject NicknamePanel;

    //기기연동이 되어있는 상태인지 체크하는 변수
    private bool signedIn = false;

    public string FireBaseId = string.Empty;

    public DatabaseReference reference;        //데이터베이스 reference

    [SerializeField] public string Usersid;

    //User user;

    // 임시저장클래스
    private User.userLoginData.LoginType tempLoginType = User.userLoginData.LoginType.None;
    private string tempemail = string.Empty;
    private string temppw = string.Empty;


    public bool StartTouch;
    [SerializeField] private Text StartText;

    public PlayerData playerdata;

    [SerializeField] private Text ULV;
    [SerializeField] private Text UEXP;

    private string UEXPs;
    private string ULVs;

    private void Awake()
    {
        //초기화 auth
        auth = FirebaseAuth.DefaultInstance;

        //유저의 로그인 정보에 변경점을 체크하면 이벤트를 걸어줌
        auth.StateChanged += AuthStateChanged;
        //AuthStateChanged(this, null);

        //첫 시작시 Off
        LoginPanel.SetActive(false);
        LoadingPanel.SetActive(false);

        //Firebase reference 경로 설정
        FirebaseDatabase.GetInstance("https://acrobatgames-f9ba6-default-rtdb.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .RequestServerAuthCode(false /* Don't force refresh */)
            .RequestIdToken()
            .RequestEmail()
            .EnableSavedGames()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        //auth = FirebaseAuth.DefaultInstance;

        //StartTouch = true;
        //GameStart();

        //실험용

        //Usersid = "NaIxowYCsaSqdaYaWtWbIYErkqM2";

        reference.Child("users").Child(Usersid).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("failed reading...");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                Debug.Log("���ø�����");

                foreach (DataSnapshot data in snapshot.Children)
                {
                    //IDictionary userinfo = (IDictionary)data.Value;
                    Debug.LogFormat("[Database] key : {0}, value :{1}", data.Key, data.Value);
                    if(data.Key == "LV")
                    {
                        Debug.Log("LV," + data.Value);
                        ULVs = data.Value.ToString();
                        ULV.text = "LV : " + ULVs;
                    }
                }
            }
        });
    }



    //계정 로그인에 어떠한 변경점 발생 시 진입
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if(auth.CurrentUser != user)
        {
            //연동된 계정과 기기의 계정이 같다면 true 리턴
            signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if(!signedIn && user != null)
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

    //로그인 선택패널을 열며 로그인 user가 있는지 확인
    //없으면 계정 생성 시작
    public void LoginCheck()
    {
        //연동 상태가 아니라면
        if (!signedIn)
        {
            LoginPanel.SetActive(true);
        }
        else
        {
            StartCoroutine(CurrentUserDataGet());
        }
    }

    //기존 유저 정보 서버에서 가져오기
    public IEnumerator CurrentUserDataGet()
    {
        LoadingPanel.SetActive(true);
        /*
        // ���� ����
        User.Instance.GetUserData(auth.CurrentUser.UserId, new System.Action(() => {
            Debug.Log("���� ���� �ε� �Ϸ�!");
            // ���� �κ� ����
            User.Instance.GetUserInven(auth.CurrentUser.UserId, new System.Action(() => {
                // ���� ������ �ѱ���.
                NextSecne();
            }));
        }));
        */
        yield return null;
    }

    //게임 씬으로 넘어가기
    public void NextScene()
    {
        Debug.Log("GameScene����");

        SceneManager.LoadSceneAsync(1);
    }

    private void Update()
    {
        if(StartTouch == true && Input.GetMouseButton(0))
        {
            StartText.gameObject.SetActive(false);
            StartTouch = false;

            CheckUID();
        }
    }

    private void GameStart()
    {
        if(StartTouch == true)
        {
            StartText.gameObject.SetActive(true);
        }
    }

    private void CheckUID()
    {
        if(File.Exists(Application.persistentDataPath + "/Userdata.json"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/Userdata.json");
            //userdata = JsonUtility.FromJson<UserData>(json);

            Usersid = playerdata.UID;

            reference.Child("users").Child(Usersid).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("failed reading...");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    Debug.Log("InitializeFirebase ���ٿϷ�");

                    foreach (DataSnapshot data in snapshot.Children)
                    {
                        //IDictionary userinfo = (IDictionary)data.Value;
                        //���ųʸ� �����ؾ��ҵ�. ���ذ� �ȴ�
                        Debug.LogFormat("[Database] key : {0}, value :{1}", data.Key, data.Value);
                    }
                }
            });
        }
        else
        {
            LoginPanel.SetActive(true);
        }
    }

    public void GoogleLoginv()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate(success => // �α��� �õ�
            {
                if (success) // �����ϸ�
                {
                    Debug.Log("TryFirebaseLogin �Լ� ����");
                    //StartCoroutine(TryFirebaseLogin());
                }
                else // �����ϸ�
                {
                    Debug.Log("����!");
                }
            });
        }
    }

    public void GoogleLogout()
    {
        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.SignOut();
            auth.SignOut();
            Debug.Log("auth.SignOut �Ϸ�");
        }
    }

    //익명 로그인
    public void GuestLogin()
    {
        //LoadingPanel.SetActive(true);

        auth.SignInAnonymouslyAsync().ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }
            Debug.Log("익명 로그인 컴플리트");

            FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            Usersid = newUser.UserId;
            //Debug.Log("writeNewUser �Լ� �ߵ�");
            //writeNewUser(newUser.UserId);

            Debug.Log("UIDRigister 작동");
            UIDRigister(newUser.UserId, "Guest");
            InitializeFirebase();
        });

        //신규 유저 데이터 임시 저장 함수
        userDataInit();
        userDataTempSave(User.userLoginData.LoginType.anony);

        //LoadingPanel.SetActive(false);
    }

    //구글 로그인
    public void GoogleLogin()
    {
        //LoadingPanel.SetActive(true);

        try
        {
            //구글로그인 처리 부분
            //구글 로그인 팝업창이 꺼지면 실행할 콜백 함수 선언
            TryFirebaseLogin(new System.Action<bool>((bool chk) =>
            {
                if (chk)
                {
                    //신규 유저 데이터 임시 저장
                    userDataInit();
                    userDataTempSave(User.userLoginData.LoginType.google);

                    //LoadingPanel.SetActive(false);
                    //닉네임 창 켜주기
                    //NicknamePanel.SetActive(true);
                }
                else
                {
                    LoadingPanel.SetActive(false);
                }
            }));
        }
        catch (System.Exception err)
        {
            Debug.LogError(err);
            LoadingPanel.SetActive(false);
        }
    }


    //구글 로그인 구동
    public IEnumerator TryFirebaseLogin(System.Action<bool> callback)
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
                callback(false);
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                callback(false);
                return;
            }

            user = task.Result;     //newuser 인식
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                user.DisplayName, user.UserId);

            callback(true);

            Usersid = user.UserId;

            UIDRigister(user.UserId, "Google");
            InitializeFirebase();
        });
    }

    //유저 닉네임 입력 시작
    private void InsertNewUserData()
    {
        var nickname = NicknamePanel.transform.Find("InputField").Find("NickNameInput")
            .GetComponent<Text>().text;

        if(nickname.Length > 0)
        {
            //신규 유저 데이터 입력
            //loginDataSave(tempLoginType, nickname, tempemail, temppw);
        }
        else
        {
            Debug.Log("������ �Է����ּ���");
            return;
        }
    }

    //유저데이터 임시 초기화
    private void userDataInit()
    {
        tempLoginType = User.userLoginData.LoginType.None;
        tempemail = string.Empty;
        temppw = string.Empty;
    }

    // 임시 저장
    private void userDataTempSave(User.userLoginData.LoginType loginType, string email = null, string pw = null)
    {
        tempLoginType = loginType;
        tempemail = email;
        temppw = pw;
    }

    // �ű� ���� ������ �Է�
    /*private void loginDataSave(User.userLoginData.LoginType loginType, string nickname = null,
        string email = null, string pw = null)
    {
        // ���� ������
        var newUser = new User.userLoginData();

        // DB�� ������ ���������� �ʱ�ȭ
        newUser.loginType = tempLoginType;
        newUser.nickname = nickname;
        newUser.uid = user.UserId;
        newUser.email = email;
        newUser.pw = pw;
        newUser.deviceModel = SystemInfo.deviceModel;
        newUser.deviceName = SystemInfo.deviceName;
        newUser.deviceType = SystemInfo.deviceType;
        newUser.deviceOS = SystemInfo.operatingSystem;
        newUser.createDate = auth.CurrentUser.Metadata.CreationTimestamp;
        string json = JsonUtility.ToJson(newUser);


        // ���� ������
        var newUserInventory = new User.userGoodsData();

        // DB�� ������ ���������� �ʱ�ȭ
        newUserInventory.uid = user.UserId;
        newUserInventory.coin = 0;

        LoadingPanel.SetActive(true);
        LoginPanel.SetActive(false);
        NicknamePanel.SetActive(false);

        // ���� ������ json�� ������ ���� DB�� �����Ѵ�.
        // ���ο� ������ ���� �����͸� DB�� ������.
       /* Server.Instance.NewUserJsonDBSave(json, () => {
            // DB�� ���� �� �����̽� user�������� �����Ѵ�.
            User.Instance.mainUser = newUser;

            // ���ο� ������ �κ��丮 �����͸� DB�� ������.
            Server.Instance.NewUserInventoryJsonDBSave(JsonUtility.ToJson(newUserInventory), () => {
                // DB�� ���� �� �����̽� user�������� �����Ѵ�.
                User.Instance.mainInventory = newUserInventory;
                // ���������� �̵�
                NextSecne();
            });
        });
    }*/

    public void GuestLogout()
    {
        if(auth == null)
        {
            Debug.Log("auth ������");
        }
        else
            auth.SignOut();
    }

    public void GoBattle()
    {
        SceneManager.LoadScene("Stage");
    }


    public class MyUserData //������
    {
        public string UID;

        //Goods
        public int Gold;
        public int SkillPoints;
        public int UnitPoints;

        //Skill
        //Skill1
        public int Skill1_Level;
        public bool Skill1_Unlock;
        //Skill2
        public int Skill2_Level;
        public bool Skill2_Unlock;
        //Skill3
        public int Skill3_Level;
        public bool Skill3_Unlock;
        //Skill4
        public int Skill4_Level;
        public bool Skill4_Unlock;
        //Skill5
        public int Skill5_Level;
        public bool Skill5_Unlock;
        //Skill6
        public int Skill6_Level;
        public bool Skill6_Unlock;
        //Skill7
        public int Skill7_Level;
        public bool Skill7_Unlock;

        //Stage
        //Stage1
        public bool S1EClear;
        public bool S1NClear;
        public bool S1HClear;
        //Stage2
        public bool S2EClear;
        public bool S2NClear;
        public bool S2HClear;
        //Stage3
        public bool S3EClear;
        public bool S3NClear;
        public bool S3HClear;
        //Stage4
        public bool S4EClear;
        public bool S4NClear;
        public bool S4HClear;
        //Stage5
        public bool S5EClear;
        public bool S5NClear;
        public bool S5HClear;
        //Stage6
        public bool S6EClear;
        public bool S6NClear;
        public bool S6HClear;
        //Stage7
        public bool S7EClear;
        public bool S7NClear;
        public bool S7HClear;
        //Stage8
        public bool S8EClear;
        public bool S8NClear;
        public bool S8HClear;

        //Status
        public int Level;
        public int EXP;
        public float Speed;
        public float HP;
        public float MP;
        public float ATKPower;

        //Unit
        //Warrior
        public int Warrior_Level;
        public bool Warrior_Unlock;
        //Assassin
        public int Assassin_Level;
        public bool Assassin_Unlock;
        //Druid
        public int Druid_Level;
        public bool Druid_Unlock;
        //Shielder
        public int Shielder_Level;
        public bool Shielder_Unlock;
        //Archo
        public int Archor_Level;
        public bool Archor_Unlock;
        //Mechanic
        public int Mechanic_Level;
        public bool Mechanic_Unlock;
        //Magician
        public int Magician_Level;
        public bool Magician_Unlock;
        //Specialist
        public int Specialist_Level;
        public bool Specialist_Unlock;
    }


    private void InitializeFirebase()
    {
        reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log("InitializeFirebase ���ٿϷ�");

                foreach (DataSnapshot data in snapshot.Children)
                {
                    Debug.LogFormat("[Database] key : {0}, value :{1}", data.Key, data.Value);
                }
            }
        });
    }

    public void UIDRigister(string uid, string LastLogin)
    {
        Debug.Log("UIDRigister 함수 실행");

        if(File.Exists(Application.persistentDataPath + "/Userdata.json"))  //�̹� ���� �����Ͱ� ���� �����ҿ�
        {                                                                   //���� �Ѵٸ�
            Debug.Log("UID인식 후 데이터 접근");

            string json = File.ReadAllText(Application.persistentDataPath + "/Userdata.json");
            //userdata = JsonUtility.FromJson<UserData>(json);
        }
        else
        {
            Debug.Log("UID 기초값 생성");

            playerdata.UID = uid;
            playerdata.LastLogin = LastLogin;

            playerdata.Gold = 0;
            playerdata.SkillPoints = 0;
            playerdata.UnitPoints = 0;

            /*playerdata.Skill1_Level = 1;
            playerdata.Skill1_Unlock = true;
            playerdata.Skill2_Level = 1;
            playerdata.Skill2_Unlock = false;
            playerdata.Skill3_Level = 1;
            playerdata.Skill3_Unlock = false;
            playerdata.Skill4_Level = 1;
            playerdata.Skill4_Unlock = false;
            playerdata.Skill5_Level = 1;
            playerdata.Skill5_Unlock = false;
            playerdata.Skill6_Level = 1;
            playerdata.Skill6_Unlock = false;
            playerdata.Skill7_Level = 1;
            playerdata.Skill7_Unlock = false;

            playerdata.S1EClear = false;
            playerdata.S1HClear = false;
            playerdata.S1NClear = false;

            playerdata.S2EClear = false;
            playerdata.S2HClear = false;
            playerdata.S2NClear = false;

            playerdata.S3EClear = false;
            playerdata.S3HClear = false;
            playerdata.S3NClear = false;

            playerdata.S4EClear = false;
            playerdata.S4HClear = false;
            playerdata.S4NClear = false;

            playerdata.S5EClear = false;
            playerdata.S5HClear = false;
            playerdata.S5NClear = false;

            playerdata.S6EClear = false;
            playerdata.S6HClear = false;
            playerdata.S6NClear = false;

            playerdata.S7EClear = false;
            playerdata.S7HClear = false;
            playerdata.S7NClear = false;

            playerdata.S8EClear = false;
            playerdata.S8HClear = false;
            playerdata.S8NClear = false;

            playerdata.ATKPower = 1;
            playerdata.EXP = 0;
            playerdata.HP = 100;
            playerdata.Level = 1;
            playerdata.MP = 1;
            playerdata.Speed = 5;

            playerdata.Warrior_Level = 1;
            playerdata.Warrior_Unlock = true;

            playerdata.Assassin_Level = 1;
            playerdata.Assassin_Unlock = false;

            playerdata.Shielder_Level = 1;
            playerdata.Shielder_Unlock = true;

            playerdata.Druid_Level = 1;
            playerdata.Druid_Unlock = false;

            playerdata.Archor_Level = 1;
            playerdata.Archor_Unlock = true;

            playerdata.Mechanic_Level = 1;
            playerdata.Mechanic_Unlock = false;

            playerdata.Magician_Level = 1;
            playerdata.Magician_Unlock = true;

            playerdata.Specialist_Level = 1;
            playerdata.Specialist_Unlock = false;*/


            string json = JsonUtility.ToJson(playerdata);
            string key = Usersid;

            File.WriteAllText(Application.persistentDataPath + "/Userdata.json", JsonUtility.ToJson(playerdata, true));
            reference.Child("users").Child(key).SetRawJsonValueAsync(json);
        }

        /*Debug.Log("UID �����ϱ�");

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
        string key = uid;

        reference.Child("users").Child(key).SetRawJsonValueAsync(json);

        Debug.Log(json);*/
    }

    /*public void UpdateDbUserInfo()
    {
        Debug.LogFormat("[Database] insert !");
        User users = new User();

        users.user_name = "�� �̸��� ������";
        users.Skill1_Level = 1;
        users.Skill2_Level = 1;
        users.Skill3_Level = 1;
        users.Area1 = false;
        users.Area2 = false;
        users.Area3 = false;
        users.Area4 = false;
        users.A1Stage1_achievement = 0;
        users.A1Stage2_achievement = 0;
        users.A1Stage3_achievement = 0;

        string json = JsonUtility.ToJson(users);

        string key = Usersid;
        reference.Child("users").Child(key).SetRawJsonValueAsync(json);
    }*/

    //���۷��� ������ �о�����
    public void ReadingDbUserInfo()
    {
        Debug.Log("리딩 슈타이너 발동");
        reference.Child("users").Child(Usersid).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error Database");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("리딩슈타이너 컴플릿");

                DataSnapshot snapshot = task.Result;

                //������ ������ snapshot�� json���� ��ȯ ����
                //string json = JsonUtility.ToJson(snapshot);

                foreach (DataSnapshot userdata in snapshot.Children)
                {
                    Debug.Log(userdata.Value);
                }
            }
        });
    }

    //���۰������� ��ȯ
    public void onAnonyToGoogle()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                string authCode = PlayGamesPlatform.Instance.GetServerAuthCode();

                FirebaseAuth auth = FirebaseAuth.DefaultInstance;
                Credential credential = PlayGamesAuthProvider.GetCredential(authCode);

                if (auth.CurrentUser != null)
                {
                    auth.CurrentUser.LinkWithCredentialAsync(credential).ContinueWith(task =>
                    {
                        if (task.IsCanceled)
                        {
                            Debug.LogError("LinkWithCredentialAsync was canceled.");
                            return;
                        }

                        if (task.IsFaulted)
                        {
                            Debug.LogError("LinkWithCredentialAsync encountered an error: " + task.Exception);
                            return;
                        }

                        FirebaseUser newUser = task.Result;
                        Debug.LogFormat("User signed in successfully: {0} ({1})",
      newUser.DisplayName, newUser.UserId);
                    });
                }
            }
        });
    }

    //Shop������ �̵�
    public void GoShop()
    {
        SceneManager.LoadScene("Shop");
    }


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
