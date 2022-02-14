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


public class GameCenterManager : MonoBehaviour
{
    public string FireBaseId = string.Empty;
    FirebaseAuth auth;      //파이어베이스 인증 관리 객체

    DatabaseReference reference;        //데이터를 쓰기위한 reference

    string Usersid;

    //User user;


    public bool StartTouch;
    [SerializeField] private Text StartText;
    [SerializeField] private GameObject LoginPanel;

    public UserData userdata;

    void Awake()
    {
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

        StartTouch = true;      //로딩 완료
        GameStart();

        //실험구간

        Usersid = "NaIxowYCsaSqdaYaWtWbIYErkqM2";

        reference.Child("users").Child(Usersid).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("failed reading...");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                Debug.Log("컴플릿떳음");

                foreach (DataSnapshot data in snapshot.Children)
                {
                    //IDictionary userinfo = (IDictionary)data.Value;
                    //딕셔너리 공부해야할듯. 이해가 안댐
                    Debug.LogFormat("[Database] key : {0}, value :{1}", data.Key, data.Value);

                    //아씨발좆같다
                }
            }
        });
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

                    Debug.Log("InitializeFirebase 접근완료");

                    foreach (DataSnapshot data in snapshot.Children)
                    {
                        IDictionary userinfo = (IDictionary)data.Value;
                        //딕셔너리 공부해야할듯. 이해가 안댐
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

    public void GoogleLogin()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate(success => // 로그인 시도
            {
                if (success) // 성공하면
                {
                    Debug.Log("TryFirebaseLogin 함수 실행");
                    StartCoroutine(TryFirebaseLogin());
                }
                else // 실패하면
                {
                    Debug.Log("실패!");
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
            Debug.Log("auth.SignOut 완료");
        }
    }

    public void GuestLogin()
    {
        auth = FirebaseAuth.DefaultInstance;

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
            Debug.Log("게스트 로그인 완료");

            FirebaseUser newUser = task.Result;
            Usersid = newUser.UserId;
            //Debug.Log("writeNewUser 함수 발동");
            //writeNewUser(newUser.UserId);
            
            Debug.Log("UIDRigister 시작");
            UIDRigister(newUser.UserId, "Guest");        //기기 저장소 확인하는 if조건문 만들어주는게 좋을듯
            InitializeFirebase();

            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }

    public void GuestLogout()
    {
        if(auth == null)
        {
            Debug.Log("auth 미참조");
        }
        else
            auth.SignOut();
    }

    public void GoBattle()
    {
        SceneManager.LoadScene("SampleScene");
    }


    public class MyUserData //데이터
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

    public IEnumerator TryFirebaseLogin()
    {
        while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
            yield return null;
        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();   //id토큰가져오기
        string accessToken = null;

        auth = FirebaseAuth.DefaultInstance;
        Credential credential = GoogleAuthProvider.GetCredential(idToken, accessToken);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => { //ContinueWithOnMainThread
            if (task.IsCanceled)        //실패경우
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;     
            }

            FirebaseUser newUser = task.Result;     //newUser에 task정보 저장.

            Usersid = newUser.UserId;

            UIDRigister(newUser.UserId, "Google");
            InitializeFirebase();

            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }

    private void InitializeFirebase()
    {
        reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log("InitializeFirebase 접근완료");

                foreach (DataSnapshot data in snapshot.Children)
                {
                    Debug.LogFormat("[Database] key : {0}, value :{1}", data.Key, data.Value);
                }
            }
        });
    }

    public void UIDRigister(string uid, string LastLogin)
    {
        Debug.Log("UIDRigister 진입 성공");

        if(File.Exists(Application.persistentDataPath + "/Userdata.json"))  //이미 유저 데이터가 로컬 저장소에
        {                                                                   //존재 한다면
            Debug.Log("UID를 비롯한 유저 정보 엑세스");

            string json = File.ReadAllText(Application.persistentDataPath + "/Userdata.json");
            userdata = JsonUtility.FromJson<UserData>(json);
        }
        else
        {
            Debug.Log("UID 생성하고 데이터 처음 만들기");

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

        /*Debug.Log("UID 저장하기");

        userdata.user_name = "박찬중";
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

        users.user_name = "내 이름은 박찬중";
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

    public void ReadingDbUserInfo()
    {
        Debug.Log("리딩슈타이너 발동");
        reference.Child("users").Child(Usersid).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error Database");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("리딩슈타이너 컴플릿트");

                DataSnapshot snapshot = task.Result;

                //가져온 데이터 snapshot을 json으로 변환 저장
                //string json = JsonUtility.ToJson(snapshot);

                foreach (DataSnapshot userdata in snapshot.Children)
                {
                    Debug.Log(userdata.Value);
                }
            }
        });
    }

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


    public void SaveData()
    {
        /*if (!Directory.Exists(Application.persistentDataPath + "/생성할 폴더 이름"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/생성할 폴더 이름");
        }*/

        /*[안드로이드 External]
        Application.persistentDataPath : / mnt / sdcard / Android / data / com.YourProductName.YourCompanyName / files[파일 읽기 / 쓰기 가능]
        Application.dataPath : / data / app / 번들이름 - 번호.apk
        Application.streamingAssetsPath : jar: file:///data/app/번들이름.apk!/assets [파일이 아닌 WWW로 읽기 가능]

        [안드로이드 Internal]
        Application.persistentDataPath : / Android / data / com.YourProductName.YourCompanyName / files[파일 읽기 / 쓰기 가능]
        Application.dataPath : / data / app / 번들이름 - 번호.apk
        Application.streamingAssetsPath : jar: file:///data/app/번들이름.apk!/assets [파일이 아닌 WWW로 읽기 가능]*/
    }
}