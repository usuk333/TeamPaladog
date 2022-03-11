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

    private string TankerPoints = string.Empty;
    private string WarriorPoints = string.Empty;
    private string ADPoints = string.Empty;
    private string MagePoints = string.Empty;

    private string ShielderHP = string.Empty;
    [SerializeField] private Text tShielderHP;
    private string ShielderATK = string.Empty;
    [SerializeField] private Text tShielderATK;

    private string WarriorHP = string.Empty;
    [SerializeField] private Text tWarriorHP;
    private string WarriorATK = string.Empty;
    [SerializeField] private Text tWarriorATK;

    private string ArchorHP = string.Empty;
    [SerializeField] private Text tArchorHP;
    private string ArchorATK = string.Empty;
    [SerializeField] private Text tArchorATK;

    private string MagicianHP = string.Empty;
    [SerializeField] private Text tMagicianHP;
    private string MagicianATK = string.Empty;
    [SerializeField] private Text tMagicianATK;


    [SerializeField] private Text Gold;
    [SerializeField] private Text TLv;
    [SerializeField] private Text THp;
    [SerializeField] private Text FLv;
    [SerializeField] private Text FHp;
    [SerializeField] private Text MksLv;
    [SerializeField] private Text MksHp;
    [SerializeField] private Text MLv;
    [SerializeField] private Text MHp;

    [SerializeField] private Text tTankerPoints;
    [SerializeField] private Text tWarriorPoints;
    [SerializeField] private Text tADPoints;
    [SerializeField] private Text tMagePoints;

    [SerializeField] private GameObject NotEnough;

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

        NotEnough.SetActive(false);
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

        //계열포인트
        TankerPoints = snapshot.Child("TankerPoints").Value.ToString();
        WarriorPoints = snapshot.Child("WarriorPoints").Value.ToString();
        ADPoints = snapshot.Child("ADPoints").Value.ToString();
        MagePoints = snapshot.Child("MagePoints").Value.ToString();

        //유닛 레벨
        Shielder_level = snapshot.Child("Shielder").Child("Shielder_Level").Value.ToString();
        Warrior_level = snapshot.Child("Warrior").Child("Warrior_Level").Value.ToString();
        Archor_level = snapshot.Child("Archor").Child("Archor_Level").Value.ToString();
        Magician_level = snapshot.Child("Magician").Child("Magician_Level").Value.ToString();

        ShielderHP = snapshot.Child("Shielder").Child("Shielder_HP").Value.ToString();
        ShielderATK = snapshot.Child("Shielder").Child("Shielder_ATK").Value.ToString();

        Debug.Log("스냅샷 자식 수는 " + snapshot.ChildrenCount);

        //UI에 표시
        tTankerPoints.text = "x " + TankerPoints;
        tWarriorPoints.text = "x " + WarriorPoints;
        tADPoints.text = "x " + ADPoints;
        tMagePoints.text = "x " + MagePoints;

        Gold.text = "UnitPoints : " + Unitpoints;
        TLv.text = "LV : " + Shielder_level;
        FLv.text = "LV : " + Warrior_level;
        MksLv.text = "LV : " + Archor_level;
        MLv.text = "LV : " + Magician_level;

        tShielderHP.text = "체력 : " + ShielderHP;
        tShielderATK.text = "공격력 : " + ShielderATK;

        //HP 공식 추가되면 함수 만들기
    }

    //메인으로 버튼
    public void ShopGoMain()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void CloseNotEnough()
    {
        NotEnough.SetActive(false);
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
        if(int.Parse(TankerPoints) > 0)
        {
            int UL = int.Parse(Shielder_level) + 1;
            int TP = int.Parse(TankerPoints) - 1;

            Dictionary<string, object> update = new Dictionary<string, object>();
            Dictionary<string, object> updatet = new Dictionary<string, object>();
            update.Add("Shielder_Level", UL);
            updatet.Add("TankerPoints", TP);
            reference.Child("users").Child(Userid).Child("Unit").Child("Shielder").UpdateChildrenAsync(update);
            reference.Child("users").Child(Userid).Child("Unit").UpdateChildrenAsync(updatet);

            Shielder_level = UL.ToString();
            TLv.text = "LV : " + Shielder_level;
            TankerPoints = TP.ToString();
            tTankerPoints.text = "x " + TankerPoints;

            /*reference.Child("users").Child(Userid).Child("Unit").Child("Shielder").Child("Shielder_Level").GetValueAsync().ContinueWithOnMainThread(task =>
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
            });*/ //써야할지 아직 모르겠음. 일일히 서버로 가져올 필요가 있을까?
        }
        else if (int.Parse(TankerPoints) <= 0)
        {
            NotEnough.SetActive(true);
        }
        else
        {
            Debug.Log("오류 발생");
        }
    }

    //전사 레벨 업
    public void FighterLevelUp()
    {
        if (int.Parse(WarriorPoints) > 0)
        {
            int UL = int.Parse(Warrior_level) + 1;
            int WP = int.Parse(WarriorPoints) - 1;

            Dictionary<string, object> update = new Dictionary<string, object>();
            Dictionary<string, object> updatet = new Dictionary<string, object>();
            update.Add("Warrior_Level", UL);
            updatet.Add("WarriorPoints", WP);
            reference.Child("users").Child(Userid).Child("Unit").Child("Warrior").UpdateChildrenAsync(update);
            reference.Child("users").Child(Userid).Child("Unit").UpdateChildrenAsync(updatet);

            Warrior_level = UL.ToString();
            FLv.text = "LV : " + Warrior_level;
            WarriorPoints = WP.ToString();
            tWarriorPoints.text = "x " + WarriorPoints;

            /*reference.Child("users").Child(Userid).Child("Unit").Child("Warrior").Child("Warrior_Level").GetValueAsync().ContinueWithOnMainThread(task =>
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
            });*/
        }
        else if (int.Parse(WarriorPoints) <= 0)
        {
            NotEnough.SetActive(true);
        }
        else
        {
            Debug.Log("오류 발생");
        }
    }

    //원딜 레벨 업
    public void MarksmanLevelUp()
    {
        if(int.Parse(ADPoints) > 0)
        {
            int UL = int.Parse(Archor_level) + 1;
            int AP = int.Parse(ADPoints) - 1;

            Dictionary<string, object> update = new Dictionary<string, object>();
            Dictionary<string, object> updatet = new Dictionary<string, object>();
            update.Add("Archor_Level", UL);
            updatet.Add("ADPoints", AP);
            reference.Child("users").Child(Userid).Child("Unit").Child("Archor").UpdateChildrenAsync(update);
            reference.Child("users").Child(Userid).Child("Unit").UpdateChildrenAsync(updatet);

            Archor_level = UL.ToString();
            MksLv.text = "LV : " + Archor_level;
            ADPoints = AP.ToString();
            tADPoints.text = "x " + ADPoints;

            /*reference.Child("users").Child(Userid).Child("Unit").Child("Archor").Child("Archor_Level").GetValueAsync().ContinueWithOnMainThread(task =>
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
            });*/
        }
        else if (int.Parse(ADPoints) <= 0)
        {
            NotEnough.SetActive(true);
        }
        else
        {
            Debug.Log("오류 발생");
        }
    }

    //법사류 레벨 업
    public void MageLevelUp()
    {
        if(int.Parse(MagePoints) > 0)
        {
            int UL = int.Parse(Magician_level) + 1;
            int MP = int.Parse(MagePoints) - 1;

            Dictionary<string, object> update = new Dictionary<string, object>();
            Dictionary<string, object> updatet = new Dictionary<string, object>();
            update.Add("Magician_Level", UL);
            updatet.Add("MagePoints", MP);
            reference.Child("users").Child(Userid).Child("Unit").Child("Magician").UpdateChildrenAsync(update);
            reference.Child("users").Child(Userid).Child("Unit").UpdateChildrenAsync(updatet);

            Magician_level = UL.ToString();
            MLv.text = "LV : " + Magician_level;
            MagePoints = MP.ToString();
            tMagePoints.text = "x " + MagePoints;

            /*reference.Child("users").Child(Userid).Child("Unit").Child("Magician").Child("Magician_Level").GetValueAsync().ContinueWithOnMainThread(task =>
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
            });*/
        }
        else if (int.Parse(MagePoints) <= 0)
        {
            NotEnough.SetActive(true);
        }
        else
        {
            Debug.Log("오류 발생");
        }
    }

    
}
