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
    //���̾�̽� ���� ���� ��ü
    FirebaseAuth auth = null;

    //������ ����
    FirebaseUser user = null;

    //�α��� ���� ȭ��
    public GameObject LoginPanel;

    //�ӽ� �ε� �г�
    public GameObject LoadingPanel;

    // �г��� ���� �г�
    public GameObject NicknamePanel;

    //���� ������ �Ǿ� �ִ� �������� üũ�ϴ� bool ����
    private bool signedIn = false;

    public string FireBaseId = string.Empty;

    public DatabaseReference reference;        //�����͸� �������� reference

    [SerializeField] public string Usersid;

    //User user;

    // �ӽ������� Ŭ����
    private User.userLoginData.LoginType tempLoginType = User.userLoginData.LoginType.None;
    private string tempemail = string.Empty;
    private string temppw = string.Empty;


    public bool StartTouch;
    [SerializeField] private Text StartText;

    public UserData userdata;

    [SerializeField] private Text ULV;
    [SerializeField] private Text UEXP;

    private string UEXPs;
    private string ULVs;

    private void Awake()
    {
        //Auth�� instance �ʱ�ȭ
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        //������ �α��� ������ � ������ ������ �����ǰ� �̺�Ʈ�� �ɾ��ش�.
        auth.StateChanged += AuthStateChanged;
        //AuthStateChanged(this, null);

        //�α��� �ε� �г� ��Ȱ��
        LoginPanel.SetActive(false);
        LoadingPanel.SetActive(false);

        //Firebase ���ι� reference �ʱ�ȭ
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

        //StartTouch = true;      //�ε� �Ϸ�
        //GameStart();

        //���豸��

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
                        Debug.Log("LV �߰�, ����" + data.Value);
                        ULVs = data.Value.ToString();
                        ULV.text = "LV : " + ULVs;
                    }
                    //��
                }
            }
        });
    }


    //������ ��� ������ �߻� �� ����
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if(auth.CurrentUser != user)
        {
            //������ ������ ������ ������ ���ٸ� true ����
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

    //�α��� ���� �г��� ���� �α����� user�� �ִ��� Ȯ���Ѵ�
    //������ ���� ���� ����
    public void LoginCheck()
    {
        if (!signedIn)
        {
            LoginPanel.SetActive(true);
        }
        else
        {
            StartCoroutine(CurrentUserDataGet());
        }
    }

    //���� ���� ���� �������� �����´�
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

    //���Ӹ��ξ����� �Ѿ
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
            userdata = JsonUtility.FromJson<UserData>(json);

            Usersid = userdata.UID;

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

    //�͸� �α���
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
            Debug.Log("�Խ�Ʈ �α��� �Ϸ�");

            FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            Usersid = newUser.UserId;
            //Debug.Log("writeNewUser �Լ� �ߵ�");
            //writeNewUser(newUser.UserId);

            Debug.Log("UIDRigister ����");
            UIDRigister(newUser.UserId, "Guest");        //���� ������ Ȯ���ϴ� if���ǹ� �������ִ°� ������
            InitializeFirebase();
        });

        //�ű� ���� ������ �ӽ�����
        userDataInit();
        userDataTempSave(User.userLoginData.LoginType.anony);

        //LoadingPanel.SetActive(false);
    }

    //���� �α���
    public void GoogleLogin()
    {
        //LoadingPanel.SetActive(true);

        try
        {
            //���� �α��� ó�� �κ�
            //���� �α��� �˾�â�� ������ ������ �ݹ��Լ��� �����Ѵ�.
            TryFirebaseLogin(new System.Action<bool>((bool chk) =>
            {
                if (chk)
                {
                    //�ű� ���� ������ �ӽ� ����
                    userDataInit();
                    userDataTempSave(User.userLoginData.LoginType.google);

                    //LoadingPanel.SetActive(false);
                    //�г��� â ���ֱ�
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


    //���� �α��� ����
    public IEnumerator TryFirebaseLogin(System.Action<bool> callback)
    {
        while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
            yield return null;

        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();   //id��ū��������
        string accessToken = null;

        Credential credential = GoogleAuthProvider.GetCredential(idToken, accessToken);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => { //ContinueWithOnMainThread
            if (task.IsCanceled)        //���а���
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

            user = task.Result;     //newUser�� task���� ����.
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                user.DisplayName, user.UserId);

            callback(true);

            Usersid = user.UserId;

            UIDRigister(user.UserId, "Google");
            InitializeFirebase();
        });
    }

    //���� �г��� �Է� ����
    private void InsertNewUserData()
    {
        var nickname = NicknamePanel.transform.Find("InputField").Find("NickNameInput")
            .GetComponent<Text>().text;

        if(nickname.Length > 0)
        {
            //�ű� ���� ������ �Է�
            //loginDataSave(tempLoginType, nickname, tempemail, temppw);
        }
        else
        {
            Debug.Log("������ �Է����ּ���");
            return;
        }
    }

    // ���� �ӽ� ������ �ʱ�ȭ
    private void userDataInit()
    {
        tempLoginType = User.userLoginData.LoginType.None;
        tempemail = string.Empty;
        temppw = string.Empty;
    }

    // ���� ������ �ӽ� ����
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
        Debug.Log("UIDRigister ���� ����");

        if(File.Exists(Application.persistentDataPath + "/Userdata.json"))  //�̹� ���� �����Ͱ� ���� �����ҿ�
        {                                                                   //���� �Ѵٸ�
            Debug.Log("UID�� ������ ���� ���� ������");

            string json = File.ReadAllText(Application.persistentDataPath + "/Userdata.json");
            userdata = JsonUtility.FromJson<UserData>(json);
        }
        else
        {
            Debug.Log("UID �����ϰ� ������ ó�� ������");

            userdata.UID = uid;
            userdata.LastLogin = LastLogin;

            userdata.Gold = 0;
            userdata.SkillPoints = 0;
            userdata.UnitPoints = 0;

            userdata.Skill1_Level = 1;
            userdata.Skill1_Unlock = true;
            userdata.Skill2_Level = 1;
            userdata.Skill2_Unlock = false;
            userdata.Skill3_Level = 1;
            userdata.Skill3_Unlock = false;
            userdata.Skill4_Level = 1;
            userdata.Skill4_Unlock = false;
            userdata.Skill5_Level = 1;
            userdata.Skill5_Unlock = false;
            userdata.Skill6_Level = 1;
            userdata.Skill6_Unlock = false;
            userdata.Skill7_Level = 1;
            userdata.Skill7_Unlock = false;

            userdata.S1EClear = false;
            userdata.S1HClear = false;
            userdata.S1NClear = false;

            userdata.S2EClear = false;
            userdata.S2HClear = false;
            userdata.S2NClear = false;

            userdata.S3EClear = false;
            userdata.S3HClear = false;
            userdata.S3NClear = false;

            userdata.S4EClear = false;
            userdata.S4HClear = false;
            userdata.S4NClear = false;

            userdata.S5EClear = false;
            userdata.S5HClear = false;
            userdata.S5NClear = false;

            userdata.S6EClear = false;
            userdata.S6HClear = false;
            userdata.S6NClear = false;

            userdata.S7EClear = false;
            userdata.S7HClear = false;
            userdata.S7NClear = false;

            userdata.S8EClear = false;
            userdata.S8HClear = false;
            userdata.S8NClear = false;

            userdata.ATKPower = 1;
            userdata.EXP = 0;
            userdata.HP = 100;
            userdata.Level = 1;
            userdata.MP = 1;
            userdata.Speed = 5;

            userdata.Warrior_Level = 1;
            userdata.Warrior_Unlock = true;

            userdata.Assassin_Level = 1;
            userdata.Assassin_Unlock = false;

            userdata.Shielder_Level = 1;
            userdata.Shielder_Unlock = true;

            userdata.Druid_Level = 1;
            userdata.Druid_Unlock = false;

            userdata.Archor_Level = 1;
            userdata.Archor_Unlock = true;

            userdata.Mechanic_Level = 1;
            userdata.Mechanic_Unlock = false;

            userdata.Magician_Level = 1;
            userdata.Magician_Unlock = true;

            userdata.Specialist_Level = 1;
            userdata.Specialist_Unlock = false;


            string json = JsonUtility.ToJson(userdata);
            string key = Usersid;

            File.WriteAllText(Application.persistentDataPath + "/Userdata.json", JsonUtility.ToJson(userdata, true));
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
        Debug.Log("������Ÿ�̳� �ߵ�");
        reference.Child("users").Child(Usersid).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error Database");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("������Ÿ�̳� ���ø�Ʈ");

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
