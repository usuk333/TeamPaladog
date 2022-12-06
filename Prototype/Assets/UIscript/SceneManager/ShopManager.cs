using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject unitInfoObj;
    [SerializeField] private Sprite[] unitIconArray;
    [SerializeField] private string[] unitInfoArray;
    [SerializeField] private Sprite[] unitIllustArray;

    [Header("변경될 UI 요소들")]
    [SerializeField] private Image profileImage;
    [SerializeField] private Text levelText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private Text expText;
    [SerializeField] private Text atkText;
    [SerializeField] private Text hpText;
    [SerializeField] private Text infoText;
    [SerializeField] private Image illustImage;
    [SerializeField] private Text nameText;
    [SerializeField] private Text skillNameText;
    [SerializeField] private Text skillInfoText;
    [SerializeField] private Text goldText;
    [SerializeField] private Text[] unitPointTextArray;

    private string[] unitNameArray = { "탱커 - 길버트", "암살자 - 하나", "마법사 - 하람", "궁수 - 로젤리아" };
    private string[] unitSkillNameArray = { "특수 공격 : 흡수의 일격", "특수 공격 : 일렁이는 아지랑이", "특수공격 : 일어나는 업화", "특수공격 : 피어스" };
    private string[] unitPathArray = { "Warrior", "Assassin" , "Magician", "Archor" };
    private string[] statusPathArray = { "ATK", "EXP", "HP", "Level" };
    private string[] unitPointPathArray = { "WarriorPoints", "AssassinPoints", "MagePoints", "ADPoints" };

    private string currentUnit;

    private void Start()
    {
        goldText.text = GetInfoDataToString("Gold") + " 골드";
        for (int i = 0; i < unitPointTextArray.Length; i++)
        {
            unitPointTextArray[i].text = GetInfoDataToString(unitPointPathArray[i]);
        }
    }
    public void BtnEvt_ActiveUnitInfoObj(int i)
    {
        if (!unitInfoObj.activeSelf)
        {
            unitInfoObj.SetActive(!unitInfoObj.activeSelf);
        }
        if (unitInfoObj.activeSelf)
        {
            ChangeUnitInfo(i);
        }
    }
    private void ChangeUnitInfo(int i)
    {
        currentUnit = unitPathArray[i];
        profileImage.sprite = unitIconArray[i];
        infoText.text = unitInfoArray[i];
        nameText.text = unitNameArray[i];
        skillNameText.text = unitSkillNameArray[i];
        //illustImage.sprite = unitIllustArray[i];
        ChangeUnitData(i);
    }
    private void ChangeUnitData(int i)
    {
        atkText.text = "공격력 : " + StartManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[0]}"] + " / " + DataEquation.UnitMaxAtkEquationToLevel(unitPathArray[i]);
        expText.text = StartManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[1]}"] + " / 500";
        hpText.text = "체력 : " + StartManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[2]}"] + " / " + DataEquation.UnitMaxHpEquationToLevel(unitPathArray[i]);
        levelText.text = "LV : " + StartManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[3]}"];
        expSlider.value = System.Convert.ToInt32(StartManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[1]}"]);
        ChangeSkillInfo(i);
    }
    private void ChangeSkillInfo(int i)
    {
        switch (i)
        {
            case 0:
                skillInfoText.text = $"전설의 도끼 '파라슈'의 힘으로 상대를 내려쳐({DataEquation.UnitSkillConditionToLevel(currentUnit)})번 째 공격마다 공격력(+150%) 만큼의 피해를 입히고 공격력(+200%) 만큼의 체력을 회복합니다";
                break;
            case 1:
                skillInfoText.text = $"아지랑이가 일만큼 빠르게 상대를 베어내 ({DataEquation.UnitSkillConditionToLevel(currentUnit)}%) 확률로 공격력(+200%) 만큼의 피해를 입힙니다.";
                break;
            case 2:
                skillInfoText.text = $"강력한 마법으로 상대를 불살라 ({DataEquation.UnitSkillConditionToLevel(currentUnit)})번 째 공격마다 공격력(+400%) 만큼의 피해를 입힙니다.";
                break;
            case 3:
                skillInfoText.text = $"강력한 가시로 상대를 꿰뚫어 ({DataEquation.UnitSkillConditionToLevel(currentUnit)}%) 확률로 공격력(+180%) 만큼의 피해를 입힙니다.";
                break;
            default:
                break;
        }
    }
    private string GetInfoDataToString(string path)
    {
        string str;

        if(path == "Gold")
        {
            str = DataEquation.GetUnit(System.Convert.ToInt32(StartManager.Instance.FirebaseData.InfoDictionary[path]));
        }
        else
        {
            str = StartManager.Instance.FirebaseData.InfoDictionary[path].ToString();
        }
        return str;
    }
    //public class UserInfo
    //{
    //    public int Gold = 0;
    //}

    //[SerializeField] private string Userid;

    //private DatabaseReference reference;

    //private bool isLoad;

    //private string Gold = string.Empty;

    ////유닛 레벨
    //private string Assassin_level = string.Empty;
    //[SerializeField] private Text tAssassin_level;
    //private string Warrior_level = string.Empty;
    //[SerializeField] private Text tWarrior_level;
    //private string Archor_level = string.Empty;
    //[SerializeField] private Text tArchor_level;
    //private string Magician_level = string.Empty;
    //[SerializeField] private Text tMagician_level;

    ////유닛 경험치
    //private string Assassin_EXP = string.Empty;
    //[SerializeField] private Text tAssassin_EXP;
    //private string Warrior_EXP = string.Empty;
    //[SerializeField] private Text tWarrior_EXP;
    //private string Archor_EXP = string.Empty;
    //[SerializeField] private Text tArchor_EXP;
    //private string Magician_EXP = string.Empty;
    //[SerializeField] private Text tMagician_EXP;

    ////계열 포인트
    //private string WarriorPoints = string.Empty;
    //[SerializeField] private TextMeshProUGUI tWarriorPoints;
    //private string AssassinPoints = string.Empty;
    //[SerializeField] private TextMeshProUGUI tAssassinPoints;
    //private string ADPoints = string.Empty;
    //[SerializeField] private TextMeshProUGUI tADPoints;
    //private string MagePoints = string.Empty;
    //[SerializeField] private TextMeshProUGUI tMagePoints;

    //private string AssassinHP = string.Empty;
    //[SerializeField] private Text tAssassinHP;
    //private string AssassinATK = string.Empty;
    //[SerializeField] private Text tAssassinATK;

    //private string WarriorHP = string.Empty;
    //[SerializeField] private Text tWarriorHP;
    //private string WarriorATK = string.Empty;
    //[SerializeField] private Text tWarriorATK;

    //private string ArchorHP = string.Empty;
    //[SerializeField] private Text tArchorHP;
    //private string ArchorATK = string.Empty;
    //[SerializeField] private Text tArchorATK;

    //private string MagicianHP = string.Empty;
    //[SerializeField] private Text tMagicianHP;
    //private string MagicianATK = string.Empty;
    //[SerializeField] private Text tMagicianATK;


    //[SerializeField] private TextMeshProUGUI tGold;

    //[SerializeField] private GameObject NotEnough;

    //[SerializeField] private GameObject[] Jobpanel = new GameObject[4];
    //private bool[] panelsonoff = new bool[4];

    //public bool panelonoff = false;

    //DataSnapshot snapshot;
    //DataSnapshot snapshots;

    //FirebaseDatabase firebaseDatabase;

    //FirebaseApp firebaseApp;

    //void Awake()
    //{
    //    firebaseApp = FirebaseApp.DefaultInstance;
    //    firebaseDatabase = FirebaseDatabase.GetInstance(firebaseApp, "https://acrobatgames-f9ba6-default-rtdb.firebaseio.com/");
    //    reference = FirebaseDatabase.DefaultInstance.RootReference;

    //    panelsonoff[0] = false;
    //    panelsonoff[1] = false;
    //    panelsonoff[2] = false;
    //    panelsonoff[3] = false;

    //    NotEnough.SetActive(false);
    //}

    //// Start is called before the first frame update
    //void Start()
    //{
    //    //Usersid = "NaIxowYCsaSqdaYaWtWbIYErkqM2";
    //    //reference.Child("users").Child(Userid).GetValueAsync().ContinueWith
    //    reference.Child("users").Child(Userid).Child("Unit").GetValueAsync().ContinueWithOnMainThread(task =>
    //    {
    //        if (task.IsFaulted)
    //        {
    //            Debug.LogError("failed reading...");
    //        }
    //        else if (task.IsCompleted)
    //        {
    //            Debug.Log("컴플릿떳음");
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
    //            Debug.Log("INFO 컴플릿떳음");
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

    //    //Unitpoints = snapshot.Child("UnitPoints").Value.ToString();

    //    Gold = snapshots.Child("Gold").Value.ToString();

    //    //계열포인트
    //    WarriorPoints = snapshots.Child("Points").Child("WarriorPoints").Value.ToString();
    //    AssassinPoints = snapshots.Child("Points").Child("AssassinPoints").Value.ToString();
    //    ADPoints = snapshots.Child("Points").Child("ADPoints").Value.ToString();
    //    MagePoints = snapshots.Child("Points").Child("MagePoints").Value.ToString();

    //    //유닛 레벨
    //    Warrior_level = snapshot.Child("Warrior").Child("Level").Value.ToString();
    //    Assassin_level = snapshot.Child("Assassin").Child("Level").Value.ToString();
    //    Archor_level = snapshot.Child("Archor").Child("Level").Value.ToString();
    //    Magician_level = snapshot.Child("Magician").Child("Level").Value.ToString();

    //    //유닛 경험치
    //    Warrior_EXP = snapshot.Child("Warrior").Child("EXP").Value.ToString();
    //    Assassin_EXP = snapshot.Child("Assassin").Child("EXP").Value.ToString();
    //    Archor_EXP = snapshot.Child("Archor").Child("EXP").Value.ToString();
    //    Magician_EXP = snapshot.Child("Magician").Child("EXP").Value.ToString();

    //    //HP, ATK
    //    WarriorHP = snapshot.Child("Warrior").Child("HP").Value.ToString();
    //    WarriorATK = snapshot.Child("Warrior").Child("ATK").Value.ToString();
    //    AssassinHP = snapshot.Child("Assassin").Child("HP").Value.ToString();
    //    AssassinATK = snapshot.Child("Assassin").Child("ATK").Value.ToString();
    //    ArchorHP = snapshot.Child("Archor").Child("HP").Value.ToString();
    //    ArchorATK = snapshot.Child("Archor").Child("ATK").Value.ToString();
    //    MagicianHP = snapshot.Child("Magician").Child("HP").Value.ToString();
    //    MagicianATK = snapshot.Child("Magician").Child("ATK").Value.ToString();

    //    Debug.Log("스냅샷 자식 수는 " + snapshot.ChildrenCount);

    //    //UI에 표시
    //    tWarriorPoints.text = "x " + WarriorPoints;
    //    tAssassinPoints.text = "x " + AssassinPoints;
    //    tADPoints.text = "x " + ADPoints;
    //    tMagePoints.text = "x " + MagePoints;

    //    tWarrior_EXP.text = Warrior_EXP + " / 500";
    //    tAssassin_EXP.text = Assassin_EXP + " / 500";
    //    tArchor_EXP.text = Archor_EXP + " / 500";
    //    tMagician_EXP.text = Magician_EXP + " / 500";

    //    tGold.text = "Gold : " + Gold;
    //    tWarrior_level.text = "LV : " + Warrior_level;
    //    tAssassin_level.text = "LV : " + Assassin_level;
    //    tArchor_level.text = "LV : " + Archor_level;
    //    tMagician_level.text = "LV : " + Magician_level;

    //    tWarriorHP.text = "체력 : " + WarriorHP;
    //    tWarriorATK.text = "공격력 : " + WarriorATK;
    //    tAssassinHP.text = "체력 : " + AssassinHP;
    //    tAssassinATK.text = "공격력 : " + AssassinATK;
    //    tArchorHP.text = "체력 : " + ArchorHP;
    //    tArchorATK.text = "공격력 : " + ArchorATK;
    //    tMagicianHP.text = "체력 : " + MagicianHP;
    //    tMagicianATK.text = "공격력 : " + MagicianATK;

    //    //HP 공식 추가되면 함수 만들기
    //}

    ////메인으로 버튼
    //public void ShopGoMain()
    //{
    //    LoadingSceneController.LoadScene("StartScene");
    //}

    //public void CloseNotEnough()
    //{
    //    NotEnough.SetActive(false);
    //}

    //public void JobInfo(int i)
    //{
    //    if(panelonoff == false)
    //    {
    //        Jobpanel[i].SetActive(true);
    //        panelsonoff[i] = true;

    //        panelonoff = true;
    //    }
    //    else
    //    {
    //        for (int p =0; p < panelsonoff.Length; p++)
    //        {
    //            if(panelsonoff[p] == true)
    //            {
    //                Jobpanel[p].SetActive(false);
    //                panelsonoff[p] = false;
    //            }
    //        }
    //        Jobpanel[i].SetActive(true);
    //        panelsonoff[i] = true;
    //    }
    //}

    ////탱커 경험치 증가
    //public void WarriorEXPUp()
    //{
    //    if (int.Parse(WarriorPoints) > 0)
    //    {
    //        int UE = int.Parse(Warrior_EXP) + 100; //경험치
    //        int WP = int.Parse(WarriorPoints) - 1;   //탱커 계열 포인트 - 1


    //        if (UE >= 500)       //EXP를 다 채워서 레벨업 루트
    //        {
    //            UE -= 500;
    //            int UL = int.Parse(Warrior_level) + 1;

    //            Dictionary<string, object> update = new Dictionary<string, object>();
    //            Dictionary<string, object> updatet = new Dictionary<string, object>();
    //            Dictionary<string, object> updatee = new Dictionary<string, object>();
    //            update.Add("Level", UL);
    //            updatet.Add("WarriorPoints", WP);
    //            updatee.Add("EXP", UE);
    //            reference.Child("users").Child(Userid).Child("Unit").Child("Warrior").UpdateChildrenAsync(update);
    //            reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatet);
    //            reference.Child("users").Child(Userid).Child("Unit").Child("Warrior").UpdateChildrenAsync(updatee);

    //            Warrior_level = UL.ToString();
    //            tWarrior_level.text = "LV : " + Warrior_level;
    //            Warrior_EXP = UE.ToString();
    //            tWarrior_EXP.text = Warrior_EXP + " / 500";
    //            WarriorPoints = WP.ToString();
    //            tWarriorPoints.text = "x " + WarriorPoints;
    //        }
    //        else if (UE < 500)      //EXP가 다 안찬 루트
    //        {
    //            Warrior_EXP = UE.ToString();

    //            Dictionary<string, object> update = new Dictionary<string, object>();
    //            update.Add("EXP", UE);
    //            reference.Child("users").Child(Userid).Child("Unit").Child("Warrior").UpdateChildrenAsync(update);

    //            Warrior_EXP = UE.ToString();
    //            tWarrior_EXP.text = Warrior_EXP + " / 500";
    //            WarriorPoints = WP.ToString();
    //            tWarriorPoints.text = "x " + WarriorPoints;
    //        }
    //    }
    //    else if (int.Parse(Gold) < 100)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("오류 발생");
    //    }
    //}

    ////탱커 공격력 증가
    //public void WarriorATKUp()
    //{
    //    if(int.Parse(Gold) >= 200)
    //    {
    //        if(int.Parse(WarriorATK) < int.Parse(Warrior_level) * 10)        //레벨 한계에 도달X & 공격력 증가
    //        {
    //            int Golds = int.Parse(Gold) - 200;
    //            int SATK = int.Parse(WarriorATK) + 2;

    //            Dictionary<string, object> update = new Dictionary<string, object>();
    //            Dictionary<string, object> updatet = new Dictionary<string, object>();
    //            update.Add("ATK", SATK);
    //            updatet.Add("Gold", Golds);
    //            reference.Child("users").Child(Userid).Child("Unit").Child("Warrior").UpdateChildrenAsync(update);
    //            reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updatet);

    //            Gold = Golds.ToString();
    //            tGold.text = "Gold : " + Gold;
    //            WarriorATK = SATK.ToString();
    //            tWarriorATK.text = "공격력 : " + WarriorATK;
    //        }
    //        else if (int.Parse(WarriorATK) >= int.Parse(Warrior_level) * 10)
    //        {
    //            Debug.LogError("한계치 입니다");
    //        }
    //    }
    //    else if (int.Parse(Gold) < 200)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("오류 발생");
    //    }
    //}

    ////탱커 HP 증가
    //public void WarriorHPUp()
    //{
    //    if (int.Parse(Gold) >= 200)
    //    {
    //        if (int.Parse(WarriorHP) < int.Parse(Warrior_level) * 100)        //레벨 한계에 도달X & 공격력 증가
    //        {
    //            int Golds = int.Parse(Gold) - 200;
    //            int SHP = int.Parse(WarriorHP) + 20;

    //            Dictionary<string, object> update = new Dictionary<string, object>();
    //            Dictionary<string, object> updatet = new Dictionary<string, object>();
    //            update.Add("Warrior_HP", SHP);
    //            updatet.Add("Gold", Golds);
    //            reference.Child("users").Child(Userid).Child("Unit").Child("Warrior").UpdateChildrenAsync(update);
    //            reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updatet);

    //            Gold = Golds.ToString();
    //            tGold.text = "Gold : " + Gold;
    //            WarriorHP = SHP.ToString();
    //            tWarriorHP.text = "체력 : " + WarriorHP;
    //        }
    //        else if (int.Parse(WarriorHP) >= int.Parse(Warrior_level) * 100)
    //        {
    //            Debug.LogError("한계치 입니다");
    //        }
    //    }
    //    else if (int.Parse(Gold) < 200)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("오류 발생");
    //    }
    //}

    ////전사 레벨 업
    //public void AssassinEXPUp()
    //{
    //    if (int.Parse(AssassinPoints) > 0)
    //    {
    //        int UE = int.Parse(Assassin_EXP) + 100; //경험치   
    //        int AP = int.Parse(AssassinPoints) - 1;   //전사 계열 포인트 - 1


    //        if (UE >= 500)       //EXP를 다 채워서 레벨업 루트
    //        {
    //            UE -= 500;
    //            int UL = int.Parse(Assassin_level) + 1;

    //            Dictionary<string, object> update = new Dictionary<string, object>();
    //            Dictionary<string, object> updatet = new Dictionary<string, object>();
    //            Dictionary<string, object> updatee = new Dictionary<string, object>();
    //            update.Add("Level", UL);
    //            updatet.Add("WarriorPoints", AP);
    //            updatee.Add("EXP", UE);
    //            reference.Child("users").Child(Userid).Child("Unit").Child("Assassin").UpdateChildrenAsync(update);
    //            reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatet);
    //            reference.Child("users").Child(Userid).Child("Unit").Child("Assassin").UpdateChildrenAsync(updatee);

    //            Assassin_level = UL.ToString();
    //            tAssassin_level.text = "LV : " + Assassin_level;
    //            Assassin_EXP = UE.ToString();
    //            tAssassin_EXP.text = Assassin_EXP + " / 500";
    //            AssassinPoints = AP.ToString();
    //            tAssassinPoints.text = "x " + AssassinPoints;
    //        }
    //        else if (UE < 500)      //EXP가 다 안찬 루트
    //        {
    //            Assassin_EXP = UE.ToString();

    //            Dictionary<string, object> update = new Dictionary<string, object>();
    //            update.Add("EXP", UE);
    //            reference.Child("users").Child(Userid).Child("Unit").Child("Assassin").UpdateChildrenAsync(update);

    //            Assassin_EXP = UE.ToString();
    //            tAssassin_EXP.text = Assassin_EXP + " / 500";
    //            AssassinPoints = AP.ToString();
    //            tAssassinPoints.text = "x " + AssassinPoints;
    //        }
    //    }
    //    else if (int.Parse(Gold) < 100)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("오류 발생");
    //    }
    //}

    ////암살자 공격력 증가
    //public void AssassinATKUp()
    //{
    //    if (int.Parse(Gold) >= 200)
    //    {
    //        if (int.Parse(AssassinATK) < int.Parse(Assassin_level) * 10)        //레벨 한계에 도달X & 공격력 증가
    //        {
    //            int Golds = int.Parse(Gold) - 200;
    //            int AATK = int.Parse(AssassinATK) + 2;

    //            Dictionary<string, object> update = new Dictionary<string, object>();
    //            Dictionary<string, object> updatet = new Dictionary<string, object>();
    //            update.Add("ATK", AATK);
    //            updatet.Add("Gold", Golds);
    //            reference.Child("users").Child(Userid).Child("Unit").Child("Assassin").UpdateChildrenAsync(update);
    //            reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updatet);

    //            Gold = Golds.ToString();
    //            tGold.text = "Gold : " + Gold;
    //            AssassinATK = AATK.ToString();
    //            tAssassinATK.text = "공격력 : " + AssassinATK;
    //        }
    //        else if (int.Parse(AssassinATK) >= int.Parse(Assassin_level) * 10)
    //        {
    //            Debug.LogError("한계치 입니다");
    //        }
    //    }
    //    else if (int.Parse(Gold) < 200)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("오류 발생");
    //    }
    //}

    ////전사 HP 증가
    //public void AssassinHPUp()
    //{
    //    if (int.Parse(Gold) >= 200)
    //    {
    //        if (int.Parse(AssassinHP) < int.Parse(Assassin_level) * 100)        //레벨 한계에 도달X & 공격력 증가
    //        {
    //            int Golds = int.Parse(Gold) - 200;
    //            int AHP = int.Parse(AssassinHP) + 20;

    //            Dictionary<string, object> update = new Dictionary<string, object>();
    //            Dictionary<string, object> updatet = new Dictionary<string, object>();
    //            update.Add("HP", AHP);
    //            updatet.Add("Gold", Golds);
    //            reference.Child("users").Child(Userid).Child("Unit").Child("Assassin").UpdateChildrenAsync(update);
    //            reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updatet);

    //            Gold = Golds.ToString();
    //            tGold.text = "Gold : " + Gold;
    //            AssassinHP = AHP.ToString();
    //            tAssassinHP.text = "체력 : " + AssassinHP;
    //        }
    //        else if (int.Parse(AssassinHP) >= int.Parse(Assassin_level) * 100)
    //        {
    //            Debug.LogError("한계치 입니다");
    //        }
    //    }
    //    else if (int.Parse(Gold) < 200)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("오류 발생");
    //    }
    //}

    ////아처 경험치 업
    //public void ArchorEXPUp()
    //{
    //    if (int.Parse(ADPoints) > 0)
    //    {
    //        int UE = int.Parse(Archor_EXP) + 100; //경험치   
    //        int AP = int.Parse(ADPoints) - 1;   //탱커 계열 포인트 - 1


    //        if (UE >= 500)       //EXP를 다 채워서 레벨업 루트
    //        {
    //            UE -= 500;
    //            int UL = int.Parse(Archor_level) + 1;

    //            Dictionary<string, object> update = new Dictionary<string, object>();
    //            Dictionary<string, object> updatet = new Dictionary<string, object>();
    //            Dictionary<string, object> updatee = new Dictionary<string, object>();
    //            update.Add("Archor_Level", UL);
    //            updatet.Add("ADPoints", AP);
    //            updatee.Add("Archor_EXP", UE);
    //            reference.Child("users").Child(Userid).Child("Unit").Child("Archor").UpdateChildrenAsync(update);
    //            reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatet);
    //            reference.Child("users").Child(Userid).Child("Unit").Child("Archor").UpdateChildrenAsync(updatee);

    //            Archor_level = UL.ToString();
    //            tArchor_level.text = "LV : " + Archor_level;
    //            Archor_EXP = UE.ToString();
    //            tArchor_EXP.text = Archor_EXP + " / 500";
    //            ADPoints = AP.ToString();
    //            tADPoints.text = "x " + ADPoints;
    //        }
    //        else if (UE < 500)      //EXP가 다 안찬 루트
    //        {
    //            Archor_EXP = UE.ToString();

    //            Dictionary<string, object> update = new Dictionary<string, object>();
    //            update.Add("Archor_EXP", UE);
    //            reference.Child("users").Child(Userid).Child("Unit").Child("Archor").UpdateChildrenAsync(update);

    //            Archor_EXP = UE.ToString();
    //            tArchor_EXP.text = Archor_EXP + " / 500";
    //            ADPoints = AP.ToString();
    //            tADPoints.text = "x " + ADPoints;
    //        }
    //    }
    //    else if (int.Parse(Gold) < 100)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("오류 발생");
    //    }
    //}

    ////아처 공격력 증가
    //public void ArchorATKUp()
    //{
    //    if (int.Parse(Gold) >= 200)
    //    {
    //        if (int.Parse(ArchorATK) < int.Parse(Archor_level) * 10)        //레벨 한계에 도달X & 공격력 증가
    //        {
    //            int Golds = int.Parse(Gold) - 200;
    //            int SATK = int.Parse(ArchorATK) + 2;

    //            Dictionary<string, object> update = new Dictionary<string, object>();
    //            Dictionary<string, object> updatet = new Dictionary<string, object>();
    //            update.Add("Archor_ATK", SATK);
    //            updatet.Add("Gold", Golds);
    //            reference.Child("users").Child(Userid).Child("Unit").Child("Archor").UpdateChildrenAsync(update);
    //            reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updatet);

    //            Gold = Golds.ToString();
    //            tGold.text = "Gold : " + Gold;
    //            ArchorATK = SATK.ToString();
    //            tArchorATK.text = "공격력 : " + ArchorATK;
    //        }
    //        else if (int.Parse(ArchorATK) >= int.Parse(Archor_level) * 10)
    //        {
    //            Debug.LogError("한계치 입니다");
    //        }
    //    }
    //    else if (int.Parse(Gold) < 200)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("오류 발생");
    //    }
    //}

    ////아처 HP 증가
    //public void ArchorHPUp()
    //{
    //    if (int.Parse(Gold) >= 200)
    //    {
    //        if (int.Parse(ArchorHP) < int.Parse(Archor_level) * 100)        //레벨 한계에 도달X & 공격력 증가
    //        {
    //            int Golds = int.Parse(Gold) - 200;
    //            int AHP = int.Parse(ArchorHP) + 20;

    //            Dictionary<string, object> update = new Dictionary<string, object>();
    //            Dictionary<string, object> updatet = new Dictionary<string, object>();
    //            update.Add("Archor_HP", AHP);
    //            updatet.Add("Gold", Golds);
    //            reference.Child("users").Child(Userid).Child("Unit").Child("Archor").UpdateChildrenAsync(update);
    //            reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updatet);

    //            Gold = Golds.ToString();
    //            tGold.text = "Gold : " + Gold;
    //            ArchorHP = AHP.ToString();
    //            tArchorHP.text = "체력 : " + ArchorHP;
    //        }
    //        else if (int.Parse(ArchorHP) >= int.Parse(Archor_level) * 100)
    //        {
    //            Debug.LogError("한계치 입니다");
    //        }
    //    }
    //    else if (int.Parse(Gold) < 200)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("오류 발생");
    //    }
    //}

    ////법사류 레벨 업
    //public void MagicianEXPUp()
    //{
    //    if (int.Parse(MagePoints) > 0)
    //    {
    //        int UE = int.Parse(Magician_EXP) + 100; //경험치   
    //        int MP = int.Parse(MagePoints) - 1;   //탱커 계열 포인트 - 1


    //        if (UE >= 500)       //EXP를 다 채워서 레벨업 루트
    //        {
    //            UE -= 500;
    //            int UL = int.Parse(Magician_level) + 1;

    //            Dictionary<string, object> update = new Dictionary<string, object>();
    //            Dictionary<string, object> updatet = new Dictionary<string, object>();
    //            Dictionary<string, object> updatee = new Dictionary<string, object>();
    //            update.Add("Magician_Level", UL);
    //            updatet.Add("MagePoints", MP);
    //            updatee.Add("Magician_EXP", UE);
    //            reference.Child("users").Child(Userid).Child("Unit").Child("Magician").UpdateChildrenAsync(update);
    //            reference.Child("users").Child(Userid).Child("Info").Child("Points").UpdateChildrenAsync(updatet);
    //            reference.Child("users").Child(Userid).Child("Unit").Child("Magician").UpdateChildrenAsync(updatee);

    //            Magician_level = UL.ToString();
    //            tMagician_level.text = "LV : " + Magician_level;
    //            Magician_EXP = UE.ToString();
    //            tMagician_EXP.text = Magician_EXP + " / 500";
    //            MagePoints = MP.ToString();
    //            tMagePoints.text = "x " + MagePoints;
    //        }
    //        else if (UE < 500)      //EXP가 다 안찬 루트
    //        {
    //            Magician_EXP = UE.ToString();

    //            Dictionary<string, object> update = new Dictionary<string, object>();
    //            update.Add("Magician_EXP", UE);
    //            reference.Child("users").Child(Userid).Child("Unit").Child("Magician").UpdateChildrenAsync(update);

    //            Magician_EXP = UE.ToString();
    //            tMagician_EXP.text = Magician_EXP + " / 500";
    //            MagePoints = MP.ToString();
    //            tMagePoints.text = "x " + MagePoints;
    //        }
    //    }
    //    else if (int.Parse(Gold) < 100)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("오류 발생");
    //    }
    //}

    ////법사 공격력 증가
    //public void MagicianATKUp()
    //{
    //    if (int.Parse(Gold) >= 200)
    //    {
    //        if (int.Parse(MagicianATK) < int.Parse(Magician_level) * 10)        //레벨 한계에 도달X & 공격력 증가
    //        {
    //            int Golds = int.Parse(Gold) - 200;
    //            int MATK = int.Parse(MagicianATK) + 2;

    //            Dictionary<string, object> update = new Dictionary<string, object>();
    //            Dictionary<string, object> updatet = new Dictionary<string, object>();
    //            update.Add("Magician_ATK", MATK);
    //            updatet.Add("Gold", Golds);
    //            reference.Child("users").Child(Userid).Child("Unit").Child("Magician").UpdateChildrenAsync(update);
    //            reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updatet);

    //            Gold = Golds.ToString();
    //            tGold.text = "Gold : " + Gold;
    //            MagicianATK = MATK.ToString();
    //            tMagicianATK.text = "공격력 : " + MagicianATK;
    //        }
    //        else if (int.Parse(MagicianATK) >= int.Parse(Magician_level) * 10)
    //        {
    //            Debug.LogError("한계치 입니다");
    //        }
    //    }
    //    else if (int.Parse(Gold) < 200)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("오류 발생");
    //    }
    //}

    ////법사 HP 증가
    //public void MagicianHPUp()
    //{
    //    if (int.Parse(Gold) >= 200)
    //    {
    //        if (int.Parse(MagicianHP) < int.Parse(Magician_level) * 100)        //레벨 한계에 도달X & 공격력 증가
    //        {
    //            int Golds = int.Parse(Gold) - 200;
    //            int MHP = int.Parse(MagicianHP) + 20;

    //            Dictionary<string, object> update = new Dictionary<string, object>();
    //            Dictionary<string, object> updatet = new Dictionary<string, object>();
    //            update.Add("Magician_HP", MHP);
    //            updatet.Add("Gold", Golds);
    //            reference.Child("users").Child(Userid).Child("Unit").Child("Magician").UpdateChildrenAsync(update);
    //            reference.Child("users").Child(Userid).Child("Info").UpdateChildrenAsync(updatet);

    //            Gold = Golds.ToString();
    //            tGold.text = "Gold : " + Gold;
    //            MagicianHP = MHP.ToString();
    //            tMagicianHP.text = "체력 : " + MagicianHP;
    //        }
    //        else if (int.Parse(MagicianHP) >= int.Parse(Magician_level) * 100)
    //        {
    //            Debug.LogError("한계치 입니다");
    //        }
    //    }
    //    else if (int.Parse(Gold) < 200)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("오류 발생");
    //    }
    //}

}
