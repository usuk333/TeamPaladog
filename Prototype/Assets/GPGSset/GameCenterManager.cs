using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SceneManagement;
using Firebase.Database;


public class GameCenterManager : MonoBehaviour
{
    public string FireBaseId = string.Empty;
    FirebaseAuth auth;      //���̾�̽� ���� ���� ��ü

    DatabaseReference reference;        //�����͸� �������� reference

    string Usersid;

    User user;

    [SerializeField] Text text1;
    [SerializeField] Text text2;

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
    }

    public void GoogleLogin()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate(success => // �α��� �õ�
            {
                if (success) // �����ϸ�
                {
                    StartCoroutine(TryFirebaseLogin());
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
            Debug.Log("auth ������");
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
        public string user_name;        //����Ʈ����
        public bool Area1;       //��ô��
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
    }

    IEnumerator TryFirebaseLogin()
    {
        while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
            yield return null;
        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();   //id��ū��������
        string accessToken = null;

        auth = FirebaseAuth.DefaultInstance;
        Credential credential = GoogleAuthProvider.GetCredential(idToken, accessToken);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => { //ContinueWithOnMainThread
            if (task.IsCanceled)        //���а��
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;     
            }

            FirebaseUser newUser = task.Result;     //newUser�� task���� ����.
            //Users = newUser;
            writeNewUser(newUser.UserId);
            Debug.Log(newUser.UserId);
            //�Ƹ� �̹� ȸ�������� �Ǿ� ������ writeNewUser�� �ߵ� ���ؾ���
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }

    void writeNewUser(string userid) // ������ ȸ�� ���� ��ȣ�� ���� ����� �⺻�� ����
    {
        /*FirebaseDatabase.GetInstance("https://acrobatgames-f9ba6-default-rtdb.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference; //�����ͺ��̽� ��ü �ʱ�ȭ*/

        Debug.Log("�����ͺ��̽� ��ü �ʱ�ȭ");


        user = new User();
        string json = JsonUtility.ToJson(user); // ������ ����ڿ� ���� ���� json �������� ����
        reference.Child(userid).SetRawJsonValueAsync(json); // �����ͺ��̽��� json ���� ���ε�

        Usersid = userid;
    }

    void readingData()
    {
        //FirebaseDatabase.GetInstance("https://acrobatgames-f9ba6-default-rtdb.firebaseio.com/");
        //reference = FirebaseDatabase.DefaultInstance.RootReference; //�����ͺ��̽� ��ü �ʱ�ȭ

        reference.Child(Usersid).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error Database");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                object value = snapshot.Value;

                /*if(null != (value as |Dictionary))
                {
                    dic = 
                }*/
            }
        }
        );
    }

    public void Area1Clear()
    {
        user.Area1 = true;      //1�������� Ŭ�����

        string json = JsonUtility.ToJson(user);
        reference.Child(Usersid).SetRawJsonValueAsync(json);
        text1.text = "@Area1 => Clear";
    }

    public void Area2Clear()
    {
        user.Area2 = true;      //2�������� Ŭ�����

        string json = JsonUtility.ToJson(user);
        reference.Child(Usersid).SetRawJsonValueAsync(json);
        text2.text = "@Area2 => Clear";
    }
}