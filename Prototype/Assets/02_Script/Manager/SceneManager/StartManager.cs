using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

using GooglePlayGames;
using GooglePlayGames.BasicApi;

using Data;
using ETC;

namespace StartScene
{
    public class StartManager : MonoBehaviour
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
            public int AssassinLevel = 1;
            public int MagicianATK = 400;
            public int MagicianEXP;
            public int MagicianHP = 800;
            public int MagicianLevel = 1;
            public int WarriorATK = 380;
            public int WarriorEXP;
            public int WarriorHP = 1200;
            public int WarriorLevel = 1;
        }

        private bool initComplete;

        private string version;
        [SerializeField] private Text versionText;
        //���� �����ϴ� ��ü��
        [SerializeField] private WarningMessage warningMessage;
        [SerializeField] private CreateAccountPopup createAccountPopup;
        [SerializeField] private LoginObject loginObject;

        //�����ͺ��̽� reference
        private DatabaseReference reference;
        private FirebaseAuth auth = null;
        //����� ����
        private FirebaseUser user = null;
        //�׽�Ʈ
        [SerializeField] private string userId;
        public DatabaseReference Reference { get => reference; }

        private void Awake()
        {
            InitGoogleFunction();
            InitFireBaseFunction();
        }
        private IEnumerator Start()
        {
            Debug.Log("hi");
            yield return new WaitUntil(() => version != null);
            Debug.Log("hello");
            versionText.text = GameManager.Instance.Version;
            if (version != GameManager.Instance.Version)
            {
                warningMessage.SetWarningText("�ֽ� �������� ������Ʈ �ؾ��մϴ�!");
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif

                yield break;
            }
            loginObject.ActiveLoginObj();
        }
        public void SetPlayerData()
        {
            reference.Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Result.Value != null) //���� ������ Ȯ��
            {
                    if (task.IsCompleted)
                    {
                        GameManager.Instance.FirebaseData = new FirebaseData(userId);
                        StartCoroutine(GameManager.Instance.FirebaseData.Co_InitData());
                        StartCoroutine(Co_WaitUntillInitData());
                    }
                    else if (task.IsFaulted)
                    {
                        warningMessage.SetWarningText("�����͸� �о���� �� �����߽��ϴ�.");
                        return;
                    }
                    else if (task.IsCanceled)
                    {
                        warningMessage.SetWarningText("������ �о���Ⱑ ��ҵǾ����ϴ�.");
                        return;
                    }
                }
                else //���� �����Ͱ� ����, ���� ������ ��ġ
            {
                    if (task.IsCompleted)
                    {
                        createAccountPopup.SetCreateAccountPopup();
                    }
                    else if (task.IsFaulted)
                    {
                        warningMessage.SetWarningText("�����͸� �о���� �� �����߽��ϴ�.");
                        return;
                    }
                    else if (task.IsCanceled)
                    {
                        warningMessage.SetWarningText("������ �о���Ⱑ ��ҵǾ����ϴ�.");
                        return;
                    }
                }
            });
        }
        public void CheckNickname(string nickname)
        {
            reference.Child("AllNicknames").Child(nickname).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    if (task.Result.Value != null)
                    {
                        Debug.Log(task.Result.Value);
                        warningMessage.SetWarningText("�̹� �����ϴ� �г����Դϴ�!");
                        return;
                    }
                    else
                    {
                        CreateNewUserData(nickname);
                        createAccountPopup.gameObject.SetActive(false);
                    }
                }
                else if (task.IsFaulted)
                {
                    warningMessage.SetWarningText("�����͸� �о���� �� �����߽��ϴ�.");
                }
            });
        }
        private void CreateNewUserData(string nickname)
        {
            Info info = new Info(userId, nickname);
            Skill skill = new Skill();
            Stage stage = new Stage();
            Unit unit = new Unit();

            string infoJson = JsonUtility.ToJson(info);
            string skillJson = JsonUtility.ToJson(skill);
            string stageJson = JsonUtility.ToJson(stage);
            string unitJson = JsonUtility.ToJson(unit);

            reference.Child(userId).Child("Info").SetRawJsonValueAsync(infoJson);
            reference.Child(userId).Child("Skill").SetRawJsonValueAsync(skillJson);
            reference.Child(userId).Child("Stage").SetRawJsonValueAsync(stageJson);
            reference.Child(userId).Child("Unit").SetRawJsonValueAsync(unitJson);

            reference.Child("AllNicknames").Child(nickname).SetValueAsync(true);

            GameManager.Instance.FirebaseData = new FirebaseData(userId);

            StartCoroutine(GameManager.Instance.FirebaseData.Co_InitData());

            StartCoroutine(Co_WaitUntillInitData());
        }
        private IEnumerator Co_WaitUntillInitData()
        {
            yield return new WaitUntil(() => GameManager.Instance.FirebaseData.dataLoadComplete);
            GameManager.Instance.StageInfo = new StageInfo();
            GameManager.Instance.LoadSceneThroughLoadingScene(0);
        }
        private IEnumerator TryGoogleLogin()
        {
            yield return new WaitUntil(() => initComplete);
            Social.localUser.Authenticate((bool success) =>
            {
                if (success) StartCoroutine(TryFirebaseLogin());
                else warningMessage.SetWarningText("���� ���� ����!");
            });
        }
        private IEnumerator TryFirebaseLogin()
        {
            while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
                yield return null;

            string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
            string accessToken = null;

            Credential credential = GoogleAuthProvider.GetCredential(idToken, accessToken);
            auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    warningMessage.SetWarningText("���� ������ �о���Ⱑ ��ҵǾ����ϴ�.");
                    return;
                }
                if (task.IsFaulted)
                {
                    warningMessage.SetWarningText("���� ������ �о���� �� �����߽��ϴ�.");
                    return;
                }

                user = task.Result;
                userId = user.UserId;

                loginObject.ActiveLoginObj();
            });
        }

        private void InitGoogleFunction()
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .RequestServerAuthCode(false /* Don't force refresh */)
            .RequestIdToken()
            .RequestEmail()
            .Build();

            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
        }
        private void InitFireBaseFunction()
        {
            auth = FirebaseAuth.DefaultInstance;

            FirebaseDatabase.GetInstance("https://acrobatgames-f9ba6-default-rtdb.firebaseio.com/");

            reference = FirebaseDatabase.DefaultInstance.GetReference("users");

            reference.Child("Version").GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    if (task.Result.Value != null)
                    {
                        version = task.Result.Value.ToString();
                    }
                }
                else
                {
                    warningMessage.SetWarningText("���� ������ �о���� �� �����߽��ϴ�.");
                }
            }
            );
        }
        //void AuthStateChanged(object sender, System.EventArgs eventArgs)
        //{
        //    if (auth.CurrentUser != user)
        //    {
        //        //������ ������ ����� ������ ���ٸ� true ����
        //        signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

        //        if (!signedIn && user != null)
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
    }
}
