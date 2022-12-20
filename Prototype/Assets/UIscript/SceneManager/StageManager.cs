using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using UnityEngine.SceneManagement;
using System.IO;
using System.Threading.Tasks;

using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;

public class StageManager : MonoBehaviour
{
    [SerializeField] private string Userid;

    private bool panelonoff;

    //�������� �г� ����
    [SerializeField] private GameObject StagePanel;
    [SerializeField] private string[] StageTopText = new string[6];

    private bool[] panelsonoff = new bool[6];

    //Ÿ�� ������Ʈ
    [SerializeField] private GameObject Tower;

    //���������� ����� ����
    [SerializeField] private string[] UnitSetting = new string[4];

    //���������� ����� ��ų
    [SerializeField] private GameObject[] SkillSettingIcon = new GameObject[4];
    [SerializeField] private string[] SkillSetting = new string[4];

    //����, ��ų ������
    [SerializeField] private Sprite TankerIcon;
    [SerializeField] private Sprite MeleeIcon;
    [SerializeField] private Sprite ADIcon;
    [SerializeField] private Sprite MageIcon;

    private string Gold = string.Empty;
    [SerializeField] private TextMeshProUGUI tGold;

    //�迭 ����Ʈ
    private string TankerPoints = string.Empty;
    [SerializeField] private TextMeshProUGUI tTankerPoints;
    private string WarriorPoints = string.Empty;
    [SerializeField] private TextMeshProUGUI tWarriorPoints;
    private string ADPoints = string.Empty;
    [SerializeField] private TextMeshProUGUI tADPoints;
    private string MagePoints = string.Empty;
    [SerializeField] private TextMeshProUGUI tMagePoints;

    [SerializeField] private Sprite[] SkillIcon = new Sprite[4];

    //�� �����, ���� ��ü ����
    [SerializeField] private Text StageIntroduction;
    [SerializeField] private Text StageCompensation;

    //���� ��ų ����
    [SerializeField] private GameObject SkillSelectPanel;
    [SerializeField] private GameObject UnitSelectPanel;

    private int nowsetting;
    private int nowStage;

    [SerializeField] private GameObject[] Skilllist = new GameObject[7];
    [SerializeField] private string[] tSkilllist = new string[7];
    public GameObject[] BossSection = new GameObject[4];

    [SerializeField] private bool[] StageClear = new bool[12];
    [SerializeField] private string[] StageClearstring = new string[12];

    [SerializeField] private GameObject Difficult;
    private bool diffonoff;
    private int nowdif;
   [SerializeField] private bool Movenow;
    [SerializeField] private int nowfloor;
    [SerializeField] private GameObject TowerUpBtn;
    [SerializeField] private GameObject TowerDownBtn;
    private bool StagePanelon;

    [SerializeField] private DifficultyButton difficultyButton;

    [SerializeField] private GameObject SettingPanel;

    [SerializeField] private GameObject Background;

    public int NowStage { get => nowStage; }
    public int Nowdif { get => nowdif; }

    [SerializeField] private GameObject[] UpDownBtn = new GameObject[2];
    [SerializeField] private int BtnCount = 1;

    void Awake()
    {

        panelsonoff[0] = false;
        panelsonoff[1] = false;
        panelsonoff[2] = false;
        panelsonoff[3] = false;
        panelsonoff[4] = false;
        panelsonoff[5] = false;

        tSkilllist[0] = "Skill1";
        tSkilllist[1] = "Skill2";
        tSkilllist[2] = "Skill3";
        tSkilllist[3] = "Skill4";
        tSkilllist[4] = "Skill5";
        tSkilllist[5] = "Skill6";
        tSkilllist[6] = "Skill7";


        StageTopText[0] = "������ ���ڵ��� ��\n �ΰ��� ��ɿ� �� ���� ���� ���� ������ ��ȿ�Ѵ�.\n�׸��� �˸���.\n �ͼ��� ����, �̰��� �����߳���. ";
        StageTopText[1] = "�������� ������ ���\n ���������� �����Ǿ� ���븦 ǳ��� ��ü�� ��. \n \n �Ѹ���� ���� ����.\n��� ���� �׸� ���״�. ";
        StageTopText[2] = "�������� ������ ��\n���� ��.\n���� ��.\n������ ��ȭ.\n�� ������ �����ǰ� ���� �ʱ���.��Ÿ��� ���̷μ�.";
        StageTopText[3] = "��� ����� ������\n������ ������ �ִٱ⿡ ã�ư���.\n������ �ݺ����� ������ �����꿡�� ���� ������ ����.\n��� ������� ���� �����ڿ���.\n-�ʴ� ����� �ڼ������� ���� - ";
        StageTopText[4] = "5�� ��¼�� ����\n (���丮 ����)";
        StageTopText[5] = "6�� ��¼�� ����\n (���丮 ����)";

        panelonoff = false;
        diffonoff = false;
        Movenow = false;
        nowfloor = 1;
        StagePanelon = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    //��׶��� ��ư
    public void BackGroundBtn()
    {
        //���̵� ����������
        if (diffonoff == true && Movenow == false)
        {
            Movenow = true;
            Difficult.SetActive(false);
            Tower.transform.DOLocalMoveX(0, 1);
            Invoke("Difficultdelay", 1f);
        }
    }

    //���̵� ���� �� 1�� �Ŀ� ����Ǵ� �Լ�
    public void Difficultdelay()
    {
        Movenow = false;
        if (diffonoff == false)
        {
            diffonoff = true;
            StagePanel.SetActive(false);
        }
        else
        {
            StagePanel.SetActive(false);
        }
    }

    //�������� ������ ��ư
    public void StageInfo(int i)
    {
        if(Movenow == false)
        {
           
            StagePanelon = false;
            Movenow = true;

            Tower.transform.DOLocalMoveX(-350, 1);

            Invoke("StageInfodelay", 1f);

            StageIntroduction.text = StageTopText[i];

            nowStage = i;
            difficultyButton.UpdateDifficultyButton(i + 1);
        }
    }

    //�������� ���� �� 1�� �Ŀ� ����Ǵ� �Լ�
    public void StageInfodelay()
    {
        Movenow = false;
        if (diffonoff == false)
        {
            diffonoff = true;
            Difficult.SetActive(true);
            StagePanel.SetActive(false);
        }
        else
        {
            StagePanel.SetActive(false);
            Difficult.SetActive(true);
        }
    }

    //�������� ���� ��ư
    public void Stagedifficult(int i)
    {
        Difficult.SetActive(false);
        diffonoff = false;

        nowdif = i;
        BossManager.difficulty = (BossManager.Difficulty)i;

        StagePanelon = true;
        StagePanel.SetActive(true);
        StageIntroduction.text = StageTopText[nowStage];
    }

    public void BtnEvt_LoadStage(int index)
    {
        BossManager.bossIndex = index;
        //LoadingSceneController.LoadScene("Stage");
    }

    public void BtnEvt_ChooseDifficulty(int index)
    {
        BossManager.difficulty = (BossManager.Difficulty)index;
    }

    //���� ��ư
    public void SettingPanelOpen()
    {
        SettingPanel.SetActive(true);
    }
    public void SettingPanelClose()
    {
        SettingPanel.SetActive(false);
    }

    //�������� X ��ư
    public void StagePanelClose()
    {
        StagePanelon = false;
        Movenow = true;
        diffonoff = true;
        StagePanel.SetActive(false);
        Difficult.SetActive(true);
        Invoke("StagePanelOff", 0.1f);
    }


    public void DifficultClose()
    {
        Difficult.SetActive(false);
        diffonoff = false;
        Tower.transform.DOLocalMoveX(0, 1);
        Movenow = true;
        Invoke("StagePanelOff", 1f);
    }

    public void TowerUpDown(int i)
    {
        int k = i;
        StartCoroutine(UpDownBtnManager(k));
    }

    IEnumerator UpDownBtnManager(int k)
    {
        if (diffonoff == false && k == 0 && Movenow == false && StagePanelon == false)
        {
            //up
            nowfloor++;
            if (nowfloor == 4)
            {
                TowerUpBtn.SetActive(false);
                TowerDownBtn.SetActive(true);
            }
            else
            {
                TowerDownBtn.SetActive(true);
            }
            Movenow = true;
            Tower.transform.DOLocalMoveY(Tower.transform.localPosition.y - 720, 1);
            yield return new WaitForSeconds(1f);

            Movenow = false;
        }
        if (diffonoff == false && k == 1 && Movenow == false && StagePanelon == false)
        {
            //down
            nowfloor--;
            if (nowfloor == 1)
            {
                TowerDownBtn.SetActive(false);
                TowerUpBtn.SetActive(true);
            }
            else
            {
                TowerUpBtn.SetActive(true);
            }
            Movenow = true;
            Tower.transform.DOLocalMoveY(Tower.transform.localPosition.y + 720, 1);
            yield return new WaitForSeconds(1f);

            Movenow = false;
        }
    }

    public void SkillSelect(int i)
    {
        SkillSelectPanel.SetActive(true);
        nowsetting = i;
        //SkillSettingIcon[i];
    }

    //SkillSelect�гο� �ִ� ���
    public void SkillChange(int i)
    {
        for (int p = 0; p < SkillSetting.Length; p++)
        {
            if(SkillSetting[p] == tSkilllist[i])
            {
                SkillSetting[p] = SkillSetting[nowsetting];
                //SkillSetting[]
                SkillSettingIcon[p].GetComponent<Image>().sprite = SkillSettingIcon[nowsetting].GetComponent<Image>().sprite;
            }
        }
        SkillSetting[nowsetting] = tSkilllist[i];
        SkillSettingIcon[nowsetting].GetComponent<Image>().sprite = SkillIcon[i];

        SkillSelectPanel.SetActive(false);
    }
    public void BtnEvt_LoadMainScene()
    {
        LoadingSceneController.LoadScene("Main");
        SoundManager.Instance.SetBGM(1);
    }
    public void GoBattleScene()
    {
        BossManager.bossIndex = nowStage;
        BossManager.difficulty = (BossManager.Difficulty)nowdif;
        LoadingSceneController.LoadScene("Stage");
        SoundManager.Instance.SetBGM(nowStage + 3);
    }

    public void Return()
    {
        LoadingSceneController.LoadScene("StartScene");
    }

    public void Mode(int index)
    {

    }
}
