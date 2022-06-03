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
using TMPro;


public class ShopManager : MonoBehaviour
{
    public class UserInfo
    {
        public int Gold = 0;
    }

    [SerializeField] private string Userid;

    private DatabaseReference reference;

    private bool isLoad;

    private string Gold = string.Empty;

    //유닛 레벨
    private string Shielder_level = string.Empty;
    [SerializeField] private Text tShielder_level;
    private string Warrior_level = string.Empty;
    [SerializeField] private Text tWarrior_level;
    private string Archor_level = string.Empty;
    [SerializeField] private Text tArchor_level;
    private string Magician_level = string.Empty;
    [SerializeField] private Text tMagician_level;

    //유닛 경험치
    private string Shielder_EXP = string.Empty;
    [SerializeField] private Text tShielder_EXP;
    private string Warrior_EXP = string.Empty;
    [SerializeField] private Text tWarrior_EXP;
    private string Archor_EXP = string.Empty;
    [SerializeField] private Text tArchor_EXP;
    private string Magician_EXP = string.Empty;
    [SerializeField] private Text tMagician_EXP;

    //계열 포인트
    private string TankerPoints = string.Empty;
    [SerializeField] private TextMeshProUGUI tTankerPoints;
    private string WarriorPoints = string.Empty;
    [SerializeField] private TextMeshProUGUI tWarriorPoints;
    private string ADPoints = string.Empty;
    [SerializeField] private TextMeshProUGUI tADPoints;
    private string MagePoints = string.Empty;
    [SerializeField] private TextMeshProUGUI tMagePoints;

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


    [SerializeField] private TextMeshProUGUI tGold;

    [SerializeField] private GameObject NotEnough;

    [SerializeField] private GameObject[] Jobpanel = new GameObject[4];
    private bool[] panelsonoff = new bool[4];

    public bool panelonoff = false;

    DataSnapshot snapshot;
    DataSnapshot snapshots;

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

        reference.Child("users").Child(Userid).Child("Info").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("failed reading...");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("컴플릿떳음");
                snapshots = task.Result;
                
            }
        });
    }
    private IEnumerator UIupdate()
    {
        yield return null;

        if (snapshot == null)
        {
            Start();
        }
        Debug.Log("UI업데이트시작");

        //Unitpoints = snapshot.Child("UnitPoints").Value.ToString();

        Gold = snapshots.Child("Gold").Value.ToString();

        //계열포인트
        TankerPoints = snapshots.Child("Points").Child("TankerPoints").Value.ToString();
        WarriorPoints = snapshots.Child("Points").Child("WarriorPoints").Value.ToString();
        ADPoints = snapshots.Child("Points").Child("ADPoints").Value.ToString();
        MagePoints = snapshots.Child("Points").Child("MagePoints").Value.ToString();

        //유닛 레벨
        Shielder_level = snapshot.Child("Shielder").Child("Shielder_Level").Value.ToString();
        Warrior_level = snapshot.Child("Warrior").Child("Warrior_Level").Value.ToString();
        Archor_level = snapshot.Child("Archor").Child("Archor_Level").Value.ToString();
        Magician_level = snapshot.Child("Magician").Child("Magician_Level").Value.ToString();

        //유닛 경험치
        Shielder_EXP = snapshot.Child("Shielder").Child("Shielder_EXP").Value.ToString();
        Warrior_EXP = snapshot.Child("Warrior").Child("Warrior_EXP").Value.ToString();
        Archor_EXP = snapshot.Child("Archor").Child("Archor_EXP").Value.ToString();
        Magician_EXP = snapshot.Child("Magician").Child("Magician_EXP").Value.ToString();

        //HP, ATK
        ShielderHP = snapshot.Child("Shielder").Child("Shielder_HP").Value.ToString();
        ShielderATK = snapshot.Child("Shielder").Child("Shielder_ATK").Value.ToString();
        WarriorHP = snapshot.Child("Warrior").Child("Warrior_HP").Value.ToString();
        WarriorATK = snapshot.Child("Warrior").Child("Warrior_ATK").Value.ToString();
        ArchorHP = snapshot.Child("Archor").Child("Archor_HP").Value.ToString();
        ArchorATK = snapshot.Child("Archor").Child("Archor_ATK").Value.ToString();
        MagicianHP = snapshot.Child("Magician").Child("Magician_HP").Value.ToString();
        MagicianATK = snapshot.Child("Magician").Child("Magician_ATK").Value.ToString();

        Debug.Log("스냅샷 자식 수는 " + snapshot.ChildrenCount);

        //UI에 표시
        tTankerPoints.text = "x " + TankerPoints;
        tWarriorPoints.text = "x " + WarriorPoints;
        tADPoints.text = "x " + ADPoints;
        tMagePoints.text = "x " + MagePoints;

        tShielder_EXP.text = Shielder_EXP + " / 500";
        tWarrior_EXP.text = Warrior_EXP + " / 500";
        tArchor_EXP.text = Archor_EXP + " / 500";
        tMagician_EXP.text = Magician_EXP + " / 500";

        tGold.text = "Gold : " + Gold;
        tShielder_level.text = "LV : " + Shielder_level;
        tWarrior_level.text = "LV : " + Warrior_level;
        tArchor_level.text = "LV : " + Archor_level;
        tMagician_level.text = "LV : " + Magician_level;

        tShielderHP.text = "체력 : " + ShielderHP;
        tShielderATK.text = "공격력 : " + ShielderATK;
        tWarriorHP.text = "체력 : " + WarriorHP;
        tWarriorATK.text = "공격력 : " + WarriorATK;
        tArchorHP.text = "체력 : " + ArchorHP;
        tArchorATK.text = "공격력 : " + ArchorATK;
        tMagicianHP.text = "체력 : " + MagicianHP;
        tMagicianATK.text = "공격력 : " + MagicianATK;

        //HP 공식 추가되면 함수 만들기
    }

    //메인으로 버튼
    public void ShopGoMain()
    {
        LoadingSceneController.LoadScene("StartScene");
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

    //탱커 경험치 증가
    public void TankerEXPUp()
    {
        if (int.Parse(TankerPoints) > 0)
        {
            int UE = int.Parse(Shielder_EXP) + 100; //경험치   
            int TP = int.Parse(TankerPoints) - 1;   //탱커 계열 포인트 - 1


            if (UE >= 500)       //EXP를 다 채워서 레벨업 루트
            {
                UE -= 500;
                int UL = int.Parse(Shielder_level) + 1;

                Dictionary<string, object> update = new Dictionary<string, object>();
                Dictionary<string, object> updatet = new Dictionary<string, object>();
                Dictionary<string, object> updatee = new Dictionary<string, object>();
                update.Add("Shielder_Level", UL);
                updatet.Add("TankerPoints", TP);
                updatee.Add("Shielder_EXP", UE);
                reference.Child("users").Child(Userid).Child("Unit").Child("Shielder").UpdateChildrenAsync(update);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatet);
                reference.Child("users").Child(Userid).Child("Unit").Child("Shielder").UpdateChildrenAsync(updatee);

                Shielder_level = UL.ToString();
                tShielder_level.text = "LV : " + Shielder_level;
                Shielder_EXP = UE.ToString();
                tShielder_EXP.text = Shielder_EXP + " / 500";
                TankerPoints = TP.ToString();
                tTankerPoints.text = "x " + TankerPoints;
            }
            else if (UE < 500)      //EXP가 다 안찬 루트
            {
                Shielder_EXP = UE.ToString();

                Dictionary<string, object> update = new Dictionary<string, object>();
                update.Add("Shielder_EXP", UE);
                reference.Child("users").Child(Userid).Child("Unit").Child("Shielder").UpdateChildrenAsync(update);

                Shielder_EXP = UE.ToString();
                tShielder_EXP.text = Shielder_EXP + " / 500";
                TankerPoints = TP.ToString();
                tTankerPoints.text = "x " + TankerPoints;
            }
        }
        else if (int.Parse(Gold) < 100)
        {
            NotEnough.SetActive(true);
        }
        else
        {
            Debug.LogError("오류 발생");
        }
    }

    //탱커 공격력 증가
    public void ShielderATKUp()
    {
        if(int.Parse(Gold) >= 200)
        {
            if(int.Parse(ShielderATK) < int.Parse(Shielder_level) * 10)        //레벨 한계에 도달X & 공격력 증가
            {
                int Golds = int.Parse(Gold) - 200;
                int SATK = int.Parse(ShielderATK) + 2;

                Dictionary<string, object> update = new Dictionary<string, object>();
                Dictionary<string, object> updatet = new Dictionary<string, object>();
                update.Add("Shielder_ATK", SATK);
                updatet.Add("Gold", Golds);
                reference.Child("users").Child(Userid).Child("Unit").Child("Shielder").UpdateChildrenAsync(update);
                reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updatet);

                Gold = Golds.ToString();
                tGold.text = "Gold : " + Gold;
                ShielderATK = SATK.ToString();
                tShielderATK.text = "공격력 : " + ShielderATK;
            }
            else if (int.Parse(ShielderATK) >= int.Parse(Shielder_level) * 10)
            {
                Debug.LogError("한계치 입니다");
            }
        }
        else if (int.Parse(Gold) < 200)
        {
            NotEnough.SetActive(true);
        }
        else
        {
            Debug.LogError("오류 발생");
        }
    }

    //탱커 HP 증가
    public void ShielderHPUp()
    {
        if (int.Parse(Gold) >= 200)
        {
            if (int.Parse(ShielderHP) < int.Parse(Shielder_level) * 100)        //레벨 한계에 도달X & 공격력 증가
            {
                int Golds = int.Parse(Gold) - 200;
                int SHP = int.Parse(ShielderHP) + 20;

                Dictionary<string, object> update = new Dictionary<string, object>();
                Dictionary<string, object> updatet = new Dictionary<string, object>();
                update.Add("Shielder_HP", SHP);
                updatet.Add("Gold", Golds);
                reference.Child("users").Child(Userid).Child("Unit").Child("Shielder").UpdateChildrenAsync(update);
                reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updatet);

                Gold = Golds.ToString();
                tGold.text = "Gold : " + Gold;
                ShielderHP = SHP.ToString();
                tShielderHP.text = "체력 : " + ShielderHP;
            }
            else if (int.Parse(ShielderHP) >= int.Parse(Shielder_level) * 100)
            {
                Debug.LogError("한계치 입니다");
            }
        }
        else if (int.Parse(Gold) < 200)
        {
            NotEnough.SetActive(true);
        }
        else
        {
            Debug.LogError("오류 발생");
        }
    }

    //전사 레벨 업
    public void FighterEXPUp()
    {
        if (int.Parse(WarriorPoints) > 0)
        {
            int UE = int.Parse(Warrior_EXP) + 100; //경험치   
            int WP = int.Parse(WarriorPoints) - 1;   //전사 계열 포인트 - 1


            if (UE >= 500)       //EXP를 다 채워서 레벨업 루트
            {
                UE -= 500;
                int UL = int.Parse(Warrior_level) + 1;

                Dictionary<string, object> update = new Dictionary<string, object>();
                Dictionary<string, object> updatet = new Dictionary<string, object>();
                Dictionary<string, object> updatee = new Dictionary<string, object>();
                update.Add("Warrior_Level", UL);
                updatet.Add("WarriorPoints", WP);
                updatee.Add("Warrior_EXP", UE);
                reference.Child("users").Child(Userid).Child("Unit").Child("Warrior").UpdateChildrenAsync(update);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatet);
                reference.Child("users").Child(Userid).Child("Unit").Child("Warrior").UpdateChildrenAsync(updatee);

                Warrior_level = UL.ToString();
                tWarrior_level.text = "LV : " + Warrior_level;
                Warrior_EXP = UE.ToString();
                tWarrior_EXP.text = Warrior_EXP + " / 500";
                WarriorPoints = WP.ToString();
                tWarriorPoints.text = "x " + WarriorPoints;
            }
            else if (UE < 500)      //EXP가 다 안찬 루트
            {
                Warrior_EXP = UE.ToString();

                Dictionary<string, object> update = new Dictionary<string, object>();
                update.Add("Warrior_EXP", UE);
                reference.Child("users").Child(Userid).Child("Unit").Child("Warrior").UpdateChildrenAsync(update);

                Warrior_EXP = UE.ToString();
                tWarrior_EXP.text = Warrior_EXP + " / 500";
                WarriorPoints = WP.ToString();
                tTankerPoints.text = "x " + WarriorPoints;
            }
        }
        else if (int.Parse(Gold) < 100)
        {
            NotEnough.SetActive(true);
        }
        else
        {
            Debug.LogError("오류 발생");
        }
    }

    //전사 공격력 증가
    public void WarriorATKUp()
    {
        if (int.Parse(Gold) >= 200)
        {
            if (int.Parse(WarriorATK) < int.Parse(Warrior_level) * 10)        //레벨 한계에 도달X & 공격력 증가
            {
                int Golds = int.Parse(Gold) - 200;
                int WATK = int.Parse(WarriorATK) + 2;

                Dictionary<string, object> update = new Dictionary<string, object>();
                Dictionary<string, object> updatet = new Dictionary<string, object>();
                update.Add("Warrior_ATK", WATK);
                updatet.Add("Gold", Golds);
                reference.Child("users").Child(Userid).Child("Unit").Child("Warrior").UpdateChildrenAsync(update);
                reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updatet);

                Gold = Golds.ToString();
                tGold.text = "Gold : " + Gold;
                WarriorATK = WATK.ToString();
                tWarriorATK.text = "공격력 : " + WarriorATK;
            }
            else if (int.Parse(WarriorATK) >= int.Parse(Warrior_level) * 10)
            {
                Debug.LogError("한계치 입니다");
            }
        }
        else if (int.Parse(Gold) < 200)
        {
            NotEnough.SetActive(true);
        }
        else
        {
            Debug.LogError("오류 발생");
        }
    }

    //전사 HP 증가
    public void WarriorHPUp()
    {
        if (int.Parse(Gold) >= 200)
        {
            if (int.Parse(WarriorHP) < int.Parse(Warrior_level) * 100)        //레벨 한계에 도달X & 공격력 증가
            {
                int Golds = int.Parse(Gold) - 200;
                int WHP = int.Parse(WarriorHP) + 20;

                Dictionary<string, object> update = new Dictionary<string, object>();
                Dictionary<string, object> updatet = new Dictionary<string, object>();
                update.Add("Warrior_HP", WHP);
                updatet.Add("Gold", Golds);
                reference.Child("users").Child(Userid).Child("Unit").Child("Warrior").UpdateChildrenAsync(update);
                reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updatet);

                Gold = Golds.ToString();
                tGold.text = "Gold : " + Gold;
                WarriorHP = WHP.ToString();
                tWarriorHP.text = "체력 : " + WarriorHP;
            }
            else if (int.Parse(WarriorHP) >= int.Parse(Warrior_level) * 100)
            {
                Debug.LogError("한계치 입니다");
            }
        }
        else if (int.Parse(Gold) < 200)
        {
            NotEnough.SetActive(true);
        }
        else
        {
            Debug.LogError("오류 발생");
        }
    }

    //아처 경험치 업
    public void ArchorEXPUp()
    {
        if (int.Parse(ADPoints) > 0)
        {
            int UE = int.Parse(Archor_EXP) + 100; //경험치   
            int AP = int.Parse(ADPoints) - 1;   //탱커 계열 포인트 - 1


            if (UE >= 500)       //EXP를 다 채워서 레벨업 루트
            {
                UE -= 500;
                int UL = int.Parse(Archor_level) + 1;

                Dictionary<string, object> update = new Dictionary<string, object>();
                Dictionary<string, object> updatet = new Dictionary<string, object>();
                Dictionary<string, object> updatee = new Dictionary<string, object>();
                update.Add("Archor_Level", UL);
                updatet.Add("ADPoints", AP);
                updatee.Add("Archor_EXP", UE);
                reference.Child("users").Child(Userid).Child("Unit").Child("Archor").UpdateChildrenAsync(update);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatet);
                reference.Child("users").Child(Userid).Child("Unit").Child("Archor").UpdateChildrenAsync(updatee);

                Archor_level = UL.ToString();
                tArchor_level.text = "LV : " + Archor_level;
                Archor_EXP = UE.ToString();
                tArchor_EXP.text = Archor_EXP + " / 500";
                ADPoints = AP.ToString();
                tADPoints.text = "x " + ADPoints;
            }
            else if (UE < 500)      //EXP가 다 안찬 루트
            {
                Archor_EXP = UE.ToString();

                Dictionary<string, object> update = new Dictionary<string, object>();
                update.Add("Archor_EXP", UE);
                reference.Child("users").Child(Userid).Child("Unit").Child("Archor").UpdateChildrenAsync(update);

                Archor_EXP = UE.ToString();
                tArchor_EXP.text = Archor_EXP + " / 500";
                ADPoints = AP.ToString();
                tADPoints.text = "x " + ADPoints;
            }
        }
        else if (int.Parse(Gold) < 100)
        {
            NotEnough.SetActive(true);
        }
        else
        {
            Debug.LogError("오류 발생");
        }
    }

    //아처 공격력 증가
    public void ArchorATKUp()
    {
        if (int.Parse(Gold) >= 200)
        {
            if (int.Parse(ArchorATK) < int.Parse(Archor_level) * 10)        //레벨 한계에 도달X & 공격력 증가
            {
                int Golds = int.Parse(Gold) - 200;
                int SATK = int.Parse(ArchorATK) + 2;

                Dictionary<string, object> update = new Dictionary<string, object>();
                Dictionary<string, object> updatet = new Dictionary<string, object>();
                update.Add("Archor_ATK", SATK);
                updatet.Add("Gold", Golds);
                reference.Child("users").Child(Userid).Child("Unit").Child("Archor").UpdateChildrenAsync(update);
                reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updatet);

                Gold = Golds.ToString();
                tGold.text = "Gold : " + Gold;
                ArchorATK = SATK.ToString();
                tArchorATK.text = "공격력 : " + ArchorATK;
            }
            else if (int.Parse(ArchorATK) >= int.Parse(Archor_level) * 10)
            {
                Debug.LogError("한계치 입니다");
            }
        }
        else if (int.Parse(Gold) < 200)
        {
            NotEnough.SetActive(true);
        }
        else
        {
            Debug.LogError("오류 발생");
        }
    }

    //아처 HP 증가
    public void ArchorHPUp()
    {
        if (int.Parse(Gold) >= 200)
        {
            if (int.Parse(ArchorHP) < int.Parse(Archor_level) * 100)        //레벨 한계에 도달X & 공격력 증가
            {
                int Golds = int.Parse(Gold) - 200;
                int AHP = int.Parse(ArchorHP) + 20;

                Dictionary<string, object> update = new Dictionary<string, object>();
                Dictionary<string, object> updatet = new Dictionary<string, object>();
                update.Add("Archor_HP", AHP);
                updatet.Add("Gold", Golds);
                reference.Child("users").Child(Userid).Child("Unit").Child("Archor").UpdateChildrenAsync(update);
                reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updatet);

                Gold = Golds.ToString();
                tGold.text = "Gold : " + Gold;
                ArchorHP = AHP.ToString();
                tArchorHP.text = "체력 : " + ArchorHP;
            }
            else if (int.Parse(ArchorHP) >= int.Parse(Archor_level) * 100)
            {
                Debug.LogError("한계치 입니다");
            }
        }
        else if (int.Parse(Gold) < 200)
        {
            NotEnough.SetActive(true);
        }
        else
        {
            Debug.LogError("오류 발생");
        }
    }

    //법사류 레벨 업
    public void MagicianEXPUp()
    {
        if (int.Parse(MagePoints) > 0)
        {
            int UE = int.Parse(Magician_EXP) + 100; //경험치   
            int MP = int.Parse(MagePoints) - 1;   //탱커 계열 포인트 - 1


            if (UE >= 500)       //EXP를 다 채워서 레벨업 루트
            {
                UE -= 500;
                int UL = int.Parse(Magician_level) + 1;

                Dictionary<string, object> update = new Dictionary<string, object>();
                Dictionary<string, object> updatet = new Dictionary<string, object>();
                Dictionary<string, object> updatee = new Dictionary<string, object>();
                update.Add("Magician_Level", UL);
                updatet.Add("MagePoints", MP);
                updatee.Add("Magician_EXP", UE);
                reference.Child("users").Child(Userid).Child("Unit").Child("Magician").UpdateChildrenAsync(update);
                reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatet);
                reference.Child("users").Child(Userid).Child("Unit").Child("Magician").UpdateChildrenAsync(updatee);

                Magician_level = UL.ToString();
                tMagician_level.text = "LV : " + Magician_level;
                Magician_EXP = UE.ToString();
                tMagician_EXP.text = Magician_EXP + " / 500";
                MagePoints = MP.ToString();
                tMagePoints.text = "x " + MagePoints;
            }
            else if (UE < 500)      //EXP가 다 안찬 루트
            {
                Magician_EXP = UE.ToString();

                Dictionary<string, object> update = new Dictionary<string, object>();
                update.Add("Magician_EXP", UE);
                reference.Child("users").Child(Userid).Child("Unit").Child("Magician").UpdateChildrenAsync(update);

                Magician_EXP = UE.ToString();
                tMagician_EXP.text = Magician_EXP + " / 500";
                MagePoints = MP.ToString();
                tMagePoints.text = "x " + MagePoints;
            }
        }
        else if (int.Parse(Gold) < 100)
        {
            NotEnough.SetActive(true);
        }
        else
        {
            Debug.LogError("오류 발생");
        }
    }

    //법사 공격력 증가
    public void MagicianATKUp()
    {
        if (int.Parse(Gold) >= 200)
        {
            if (int.Parse(MagicianATK) < int.Parse(Magician_level) * 10)        //레벨 한계에 도달X & 공격력 증가
            {
                int Golds = int.Parse(Gold) - 200;
                int MATK = int.Parse(MagicianATK) + 2;

                Dictionary<string, object> update = new Dictionary<string, object>();
                Dictionary<string, object> updatet = new Dictionary<string, object>();
                update.Add("Magician_ATK", MATK);
                updatet.Add("Gold", Golds);
                reference.Child("users").Child(Userid).Child("Unit").Child("Magician").UpdateChildrenAsync(update);
                reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updatet);

                Gold = Golds.ToString();
                tGold.text = "Gold : " + Gold;
                MagicianATK = MATK.ToString();
                tMagicianATK.text = "공격력 : " + MagicianATK;
            }
            else if (int.Parse(MagicianATK) >= int.Parse(Magician_level) * 10)
            {
                Debug.LogError("한계치 입니다");
            }
        }
        else if (int.Parse(Gold) < 200)
        {
            NotEnough.SetActive(true);
        }
        else
        {
            Debug.LogError("오류 발생");
        }
    }

    //법사 HP 증가
    public void MagicianHPUp()
    {
        if (int.Parse(Gold) >= 200)
        {
            if (int.Parse(MagicianHP) < int.Parse(Magician_level) * 100)        //레벨 한계에 도달X & 공격력 증가
            {
                int Golds = int.Parse(Gold) - 200;
                int MHP = int.Parse(MagicianHP) + 20;

                Dictionary<string, object> update = new Dictionary<string, object>();
                Dictionary<string, object> updatet = new Dictionary<string, object>();
                update.Add("Magician_HP", MHP);
                updatet.Add("Gold", Golds);
                reference.Child("users").Child(Userid).Child("Unit").Child("Magician").UpdateChildrenAsync(update);
                reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updatet);

                Gold = Golds.ToString();
                tGold.text = "Gold : " + Gold;
                MagicianHP = MHP.ToString();
                tMagicianHP.text = "체력 : " + MagicianHP;
            }
            else if (int.Parse(MagicianHP) >= int.Parse(Magician_level) * 100)
            {
                Debug.LogError("한계치 입니다");
            }
        }
        else if (int.Parse(Gold) < 200)
        {
            NotEnough.SetActive(true);
        }
        else
        {
            Debug.LogError("오류 발생");
        }
    }

}
