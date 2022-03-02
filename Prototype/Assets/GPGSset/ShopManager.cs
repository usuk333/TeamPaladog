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

using UnityEngine.SceneManagement;
using System.IO;
using System.Threading.Tasks;


public class ShopManager : MonoBehaviour
{
    public class UserInfo
    {
        public int Gold = 0;
    }

    [SerializeField] private string Userid;

    private DatabaseReference reference;

    private bool isLoad;

    private string Unitpoints = string.Empty;
    private string Shielder_level = string.Empty;
    private string Warrior_level = string.Empty;
    private string Archor_level = string.Empty;
    private string Magician_level = string.Empty;

    [SerializeField] private Text Gold;
    [SerializeField] private Text TLv;
    [SerializeField] private Text THp;
    [SerializeField] private Text FLv;
    [SerializeField] private Text FHp;
    [SerializeField] private Text MksLv;
    [SerializeField] private Text MksHp;
    [SerializeField] private Text MLv;
    [SerializeField] private Text MHp;

    [SerializeField] private GameObject[] Jobpanel = new GameObject[4];
    private bool[] panelsonoff = new bool[4];

    public bool panelonoff = false;

    DataSnapshot snapshot;

    FirebaseDatabase firebaseDatabase;

    FirebaseApp firebaseApp;

    void Awake()
    {
        firebaseApp = FirebaseApp.DefaultInstance;
        firebaseDatabase = FirebaseDatabase.GetInstance(firebaseApp, "https://acrobatgames-f9ba6-default-rtdb.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        panelsonoff[0] = false;
        panelsonoff[1] = false;
        panelsonoff[2] = false;
        panelsonoff[3] = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Usersid = "NaIxowYCsaSqdaYaWtWbIYErkqM2";
        //reference.Child("users").Child(Userid).GetValueAsync().ContinueWith
        reference.Child("users").Child(Userid).Child("Unit").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("failed reading...");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("컴플릿떳음");
                snapshot = task.Result;

                StartCoroutine(UIupdate());
            }
        });
    }
    IEnumerator UpdateData()
    {
        yield return new WaitUntil(() => isLoad);


        isLoad = false;
    }
    private IEnumerator UIupdate()
    {
        yield return null;

        Debug.Log("UI업데이트시작");

        Unitpoints = snapshot.Child("UnitPoints").Value.ToString();
        Shielder_level = snapshot.Child("Shielder").Child("Shielder_Level").Value.ToString();
        Warrior_level = snapshot.Child("Warrior").Child("Warrior_Level").Value.ToString();
        Archor_level = snapshot.Child("Archor").Child("Archor_Level").Value.ToString();
        Magician_level = snapshot.Child("Magician").Child("Magician_Level").Value.ToString();

        Debug.Log("스냅샷 자식 수는 " + snapshot.ChildrenCount);

        Gold.text = "UnitPoints : " + Unitpoints;
        TLv.text = "LV : " + Shielder_level;
        FLv.text = "LV : " + Warrior_level;
        MksLv.text = "LV : " + Archor_level;
        MLv.text = "LV : " + Magician_level;

        //HP 공식 추가되면 함수 만들기
    }

    //메인으로 버튼
    public void ShopGoMain()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void JobInfo(int i)
    {
        if(panelonoff == false)
        {
            Jobpanel[i].SetActive(true);
            panelsonoff[i] = true;

            panelonoff = true;
        }
        else
        {
            for (int p =0; p < panelsonoff.Length; p++)
            {
                if(panelsonoff[p] == true)
                {
                    Jobpanel[p].SetActive(false);
                    panelsonoff[p] = false;
                }
            }
            Jobpanel[i].SetActive(true);
            panelsonoff[i] = true;
        }
    }

    //탱커 레벨 업
    public void TankerLevelUp()
    {

        int UL = int.Parse(Shielder_level) + 1;

        Dictionary<string, object> update = new Dictionary<string, object>();
        update.Add("Shielder_Level", UL);

        reference.Child("users").Child(Userid).Child("Unit").Child("Shielder").UpdateChildrenAsync(update);

        reference.Child("users").Child(Userid).Child("Unit").Child("Shielder").Child("Shielder_Level").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("failed reading...");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("탱커 데이터 접속");
                DataSnapshot snapshot = task.Result;

                Shielder_level = snapshot.Value.ToString();
                TLv.text = "LV : " + Shielder_level;
            }
        });
    }

    //전사 레벨 업
    public void FighterLevelUp()
    {

        int UL = int.Parse(Warrior_level) + 1;

        Dictionary<string, object> update = new Dictionary<string, object>();
        update.Add("Warrior_Level", UL);

        reference.Child("users").Child(Userid).Child("Unit").Child("Warrior").UpdateChildrenAsync(update);

        reference.Child("users").Child(Userid).Child("Unit").Child("Warrior").Child("Warrior_Level").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("failed reading...");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("전사 데이터 접속");
                DataSnapshot snapshot = task.Result;

                Warrior_level = snapshot.Value.ToString();
                FLv.text = "LV : " + Warrior_level;
            }
        });
    }

    //원딜 레벨 업
    public void MarksmanLevelUp()
    {
        int UL = int.Parse(Archor_level) + 1;

        Dictionary<string, object> update = new Dictionary<string, object>();
        update.Add("Archor_Level", UL);

        reference.Child("users").Child(Userid).Child("Unit").Child("Archor").UpdateChildrenAsync(update);

        reference.Child("users").Child(Userid).Child("Unit").Child("Archor").Child("Archor_Level").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("failed reading...");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("원딜 데이터 접속");
                DataSnapshot snapshot = task.Result;

                Archor_level = snapshot.Value.ToString();
                MksLv.text = "LV : " + Archor_level;
            }
        });
    }

    //법사류 레벨 업
    public void MageLevelUp()
    {
        int UL = int.Parse(Magician_level) + 1;

        Dictionary<string, object> update = new Dictionary<string, object>();
        update.Add("Magician_Level", UL);

        reference.Child("users").Child(Userid).Child("Unit").Child("Magician").UpdateChildrenAsync(update);

        reference.Child("users").Child(Userid).Child("Unit").Child("Magician").Child("Magician_Level").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("failed reading...");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("마법사 데이터 접속");
                DataSnapshot snapshot = task.Result;

                Magician_level = snapshot.Value.ToString();
                MLv.text = "LV : " + Magician_level;
            }
        });
    }
}
