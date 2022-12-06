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

    [Header("����� UI ��ҵ�")]
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

    private string[] unitNameArray = { "��Ŀ - ���Ʈ", "�ϻ��� - �ϳ�", "������ - �϶�", "�ü� - ��������" };
    private string[] unitSkillNameArray = { "Ư�� ���� : ����� �ϰ�", "Ư�� ���� : �Ϸ��̴� ��������", "Ư������ : �Ͼ�� ��ȭ", "Ư������ : �Ǿ" };
    private string[] unitPathArray = { "Warrior", "Assassin" , "Magician", "Archor" };
    private string[] statusPathArray = { "ATK", "EXP", "HP", "Level" };
    private string[] unitPointPathArray = { "WarriorPoints", "AssassinPoints", "MagePoints", "ADPoints" };

    private string currentUnit;

    private void Start()
    {
        goldText.text = GetInfoDataToString("Gold") + " ���";
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
        atkText.text = "���ݷ� : " + StartManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[0]}"] + " / " + DataEquation.UnitMaxAtkEquationToLevel(unitPathArray[i]);
        expText.text = StartManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[1]}"] + " / 500";
        hpText.text = "ü�� : " + StartManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[2]}"] + " / " + DataEquation.UnitMaxHpEquationToLevel(unitPathArray[i]);
        levelText.text = "LV : " + StartManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[3]}"];
        expSlider.value = System.Convert.ToInt32(StartManager.Instance.FirebaseData.UnitDictionary[$"{currentUnit}{statusPathArray[1]}"]);
        ChangeSkillInfo(i);
    }
    private void ChangeSkillInfo(int i)
    {
        switch (i)
        {
            case 0:
                skillInfoText.text = $"������ ���� '�Ķ�'�� ������ ��븦 ������({DataEquation.UnitSkillConditionToLevel(currentUnit)})�� ° ���ݸ��� ���ݷ�(+150%) ��ŭ�� ���ظ� ������ ���ݷ�(+200%) ��ŭ�� ü���� ȸ���մϴ�";
                break;
            case 1:
                skillInfoText.text = $"�������̰� �ϸ�ŭ ������ ��븦 ��� ({DataEquation.UnitSkillConditionToLevel(currentUnit)}%) Ȯ���� ���ݷ�(+200%) ��ŭ�� ���ظ� �����ϴ�.";
                break;
            case 2:
                skillInfoText.text = $"������ �������� ��븦 �һ�� ({DataEquation.UnitSkillConditionToLevel(currentUnit)})�� ° ���ݸ��� ���ݷ�(+400%) ��ŭ�� ���ظ� �����ϴ�.";
                break;
            case 3:
                skillInfoText.text = $"������ ���÷� ��븦 ��վ� ({DataEquation.UnitSkillConditionToLevel(currentUnit)}%) Ȯ���� ���ݷ�(+180%) ��ŭ�� ���ظ� �����ϴ�.";
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

    ////���� ����
    //private string Assassin_level = string.Empty;
    //[SerializeField] private Text tAssassin_level;
    //private string Warrior_level = string.Empty;
    //[SerializeField] private Text tWarrior_level;
    //private string Archor_level = string.Empty;
    //[SerializeField] private Text tArchor_level;
    //private string Magician_level = string.Empty;
    //[SerializeField] private Text tMagician_level;

    ////���� ����ġ
    //private string Assassin_EXP = string.Empty;
    //[SerializeField] private Text tAssassin_EXP;
    //private string Warrior_EXP = string.Empty;
    //[SerializeField] private Text tWarrior_EXP;
    //private string Archor_EXP = string.Empty;
    //[SerializeField] private Text tArchor_EXP;
    //private string Magician_EXP = string.Empty;
    //[SerializeField] private Text tMagician_EXP;

    ////�迭 ����Ʈ
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
    //            Debug.Log("���ø�����");
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
    //            Debug.Log("INFO ���ø�����");
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
    //    Debug.Log("UI������Ʈ����");

    //    //Unitpoints = snapshot.Child("UnitPoints").Value.ToString();

    //    Gold = snapshots.Child("Gold").Value.ToString();

    //    //�迭����Ʈ
    //    WarriorPoints = snapshots.Child("Points").Child("WarriorPoints").Value.ToString();
    //    AssassinPoints = snapshots.Child("Points").Child("AssassinPoints").Value.ToString();
    //    ADPoints = snapshots.Child("Points").Child("ADPoints").Value.ToString();
    //    MagePoints = snapshots.Child("Points").Child("MagePoints").Value.ToString();

    //    //���� ����
    //    Warrior_level = snapshot.Child("Warrior").Child("Level").Value.ToString();
    //    Assassin_level = snapshot.Child("Assassin").Child("Level").Value.ToString();
    //    Archor_level = snapshot.Child("Archor").Child("Level").Value.ToString();
    //    Magician_level = snapshot.Child("Magician").Child("Level").Value.ToString();

    //    //���� ����ġ
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

    //    Debug.Log("������ �ڽ� ���� " + snapshot.ChildrenCount);

    //    //UI�� ǥ��
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

    //    tWarriorHP.text = "ü�� : " + WarriorHP;
    //    tWarriorATK.text = "���ݷ� : " + WarriorATK;
    //    tAssassinHP.text = "ü�� : " + AssassinHP;
    //    tAssassinATK.text = "���ݷ� : " + AssassinATK;
    //    tArchorHP.text = "ü�� : " + ArchorHP;
    //    tArchorATK.text = "���ݷ� : " + ArchorATK;
    //    tMagicianHP.text = "ü�� : " + MagicianHP;
    //    tMagicianATK.text = "���ݷ� : " + MagicianATK;

    //    //HP ���� �߰��Ǹ� �Լ� �����
    //}

    ////�������� ��ư
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

    ////��Ŀ ����ġ ����
    //public void WarriorEXPUp()
    //{
    //    if (int.Parse(WarriorPoints) > 0)
    //    {
    //        int UE = int.Parse(Warrior_EXP) + 100; //����ġ
    //        int WP = int.Parse(WarriorPoints) - 1;   //��Ŀ �迭 ����Ʈ - 1


    //        if (UE >= 500)       //EXP�� �� ä���� ������ ��Ʈ
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
    //        else if (UE < 500)      //EXP�� �� ���� ��Ʈ
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
    //        Debug.LogError("���� �߻�");
    //    }
    //}

    ////��Ŀ ���ݷ� ����
    //public void WarriorATKUp()
    //{
    //    if(int.Parse(Gold) >= 200)
    //    {
    //        if(int.Parse(WarriorATK) < int.Parse(Warrior_level) * 10)        //���� �Ѱ迡 ����X & ���ݷ� ����
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
    //            tWarriorATK.text = "���ݷ� : " + WarriorATK;
    //        }
    //        else if (int.Parse(WarriorATK) >= int.Parse(Warrior_level) * 10)
    //        {
    //            Debug.LogError("�Ѱ�ġ �Դϴ�");
    //        }
    //    }
    //    else if (int.Parse(Gold) < 200)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("���� �߻�");
    //    }
    //}

    ////��Ŀ HP ����
    //public void WarriorHPUp()
    //{
    //    if (int.Parse(Gold) >= 200)
    //    {
    //        if (int.Parse(WarriorHP) < int.Parse(Warrior_level) * 100)        //���� �Ѱ迡 ����X & ���ݷ� ����
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
    //            tWarriorHP.text = "ü�� : " + WarriorHP;
    //        }
    //        else if (int.Parse(WarriorHP) >= int.Parse(Warrior_level) * 100)
    //        {
    //            Debug.LogError("�Ѱ�ġ �Դϴ�");
    //        }
    //    }
    //    else if (int.Parse(Gold) < 200)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("���� �߻�");
    //    }
    //}

    ////���� ���� ��
    //public void AssassinEXPUp()
    //{
    //    if (int.Parse(AssassinPoints) > 0)
    //    {
    //        int UE = int.Parse(Assassin_EXP) + 100; //����ġ   
    //        int AP = int.Parse(AssassinPoints) - 1;   //���� �迭 ����Ʈ - 1


    //        if (UE >= 500)       //EXP�� �� ä���� ������ ��Ʈ
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
    //        else if (UE < 500)      //EXP�� �� ���� ��Ʈ
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
    //        Debug.LogError("���� �߻�");
    //    }
    //}

    ////�ϻ��� ���ݷ� ����
    //public void AssassinATKUp()
    //{
    //    if (int.Parse(Gold) >= 200)
    //    {
    //        if (int.Parse(AssassinATK) < int.Parse(Assassin_level) * 10)        //���� �Ѱ迡 ����X & ���ݷ� ����
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
    //            tAssassinATK.text = "���ݷ� : " + AssassinATK;
    //        }
    //        else if (int.Parse(AssassinATK) >= int.Parse(Assassin_level) * 10)
    //        {
    //            Debug.LogError("�Ѱ�ġ �Դϴ�");
    //        }
    //    }
    //    else if (int.Parse(Gold) < 200)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("���� �߻�");
    //    }
    //}

    ////���� HP ����
    //public void AssassinHPUp()
    //{
    //    if (int.Parse(Gold) >= 200)
    //    {
    //        if (int.Parse(AssassinHP) < int.Parse(Assassin_level) * 100)        //���� �Ѱ迡 ����X & ���ݷ� ����
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
    //            tAssassinHP.text = "ü�� : " + AssassinHP;
    //        }
    //        else if (int.Parse(AssassinHP) >= int.Parse(Assassin_level) * 100)
    //        {
    //            Debug.LogError("�Ѱ�ġ �Դϴ�");
    //        }
    //    }
    //    else if (int.Parse(Gold) < 200)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("���� �߻�");
    //    }
    //}

    ////��ó ����ġ ��
    //public void ArchorEXPUp()
    //{
    //    if (int.Parse(ADPoints) > 0)
    //    {
    //        int UE = int.Parse(Archor_EXP) + 100; //����ġ   
    //        int AP = int.Parse(ADPoints) - 1;   //��Ŀ �迭 ����Ʈ - 1


    //        if (UE >= 500)       //EXP�� �� ä���� ������ ��Ʈ
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
    //        else if (UE < 500)      //EXP�� �� ���� ��Ʈ
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
    //        Debug.LogError("���� �߻�");
    //    }
    //}

    ////��ó ���ݷ� ����
    //public void ArchorATKUp()
    //{
    //    if (int.Parse(Gold) >= 200)
    //    {
    //        if (int.Parse(ArchorATK) < int.Parse(Archor_level) * 10)        //���� �Ѱ迡 ����X & ���ݷ� ����
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
    //            tArchorATK.text = "���ݷ� : " + ArchorATK;
    //        }
    //        else if (int.Parse(ArchorATK) >= int.Parse(Archor_level) * 10)
    //        {
    //            Debug.LogError("�Ѱ�ġ �Դϴ�");
    //        }
    //    }
    //    else if (int.Parse(Gold) < 200)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("���� �߻�");
    //    }
    //}

    ////��ó HP ����
    //public void ArchorHPUp()
    //{
    //    if (int.Parse(Gold) >= 200)
    //    {
    //        if (int.Parse(ArchorHP) < int.Parse(Archor_level) * 100)        //���� �Ѱ迡 ����X & ���ݷ� ����
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
    //            tArchorHP.text = "ü�� : " + ArchorHP;
    //        }
    //        else if (int.Parse(ArchorHP) >= int.Parse(Archor_level) * 100)
    //        {
    //            Debug.LogError("�Ѱ�ġ �Դϴ�");
    //        }
    //    }
    //    else if (int.Parse(Gold) < 200)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("���� �߻�");
    //    }
    //}

    ////����� ���� ��
    //public void MagicianEXPUp()
    //{
    //    if (int.Parse(MagePoints) > 0)
    //    {
    //        int UE = int.Parse(Magician_EXP) + 100; //����ġ   
    //        int MP = int.Parse(MagePoints) - 1;   //��Ŀ �迭 ����Ʈ - 1


    //        if (UE >= 500)       //EXP�� �� ä���� ������ ��Ʈ
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
    //        else if (UE < 500)      //EXP�� �� ���� ��Ʈ
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
    //        Debug.LogError("���� �߻�");
    //    }
    //}

    ////���� ���ݷ� ����
    //public void MagicianATKUp()
    //{
    //    if (int.Parse(Gold) >= 200)
    //    {
    //        if (int.Parse(MagicianATK) < int.Parse(Magician_level) * 10)        //���� �Ѱ迡 ����X & ���ݷ� ����
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
    //            tMagicianATK.text = "���ݷ� : " + MagicianATK;
    //        }
    //        else if (int.Parse(MagicianATK) >= int.Parse(Magician_level) * 10)
    //        {
    //            Debug.LogError("�Ѱ�ġ �Դϴ�");
    //        }
    //    }
    //    else if (int.Parse(Gold) < 200)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("���� �߻�");
    //    }
    //}

    ////���� HP ����
    //public void MagicianHPUp()
    //{
    //    if (int.Parse(Gold) >= 200)
    //    {
    //        if (int.Parse(MagicianHP) < int.Parse(Magician_level) * 100)        //���� �Ѱ迡 ����X & ���ݷ� ����
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
    //            tMagicianHP.text = "ü�� : " + MagicianHP;
    //        }
    //        else if (int.Parse(MagicianHP) >= int.Parse(Magician_level) * 100)
    //        {
    //            Debug.LogError("�Ѱ�ġ �Դϴ�");
    //        }
    //    }
    //    else if (int.Parse(Gold) < 200)
    //    {
    //        NotEnough.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogError("���� �߻�");
    //    }
    //}

}
