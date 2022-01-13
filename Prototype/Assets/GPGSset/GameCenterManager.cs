using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SceneManagement;
using Firebase.Database;


public class GameCenterManager : MonoBehaviour
{
    public string FireBaseId = string.Empty;
    FirebaseAuth auth;      //파이어베이스 인증 관리 객체

    DatabaseReference reference;        //데이터를 쓰기위한 reference

    void Awake()
    {
        
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

            FirebaseUser newUser = task.Result;
            writeNewUser(newUser.UserId);

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

    public class User
    {
        public string user_name;        //디폴트네임
        public bool Area1;       //진척도
        public bool Area2;
        public bool Area3;
        public bool Area4;
        public int A1Stage1_achievement;
        public int A1Stage2_achievement;
        public int A1Stage3_achievement;

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
        }
    }

    IEnumerator TryFirebaseLogin()
    {
        while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
            yield return null;
        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
        string accessToken = null;

        auth = FirebaseAuth.DefaultInstance;
        Credential credential = GoogleAuthProvider.GetCredential(idToken, accessToken);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => { //ContinueWithOnMainThread
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result;
            writeNewUser(newUser.UserId);
            Debug.Log(newUser.UserId);

            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }

    void writeNewUser(string userid) // 가입한 회원 고유 번호에 대한 사용자 기본값 설정
    {
        FirebaseDatabase.GetInstance("https://acrobatgames-f9ba6-default-rtdb.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference; //데이터베이스 객체 초기화

        Debug.Log("데이터베이스 객체 초기화");

        User user = new User();
        string json = JsonUtility.ToJson(user); // 생성한 사용자에 대한 정보 json 형식으로 저장
        reference.Child(userid).SetRawJsonValueAsync(json); // 데이터베이스에 json 파일 업로드
    }
}