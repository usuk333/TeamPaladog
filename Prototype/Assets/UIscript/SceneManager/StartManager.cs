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

using System.IO;
using System.Threading.Tasks;
using TMPro;

public class StartManager : MonoBehaviour
{
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
    [SerializeField] public string Usersid;

    FirebaseData FirebaseData;

    private void Awake()
    {
        FirebaseData = new FirebaseData(Usersid);
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
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
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
            }
        });
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

    private void NextScene()
    {
        LoadingSceneController.LoadScene("StartScene");
    }
}
