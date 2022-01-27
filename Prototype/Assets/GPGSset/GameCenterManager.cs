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

    [SerializeField] Text text1;
    [SerializeField] Text text2;

    MyUserData userdata;

    void Awake()
    {
        FirebaseDatabase.GetInstance("https://acrobatgames-f9ba6-default-rtdb.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        userdata = new MyUserData();
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
            
            UIDRigister(newUser.UserId);        //기기 저장소 확인하는 if조건문 만들어주는게 좋을듯
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

    /*public class User
    {
        public string user_name;        //디폴트네임
        public bool Area1;       //진척도
        public bool Area2;
        public bool Area3;
        public bool Area4;
        public int A1Stage1_achievement;
        public int A1Stage2_achievement;
        public int A1Stage3_achievement;
        public int Skill1_Level;
        public int Skill2_Level;
        public int Skill3_Level;

        public User()
        {
            this.user_name = "defalut";
            this.Area1 = false;
            this.Area2 = false;
            this.Area3 = false;
            this.Area4 = false;
            this.A1Stage1_achievement = 0;
            this.A1Stage1_achievement = 0;
            this.A1Stage1_achievement = 0;
            this.Skill1_Level = 1;
            this.Skill2_Level = 1;
            this.Skill3_Level = 1;
        }
    }*/

    public class MyUserData
    {
        public string uid;
        public int level;
        public string user_name;        //디폴트네임
        public bool Area1;       //진척도
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
            //Users = newUser;
            //writeNewUser(newUser.UserId);
            InitializeFirebase();
            //아마 이미 회원가입이 되어 있으면 writeNewUser를 발동 안해야함
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

    public void UIDRigister(string uid)
    {
        if(File.Exists(Application.persistentDataPath + "/Userdata.json"))
        {
            Debug.Log("UID를 비롯한 유저 정보 엑세스");
            string json = File.ReadAllText(Application.persistentDataPath + "/Userdata.json");
            userdata = JsonUtility.FromJson<MyUserData>(json);
        }
        else
        {
            Debug.Log("UID 저장하기");

            userdata.user_name = "박찬중";
            userdata.uid = uid;
            userdata.Skill1_Level = 1;
            userdata.Skill2_Level = 1;
            userdata.Skill3_Level = 1;
            userdata.level = 1;

            File.WriteAllText(Application.persistentDataPath + "/Userdata.json", JsonUtility.ToJson(userdata));
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

    public void Area1Clear()
    {
        userdata.Area1 = true;      //1스테이지 클리어시

        string json = JsonUtility.ToJson(userdata);
        reference.Child(Usersid).SetRawJsonValueAsync(json);
        text1.text = "@Area1 => Clear";
    }

    public void Area2Clear()
    {
        userdata.Area2 = true;      //2스테이지 클리어시

        string json = JsonUtility.ToJson(userdata);
        reference.Child(Usersid).SetRawJsonValueAsync(json);
        text2.text = "@Area2 => Clear";
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