using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private GameObject skillInfoObj;
    [SerializeField] private Sprite[] skillIconArray;
    [SerializeField] private GameObject warningMessageObj;

    [Header("변경될 UI 요소들")]
    [SerializeField] private Text goldText;
    [SerializeField] private Text skillPointText;
    [SerializeField] private Text skillNameText;
    [SerializeField] private Text skillLevelText;
    [SerializeField] private Image skillIconImage;
    [SerializeField] private Text skillInfoText;
    [SerializeField] private Text warningMessageText;

    private string[] skillNameArray = { "체력 단련", "마나 단련", "아무거나 맞아라!", "보호의 물약", "치유의 물약", "사기진작" };
    private string[] skillPathArray = { "HP", "MP", "Attack", "Barrior", "Heal", "PowerUp"};

    private int currentSkill = -1;

    private void Start()
    {
        goldText.text = DataEquation.GetUnit(Convert.ToInt32(GameManager.Instance.FirebaseData.InfoDictionary["Gold"])) + " 골드";
        skillPointText.text = GameManager.Instance.FirebaseData.SkillDictionary["SkillPoints"].ToString();
    }
    public void BtnEvt_UpgradeSkill()
    {
        if (!CheckHaveSkillPoint()) return;
        UpgradeSkill();
        
    }
    public void BtnEvt_NextSkillInfo()
    {
        currentSkill++;
        if (currentSkill > 5) currentSkill = 0;
        ChangeSkillUI(currentSkill);
    }
    public void BtnEvt_PreviousSkillInfo()
    {
        currentSkill--;
        if (currentSkill < 0) currentSkill = 5;
        ChangeSkillUI(currentSkill);
    }
    private bool CheckHaveSkillPoint()
    {
        int point = Convert.ToInt32(GameManager.Instance.FirebaseData.SkillDictionary["SkillPoints"]);
        if (point <= 0)
        {
            warningMessageText.text = "스킬 돌파석이 부족합니다!";
            StartCoroutine(Co_WarningMessageAnim());
            return false;
        }
        else
        {
            return true;
        }
    }
    private void UpgradeSkill()
    {
        if (CheckSkillLevel())
        {
            int point = Convert.ToInt32(GameManager.Instance.FirebaseData.SkillDictionary["SkillPoints"]);
            point--;
            GameManager.Instance.FirebaseData.SaveData("Skill", "SkillPoints", point);
            skillPointText.text = GameManager.Instance.FirebaseData.SkillDictionary["SkillPoints"].ToString();
            int level = Convert.ToInt32(GameManager.Instance.FirebaseData.SkillDictionary[skillPathArray[currentSkill]]) + 1;
            GameManager.Instance.FirebaseData.SaveData("Skill", skillPathArray[currentSkill], level);
            ChangeSkillUI(currentSkill);
        }
    }
    private bool CheckSkillLevel()
    {
        if (currentSkill < 2) return true;
        int level = Convert.ToInt32(GameManager.Instance.FirebaseData.SkillDictionary[skillPathArray[currentSkill]]);
        if (level >= 5)
        {
            warningMessageText.text = "이미 최대치 입니다!";
            StartCoroutine(Co_WarningMessageAnim());
            return false;
        }
        return true;
    }
    public void BtnEvt_LoadMainScene()
    {
        LoadingSceneController.LoadScene("Main");
    }
    public void BtnEvt_ActiveSkillInfoObj(int i)
    {
        if (!skillInfoObj.activeSelf)
        {
            skillInfoObj.SetActive(!skillInfoObj.activeSelf);
        }
        if(currentSkill == i) return;
        currentSkill = i;
        ChangeSkillUI(i);
    }
    public void BtnEvt_ActiveObject(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }
    private void ChangeSkillUI(int i)
    {
        skillNameText.text = skillNameArray[i];
        skillLevelText.text = $"Lv. {GameManager.Instance.FirebaseData.SkillDictionary[skillPathArray[i]]}";
        skillIconImage.sprite = skillIconArray[i];
        ChangeSkillInfo(i);
    }
    private void ChangeSkillInfo(int i)
    {
        switch (i)
        {
            case 0:
                var (hMax, hRegen) = DataEquation.PlayerSkillHPToLevel();
                skillInfoText.text = $"꾸준한 운동으로 체력을 단련합니다.\n체력 최대량 = ({hMax})\n체력 회복량 = ({hRegen})";
                break;
            case 1:
                var (mMax, mRegen) = DataEquation.PlayerSkillMPToLevel();
                skillInfoText.text = $"지식을 쌓아 마나를 효율적으로 사용할 수 있게 됩니다.\n마나 최대량 = ({mMax})\n마나 회복량 = ({mRegen})";
                break;
            case 2:
                var (apple, rock, bomb) = DataEquation.PlayerSkillAttackToLevel();
                skillInfoText.text = $"손에 잡히는 걸 적에게 던집니다.\n사과, 돌맹이, 폭탄은 각각 ({apple}), ({rock}), ({bomb}) 만큼의 데미지를 가집니다.\n 마나 소모량 = (5)\n쿨타임 = (3) 초";
                break;
            case 3:
                var (bValue, bDuration, bMana, bCool) = DataEquation.PlayerSkillBarriorToLevel();
                skillInfoText.text = $"보호막을 생성하는 물약을 사용해 아군을 보호합니다.\n({bDuration}) 초 간 유지되는 아군 최대 체력 ({bValue}) 만큼의 보호막을 제공한다.\n마나 소모량 = ({bMana})\n쿨타임 = ({bCool}) 초";
                break;
            case 4:
                var (hValue, hMana, hCool) = DataEquation.PlayerSkillHealToLevel();
                skillInfoText.text = $"회복 물약을 사용해 아군을 치유합니다.\n아군 최대 체력의 ({hValue}) 만큼 회복 시킨다.\n마나 소모량 = ({hMana})\n쿨타임 = ({hCool}) 초";
                break;
            case 5:
                var (rValue, rMana) = DataEquation.PlayerSkillRageToLevel();
                skillInfoText.text = $"사기를 북돋아 아군을 강하게 만듭니다.\n아군 공격력을 ({rValue}) 만큼 증가시킵니다.\n마나 소모량 = 초당 ({rMana})\n쿨타임 = (1)초";
                break;
            default:
                break;
        }
    }
    private IEnumerator Co_WarningMessageAnim()
    {
        warningMessageObj.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        warningMessageObj.SetActive(false);
    }
    //private DatabaseReference reference;
    //FirebaseDatabase firebaseDatabase;
    //FirebaseApp firebaseApp;

    //[SerializeField] private string Userid;     //NaIxowYCsaSqdaYaWtWbIYErkqM2


    //DataSnapshot snapshot;
    //DataSnapshot snapshots;

    //[SerializeField] private GameObject NotEnough;

    //private string Gold = string.Empty;
    //[SerializeField] private TextMeshProUGUI tGold;

    //private string SkillPoint = string.Empty;
    //[SerializeField] private Text tSkillPoint;

    //private string Skill1_Level = string.Empty;
    //[SerializeField] private Text tSkill1_Level;
    //private string Skill2_Level = string.Empty;
    //[SerializeField] private Text tSkill2_Level;
    //private string Skill3_Level = string.Empty;
    //[SerializeField] private Text tSkill3_Level;
    //private string Skill4_Level = string.Empty;
    //[SerializeField] private Text tSkill4_Level;
    //private string Skill5_Level = string.Empty;
    //[SerializeField] private Text tSkill5_Level;
    //private string Skill6_Level = string.Empty;
    //[SerializeField] private Text tSkill6_Level;
    //private string Skill7_Level = string.Empty;
    //[SerializeField] private Text tSkill7_Level;

    ////계열 포인트
    //private string TankerPoints = string.Empty;
    //[SerializeField] private TextMeshProUGUI tTankerPoints;
    //private string WarriorPoints = string.Empty;
    //[SerializeField] private TextMeshProUGUI tWarriorPoints;
    //private string ADPoints = string.Empty;
    //[SerializeField] private TextMeshProUGUI tADPoints;
    //private string MagePoints = string.Empty;
    //[SerializeField] private TextMeshProUGUI tMagePoints;

    //[SerializeField] private GameObject[] Skillpanel = new GameObject[7];
    //[SerializeField] private Text[] tSkillPoints = new Text[7];
    //private bool[] panelsonoff = new bool[7];
    //public bool panelonoff = false;

    //private void Awake()
    //{
    //    firebaseApp = FirebaseApp.DefaultInstance;
    //    firebaseDatabase = FirebaseDatabase.GetInstance(firebaseApp, "https://acrobatgames-f9ba6-default-rtdb.firebaseio.com/");
    //    reference = FirebaseDatabase.DefaultInstance.RootReference;

    //    panelsonoff[0] = false;
    //    panelsonoff[1] = false;
    //    panelsonoff[2] = false;
    //    panelsonoff[3] = false;
    //    panelsonoff[4] = false;
    //    panelsonoff[5] = false;
    //    panelsonoff[6] = false;
    //}
    //// Start is called before the first frame update
    //void Start()
    //{
    //    reference.Child("users").Child(Userid).Child("Skill").GetValueAsync().ContinueWithOnMainThread(task =>
    //    {
    //        if (task.IsFaulted)
    //        {
    //            Debug.LogError("failed reading...");
    //        }
    //        else if (task.IsCompleted)
    //        {
    //            Debug.Log("스킬 로딩 완료 떳음 1 / 2");
    //            snapshot = task.Result;

    //            StartCoroutine(UIupdate());
    //        }
    //    });

    //    reference.Child("users").Child(Userid).Child("Info").GetValueAsync().ContinueWithOnMainThread(task =>
    //    {
    //        if (task.IsFaulted)
    //        {
    //            Debug.LogError("failed reading...");
    //        }
    //        else if (task.IsCompleted)
    //        {
    //            Debug.Log("인포 로딩 완료 2 / 2");
    //            snapshots = task.Result;
    //        }
    //    });
    //}

    //private IEnumerator UIupdate()
    //{
    //    yield return null;

    //    if (snapshot == null)
    //    {
    //        Start();
    //    }

    //    Debug.Log("UI업데이트시작");

    //    Gold = snapshots.Child("Gold").Value.ToString();
    //    TankerPoints = snapshots.Child("Points").Child("TankerPoints").Value.ToString();
    //    WarriorPoints = snapshots.Child("Points").Child("WarriorPoints").Value.ToString();
    //    ADPoints = snapshots.Child("Points").Child("ADPoints").Value.ToString();
    //    MagePoints = snapshots.Child("Points").Child("MagePoints").Value.ToString();

    //    Skill1_Level = snapshot.Child("Skill1").Child("Skill1_Level").Value.ToString();
    //    Skill2_Level = snapshot.Child("Skill2").Child("Skill2_Level").Value.ToString();
    //    Skill3_Level = snapshot.Child("Skill3").Child("Skill3_Level").Value.ToString();
    //    Skill4_Level = snapshot.Child("Skill4").Child("Skill4_Level").Value.ToString();
    //    SkillPoint = snapshot.Child("SkillPoints").Value.ToString();

    //    tGold.text = "Gold : " + Gold;
    //    tTankerPoints.text = "x " + TankerPoints;
    //    tWarriorPoints.text = "x " + WarriorPoints;
    //    tADPoints.text = "x " + ADPoints;
    //    tMagePoints.text = "x " + MagePoints;

    //    tSkill1_Level.text = "lv. " + Skill1_Level;
    //    tSkill2_Level.text = "lv. " + Skill2_Level;
    //    tSkill3_Level.text = "lv. " + Skill3_Level;
    //    tSkill4_Level.text = "lv. " + Skill4_Level;
    //    tGold.text = "Gold : " + Gold;

    //    for (int i = 0; i < 4; i++)
    //    {
    //        tSkillPoints[i].text = "x " + SkillPoint;
    //    } 

    //    Debug.Log("UI 업데이트 종료");
    //}


    //public void SkillGoMain()
    //{
    //    LoadingSceneController.LoadScene("StartScene");
    //}

    //public void SCloseNotEnough()
    //{
    //    NotEnough.SetActive(false);
    //}

    //public void CloseSkillPanel(int i)
    //{
    //    Skillpanel[i].SetActive(false);
    //}

    ////힐
    //public void Skill1UPBtn()
    //{
    //    if(int.Parse(SkillPoint) > 0)
    //    {
    //        int SP = int.Parse(SkillPoint) - 1;
    //        int SL = int.Parse(Skill1_Level) + 1;

    //        Dictionary<string, object> update = new Dictionary<string, object>();
    //        Dictionary<string, object> updatet = new Dictionary<string, object>();
    //        update.Add("SkillPoints", SP);
    //        updatet.Add("Skill1_Level", SL);
    //        reference.Child("users").Child(Userid).Child("Skill").UpdateChildrenAsync(update);
    //        reference.Child("users").Child(Userid).Child("Skill").Child("Skill1").UpdateChildrenAsync(updatet);

    //        SkillPoint = SP.ToString();
    //        Skill1_Level = SL.ToString();
    //        for (int i = 0; i < 7; i++)
    //        {
    //            tSkillPoints[i].text = "x " + SkillPoint;
    //        }
    //        tSkill1_Level.text = "lv. " + Skill1_Level;
    //    }
    //    else if (int.Parse(SkillPoint) <= 0)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("오류발생");
    //    }
    //}

    //public void SkillInfo(int i)
    //{
    //    Skillpanel[i].SetActive(true);
    //}

    ////배리어
    //public void Skill2UPBtn()
    //{
    //    if (int.Parse(SkillPoint) > 0)
    //    {
    //        int SP = int.Parse(SkillPoint) - 1;
    //        int SL = int.Parse(Skill2_Level) + 1;

    //        Dictionary<string, object> update = new Dictionary<string, object>();
    //        Dictionary<string, object> updatet = new Dictionary<string, object>();
    //        update.Add("SkillPoints", SP);
    //        updatet.Add("Skill2_Level", SL);
    //        reference.Child("users").Child(Userid).Child("Skill").UpdateChildrenAsync(update);
    //        reference.Child("users").Child(Userid).Child("Skill").Child("Skill2").UpdateChildrenAsync(updatet);

    //        SkillPoint = SP.ToString();
    //        Skill2_Level = SL.ToString();
    //        for (int i = 0; i < 4; i++)
    //        {
    //            tSkillPoints[i].text = "x " + SkillPoint;
    //        }
    //        tSkill2_Level.text = "lv. " + Skill2_Level;
    //    }
    //    else if (int.Parse(SkillPoint) <= 0)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("오류발생");
    //    }
    //}

    ////텔레포트
    //public void Skill3UPBtn()
    //{
    //    if (int.Parse(SkillPoint) > 0)
    //    {
    //        int SP = int.Parse(SkillPoint) - 1;
    //        int SL = int.Parse(Skill3_Level) + 1;

    //        Dictionary<string, object> update = new Dictionary<string, object>();
    //        Dictionary<string, object> updatet = new Dictionary<string, object>();
    //        update.Add("SkillPoints", SP);
    //        updatet.Add("Skill3_Level", SL);
    //        reference.Child("users").Child(Userid).Child("Skill").UpdateChildrenAsync(update);
    //        reference.Child("users").Child(Userid).Child("Skill").Child("Skill3").UpdateChildrenAsync(updatet);

    //        SkillPoint = SP.ToString();
    //        Skill3_Level = SL.ToString();
    //        for (int i = 0; i < 7; i++)
    //        {
    //            tSkillPoints[i].text = "x " + SkillPoint;
    //        }
    //        tSkill3_Level.text = "lv. " + Skill3_Level;
    //    }
    //    else if (int.Parse(SkillPoint) <= 0)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("오류발생");
    //    }
    //}

    ////정화
    //public void Skill4UPBtn()
    //{
    //    if (int.Parse(SkillPoint) > 0)
    //    {
    //        int SP = int.Parse(SkillPoint) - 1;
    //        int SL = int.Parse(Skill4_Level) + 1;

    //        Dictionary<string, object> update = new Dictionary<string, object>();
    //        Dictionary<string, object> updatet = new Dictionary<string, object>();
    //        update.Add("SkillPoints", SP);
    //        updatet.Add("Skill4_Level", SL);
    //        reference.Child("users").Child(Userid).Child("Skill").UpdateChildrenAsync(update);
    //        reference.Child("users").Child(Userid).Child("Skill").Child("Skill4").UpdateChildrenAsync(updatet);

    //        SkillPoint = SP.ToString();
    //        Skill4_Level = SL.ToString();
    //        for (int i = 0; i < 7; i++)
    //        {
    //            tSkillPoints[i].text = "x " + SkillPoint;
    //        }
    //        tSkill4_Level.text = "lv. " + Skill4_Level;
    //    }
    //    else if (int.Parse(SkillPoint) <= 0)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("오류발생");
    //    }
    //}

    ////공격속도 증가
    //public void Skill5UPBtn()
    //{
    //    if (int.Parse(SkillPoint) > 0)
    //    {
    //        int SP = int.Parse(SkillPoint) - 1;
    //        int SL = int.Parse(Skill5_Level) + 1;

    //        Dictionary<string, object> update = new Dictionary<string, object>();
    //        Dictionary<string, object> updatet = new Dictionary<string, object>();
    //        update.Add("SkillPoints", SP);
    //        updatet.Add("Skill5_Level", SL);
    //        reference.Child("users").Child(Userid).Child("Skill").UpdateChildrenAsync(update);
    //        reference.Child("users").Child(Userid).Child("Skill").Child("Skill5").UpdateChildrenAsync(updatet);

    //        SkillPoint = SP.ToString();
    //        Skill5_Level = SL.ToString();
    //        for (int i = 0; i < 7; i++)
    //        {
    //            tSkillPoints[i].text = "x " + SkillPoint;
    //        }
    //        tSkill5_Level.text = "lv. " + Skill5_Level;
    //    }
    //    else if (int.Parse(SkillPoint) <= 0)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("오류발생");
    //    }
    //}

    ////공격 1
    //public void Skill6UPBtn()
    //{
    //    if (int.Parse(SkillPoint) > 0)
    //    {
    //        int SP = int.Parse(SkillPoint) - 1;
    //        int SL = int.Parse(Skill6_Level) + 1;

    //        Dictionary<string, object> update = new Dictionary<string, object>();
    //        Dictionary<string, object> updatet = new Dictionary<string, object>();
    //        update.Add("SkillPoints", SP);
    //        updatet.Add("Skill6_Level", SL);
    //        reference.Child("users").Child(Userid).Child("Skill").UpdateChildrenAsync(update);
    //        reference.Child("users").Child(Userid).Child("Skill").Child("Skill6").UpdateChildrenAsync(updatet);

    //        SkillPoint = SP.ToString();
    //        Skill6_Level = SL.ToString();
    //        for (int i = 0; i < 7; i++)
    //        {
    //            tSkillPoints[i].text = "x " + SkillPoint;
    //        }
    //        tSkill6_Level.text = "lv. " + Skill6_Level;
    //    }
    //    else if (int.Parse(SkillPoint) <= 0)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("오류발생");
    //    }
    //}

    ////공격2
    //public void Skill7UPBtn()
    //{
    //    if (int.Parse(SkillPoint) > 0)
    //    {
    //        int SP = int.Parse(SkillPoint) - 1;
    //        int SL = int.Parse(Skill7_Level) + 1;

    //        Dictionary<string, object> update = new Dictionary<string, object>();
    //        Dictionary<string, object> updatet = new Dictionary<string, object>();
    //        update.Add("SkillPoints", SP);
    //        updatet.Add("Skill7_Level", SL);
    //        reference.Child("users").Child(Userid).Child("Skill").UpdateChildrenAsync(update);
    //        reference.Child("users").Child(Userid).Child("Skill").Child("Skill7").UpdateChildrenAsync(updatet);

    //        SkillPoint = SP.ToString();
    //        Skill7_Level = SL.ToString();
    //        for (int i = 0; i < 7; i++)
    //        {
    //            tSkillPoints[i].text = "x " + SkillPoint;
    //        }
    //        tSkill7_Level.text = "lv. " + Skill7_Level;
    //    }
    //    else if (int.Parse(SkillPoint) <= 0)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("오류발생");
    //    }
    //}
}
