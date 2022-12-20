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

    //스테이지 패널 관련
    [SerializeField] private GameObject StagePanel;
    [SerializeField] private string[] StageTopText = new string[6];

    private bool[] panelsonoff = new bool[6];

    //타워 오브젝트
    [SerializeField] private GameObject Tower;

    //마지막으로 사용한 유닛
    [SerializeField] private string[] UnitSetting = new string[4];

    //마지막으로 사용한 스킬
    [SerializeField] private GameObject[] SkillSettingIcon = new GameObject[4];
    [SerializeField] private string[] SkillSetting = new string[4];

    //유닛, 스킬 아이콘
    [SerializeField] private Sprite TankerIcon;
    [SerializeField] private Sprite MeleeIcon;
    [SerializeField] private Sprite ADIcon;
    [SerializeField] private Sprite MageIcon;

    private string Gold = string.Empty;
    [SerializeField] private TextMeshProUGUI tGold;

    //계열 포인트
    private string TankerPoints = string.Empty;
    [SerializeField] private TextMeshProUGUI tTankerPoints;
    private string WarriorPoints = string.Empty;
    [SerializeField] private TextMeshProUGUI tWarriorPoints;
    private string ADPoints = string.Empty;
    [SerializeField] private TextMeshProUGUI tADPoints;
    private string MagePoints = string.Empty;
    [SerializeField] private TextMeshProUGUI tMagePoints;

    [SerializeField] private Sprite[] SkillIcon = new Sprite[4];

    //층 설명과, 보상 개체 선언
    [SerializeField] private Text StageIntroduction;
    [SerializeField] private Text StageCompensation;

    //유닛 스킬 선택
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


        StageTopText[0] = "잊혀진 사자들의 왕\n 인간의 욕심에 긴 잠을 방해 받은 짐승이 포효한다.\n그리고 알린다.\n 맹수의 왕이, 이곳에 군림했노라고. ";
        StageTopText[1] = "끈적끈적 보랏빛 재앙\n 보랏빛으로 변색되어 악취를 풍기는 시체의 산. \n \n 겉모습에 속지 마라.\n당신 또한 그리 될테니. ";
        StageTopText[2] = "깨달음을 저버린 자\n선과 악.\n음과 양.\n균형과 조화.\n그 무엇도 유지되고 있지 않구나.안타까운 일이로세.";
        StageTopText[3] = "깨어난 고대의 감시자\n진귀한 보옥이 있다기에 찾아갔다.\n하지만 금빛으로 빛나는 바위산에서 내가 마주한 것은.\n깊게 가라앉은 용의 눈동자였다.\n-초대 용사의 자서전에서 발췌 - ";
        StageTopText[4] = "5층 어쩌고 보스\n (스토리 설명)";
        StageTopText[5] = "6층 어쩌고 보스\n (스토리 설명)";

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

    //백그라운드 버튼
    public void BackGroundBtn()
    {
        //난이도 켜져있을때
        if (diffonoff == true && Movenow == false)
        {
            Movenow = true;
            Difficult.SetActive(false);
            Tower.transform.DOLocalMoveX(0, 1);
            Invoke("Difficultdelay", 1f);
        }
    }

    //난이도 선택 후 1초 후에 실행되는 함수
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

    //스테이지 누르는 버튼
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

    //스테이지 선택 후 1초 후에 실행되는 함수
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

    //스테이지 선택 버튼
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

    //세팅 버튼
    public void SettingPanelOpen()
    {
        SettingPanel.SetActive(true);
    }
    public void SettingPanelClose()
    {
        SettingPanel.SetActive(false);
    }

    //스테이지 X 버튼
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

    //SkillSelect패널에 있는 놈들
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
