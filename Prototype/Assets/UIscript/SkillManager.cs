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

public class SkillManager : MonoBehaviour
{
    private DatabaseReference reference;
    FirebaseDatabase firebaseDatabase;
    FirebaseApp firebaseApp;

    [SerializeField] private string Userid;     //NaIxowYCsaSqdaYaWtWbIYErkqM2


    DataSnapshot snapshot;

    [SerializeField] private GameObject NotEnough;

    private string Gold = string.Empty;
    [SerializeField] private TextMeshProUGUI tGold;

    private string SkillPoint = string.Empty;
    [SerializeField] private Text tSkillPoint;

    private string Skill1_Level = string.Empty;
    [SerializeField] private Text tSkill1_Level;
    private string Skill2_Level = string.Empty;
    [SerializeField] private Text tSkill2_Level;
    private string Skill3_Level = string.Empty;
    [SerializeField] private Text tSkill3_Level;
    private string Skill4_Level = string.Empty;
    [SerializeField] private Text tSkill4_Level;
    private string Skill5_Level = string.Empty;
    [SerializeField] private Text tSkill5_Level;
    private string Skill6_Level = string.Empty;
    [SerializeField] private Text tSkill6_Level;
    private string Skill7_Level = string.Empty;
    [SerializeField] private Text tSkill7_Level;

    //계열 포인트
    private string TankerPoints = string.Empty;
    [SerializeField] private TextMeshProUGUI tTankerPoints;
    private string WarriorPoints = string.Empty;
    [SerializeField] private TextMeshProUGUI tWarriorPoints;
    private string ADPoints = string.Empty;
    [SerializeField] private TextMeshProUGUI tADPoints;
    private string MagePoints = string.Empty;
    [SerializeField] private TextMeshProUGUI tMagePoints;

    [SerializeField] private GameObject[] Skillpanel = new GameObject[7];
    [SerializeField] private Text[] tSkillPoints = new Text[7];
    private bool[] panelsonoff = new bool[7];
    public bool panelonoff = false;

    private void Awake()
    {
        firebaseApp = FirebaseApp.DefaultInstance;
        firebaseDatabase = FirebaseDatabase.GetInstance(firebaseApp, "https://acrobatgames-f9ba6-default-rtdb.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        panelsonoff[0] = false;
        panelsonoff[1] = false;
        panelsonoff[2] = false;
        panelsonoff[3] = false;
        panelsonoff[4] = false;
        panelsonoff[5] = false;
        panelsonoff[6] = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        reference.Child("users").Child(Userid).Child("Skill").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("failed reading...");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("스킬 로딩 완료 떳음 1 / 2");
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
                Debug.Log("인포 로딩 완료 2 / 2");
                DataSnapshot snapshot = task.Result;

                Gold = snapshot.Child("Gold").Value.ToString();
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

        Skill1_Level = snapshot.Child("Skill1").Child("Skill1_Level").Value.ToString();
        Skill2_Level = snapshot.Child("Skill2").Child("Skill2_Level").Value.ToString();
        Skill3_Level = snapshot.Child("Skill3").Child("Skill3_Level").Value.ToString();
        Skill4_Level = snapshot.Child("Skill4").Child("Skill4_Level").Value.ToString();
        Skill5_Level = snapshot.Child("Skill5").Child("Skill5_Level").Value.ToString();
        Skill6_Level = snapshot.Child("Skill6").Child("Skill6_Level").Value.ToString();
        Skill7_Level = snapshot.Child("Skill7").Child("Skill7_Level").Value.ToString();
        SkillPoint = snapshot.Child("SkillPoints").Value.ToString();

        tSkill1_Level.text = "lv. " + Skill1_Level;
        tSkill2_Level.text = "lv. " + Skill2_Level;
        tSkill3_Level.text = "lv. " + Skill3_Level;
        tSkill4_Level.text = "lv. " + Skill4_Level;
        tSkill5_Level.text = "lv. " + Skill5_Level;
        tSkill6_Level.text = "lv. " + Skill6_Level;
        tSkill7_Level.text = "lv. " + Skill7_Level;
        tGold.text = "Gold : " + Gold;

        for (int i = 0; i < 7; i++)
        {
            tSkillPoints[i].text = "x " + SkillPoint;
        } 

        Debug.Log("UI 업데이트 종료");
    }


    public void SkillGoMain()
    {
        LoadingSceneController.LoadScene("StartScene");
    }

    public void SCloseNotEnough()
    {
        NotEnough.SetActive(false);
    }

    public void CloseSkillPanel(int i)
    {
        Skillpanel[i].SetActive(false);
    }

    //힐
    public void Skill1UPBtn()
    {
        if(int.Parse(SkillPoint) > 0)
        {
            int SP = int.Parse(SkillPoint) - 1;
            int SL = int.Parse(Skill1_Level) + 1;

            Dictionary<string, object> update = new Dictionary<string, object>();
            Dictionary<string, object> updatet = new Dictionary<string, object>();
            update.Add("SkillPoints", SP);
            updatet.Add("Skill1_Level", SL);
            reference.Child("users").Child(Userid).Child("Skill").UpdateChildrenAsync(update);
            reference.Child("users").Child(Userid).Child("Skill").Child("Skill1").UpdateChildrenAsync(updatet);

            SkillPoint = SP.ToString();
            Skill1_Level = SL.ToString();
            for (int i = 0; i < 7; i++)
            {
                tSkillPoints[i].text = "x " + SkillPoint;
            }
            tSkill1_Level.text = "lv. " + Skill1_Level;
        }
        else if (int.Parse(SkillPoint) <= 0)
        {
            NotEnough.SetActive(true);
        }
        else
        {
            Debug.LogError("오류발생");
        }
    }

    public void SkillInfo(int i)
    {
        Skillpanel[i].SetActive(true);
    }

    //배리어
    public void Skill2UPBtn()
    {
        if (int.Parse(SkillPoint) > 0)
        {
            int SP = int.Parse(SkillPoint) - 1;
            int SL = int.Parse(Skill2_Level) + 1;

            Dictionary<string, object> update = new Dictionary<string, object>();
            Dictionary<string, object> updatet = new Dictionary<string, object>();
            update.Add("SkillPoints", SP);
            updatet.Add("Skill2_Level", SL);
            reference.Child("users").Child(Userid).Child("Skill").UpdateChildrenAsync(update);
            reference.Child("users").Child(Userid).Child("Skill").Child("Skill2").UpdateChildrenAsync(updatet);

            SkillPoint = SP.ToString();
            Skill2_Level = SL.ToString();
            for (int i = 0; i < 7; i++)
            {
                tSkillPoints[i].text = "x " + SkillPoint;
            }
            tSkill2_Level.text = "lv. " + Skill2_Level;
        }
        else if (int.Parse(SkillPoint) <= 0)
        {
            NotEnough.SetActive(true);
        }
        else
        {
            Debug.LogError("오류발생");
        }
    }

    //텔레포트
    public void Skill3UPBtn()
    {
        if (int.Parse(SkillPoint) > 0)
        {
            int SP = int.Parse(SkillPoint) - 1;
            int SL = int.Parse(Skill3_Level) + 1;

            Dictionary<string, object> update = new Dictionary<string, object>();
            Dictionary<string, object> updatet = new Dictionary<string, object>();
            update.Add("SkillPoints", SP);
            updatet.Add("Skill3_Level", SL);
            reference.Child("users").Child(Userid).Child("Skill").UpdateChildrenAsync(update);
            reference.Child("users").Child(Userid).Child("Skill").Child("Skill3").UpdateChildrenAsync(updatet);

            SkillPoint = SP.ToString();
            Skill3_Level = SL.ToString();
            for (int i = 0; i < 7; i++)
            {
                tSkillPoints[i].text = "x " + SkillPoint;
            }
            tSkill3_Level.text = "lv. " + Skill3_Level;
        }
        else if (int.Parse(SkillPoint) <= 0)
        {
            NotEnough.SetActive(true);
        }
        else
        {
            Debug.LogError("오류발생");
        }
    }

    //정화
    public void Skill4UPBtn()
    {
        if (int.Parse(SkillPoint) > 0)
        {
            int SP = int.Parse(SkillPoint) - 1;
            int SL = int.Parse(Skill4_Level) + 1;

            Dictionary<string, object> update = new Dictionary<string, object>();
            Dictionary<string, object> updatet = new Dictionary<string, object>();
            update.Add("SkillPoints", SP);
            updatet.Add("Skill4_Level", SL);
            reference.Child("users").Child(Userid).Child("Skill").UpdateChildrenAsync(update);
            reference.Child("users").Child(Userid).Child("Skill").Child("Skill4").UpdateChildrenAsync(updatet);

            SkillPoint = SP.ToString();
            Skill4_Level = SL.ToString();
            for (int i = 0; i < 7; i++)
            {
                tSkillPoints[i].text = "x " + SkillPoint;
            }
            tSkill4_Level.text = "lv. " + Skill4_Level;
        }
        else if (int.Parse(SkillPoint) <= 0)
        {
            NotEnough.SetActive(true);
        }
        else
        {
            Debug.LogError("오류발생");
        }
    }

    //공격속도 증가
    public void Skill5UPBtn()
    {
        if (int.Parse(SkillPoint) > 0)
        {
            int SP = int.Parse(SkillPoint) - 1;
            int SL = int.Parse(Skill5_Level) + 1;

            Dictionary<string, object> update = new Dictionary<string, object>();
            Dictionary<string, object> updatet = new Dictionary<string, object>();
            update.Add("SkillPoints", SP);
            updatet.Add("Skill5_Level", SL);
            reference.Child("users").Child(Userid).Child("Skill").UpdateChildrenAsync(update);
            reference.Child("users").Child(Userid).Child("Skill").Child("Skill5").UpdateChildrenAsync(updatet);

            SkillPoint = SP.ToString();
            Skill5_Level = SL.ToString();
            for (int i = 0; i < 7; i++)
            {
                tSkillPoints[i].text = "x " + SkillPoint;
            }
            tSkill5_Level.text = "lv. " + Skill5_Level;
        }
        else if (int.Parse(SkillPoint) <= 0)
        {
            NotEnough.SetActive(true);
        }
        else
        {
            Debug.LogError("오류발생");
        }
    }

    //공격 1
    public void Skill6UPBtn()
    {
        if (int.Parse(SkillPoint) > 0)
        {
            int SP = int.Parse(SkillPoint) - 1;
            int SL = int.Parse(Skill6_Level) + 1;

            Dictionary<string, object> update = new Dictionary<string, object>();
            Dictionary<string, object> updatet = new Dictionary<string, object>();
            update.Add("SkillPoints", SP);
            updatet.Add("Skill6_Level", SL);
            reference.Child("users").Child(Userid).Child("Skill").UpdateChildrenAsync(update);
            reference.Child("users").Child(Userid).Child("Skill").Child("Skill6").UpdateChildrenAsync(updatet);

            SkillPoint = SP.ToString();
            Skill6_Level = SL.ToString();
            for (int i = 0; i < 7; i++)
            {
                tSkillPoints[i].text = "x " + SkillPoint;
            }
            tSkill6_Level.text = "lv. " + Skill6_Level;
        }
        else if (int.Parse(SkillPoint) <= 0)
        {
            NotEnough.SetActive(true);
        }
        else
        {
            Debug.LogError("오류발생");
        }
    }

    //공격2
    public void Skill7UPBtn()
    {
        if (int.Parse(SkillPoint) > 0)
        {
            int SP = int.Parse(SkillPoint) - 1;
            int SL = int.Parse(Skill7_Level) + 1;

            Dictionary<string, object> update = new Dictionary<string, object>();
            Dictionary<string, object> updatet = new Dictionary<string, object>();
            update.Add("SkillPoints", SP);
            updatet.Add("Skill7_Level", SL);
            reference.Child("users").Child(Userid).Child("Skill").UpdateChildrenAsync(update);
            reference.Child("users").Child(Userid).Child("Skill").Child("Skill7").UpdateChildrenAsync(updatet);

            SkillPoint = SP.ToString();
            Skill7_Level = SL.ToString();
            for (int i = 0; i < 7; i++)
            {
                tSkillPoints[i].text = "x " + SkillPoint;
            }
            tSkill7_Level.text = "lv. " + Skill7_Level;
        }
        else if (int.Parse(SkillPoint) <= 0)
        {
            NotEnough.SetActive(true);
        }
        else
        {
            Debug.LogError("오류발생");
        }
    }
}
