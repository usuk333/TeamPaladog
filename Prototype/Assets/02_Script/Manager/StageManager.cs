using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using DG.Tweening;
using TMPro;

public class StageManager : MonoBehaviour
{
    //스테이지 패널 관련
    [SerializeField] private GameObject StagePanel;
    [SerializeField] private string[] StageTopText = new string[4];

    //타워 오브젝트
    [SerializeField] private GameObject Tower;

    //층 설명과, 보상 개체 선언
    [SerializeField] private Text StageIntroduction;
    [SerializeField] private Text StageCompensation;

    private int nowStage;

    public GameObject[] BossSection = new GameObject[4];

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

    void Awake()
    {
        StageTopText[0] = "잊혀진 사자들의 왕\n 인간의 욕심에 긴 잠을 방해 받은 짐승이 포효한다.\n그리고 알린다.\n 맹수의 왕이, 이곳에 군림했노라고. ";
        StageTopText[1] = "끈적끈적 보랏빛 재앙\n 보랏빛으로 변색되어 악취를 풍기는 시체의 산. \n \n 겉모습에 속지 마라.\n당신 또한 그리 될테니. ";
        StageTopText[2] = "깨달음을 저버린 자\n선과 악.\n음과 양.\n균형과 조화.\n그 무엇도 유지되고 있지 않구나.안타까운 일이로세.";
        StageTopText[3] = "깨어난 고대의 감시자\n진귀한 보옥이 있다기에 찾아갔다.\n하지만 금빛으로 빛나는 바위산에서 내가 마주한 것은.\n깊게 가라앉은 용의 눈동자였다.\n-초대 용사의 자서전에서 발췌 - ";

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
            StartCoroutine(Difficultdelay());
        }
    }

    IEnumerator Difficultdelay()
    {
        Movenow = true;
        Difficult.SetActive(false);
        Tower.transform.DOLocalMoveX(0, 1);

        yield return new WaitForSeconds(1f);

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
        int k = i;
        StartCoroutine(StageInfoClick(k));
    }

    IEnumerator StageInfoClick(int k)
    {
        if (Movenow == false)
        {
            StagePanelon = false;
            Movenow = true;

            Tower.transform.DOLocalMoveX(-350, 1);

            yield return new WaitForSeconds(1f);

            StageIntroduction.text = StageTopText[k];

            nowStage = k;
            difficultyButton.UpdateDifficultyButton(k + 1);

            yield return new WaitForSeconds(1f);

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
    }

    //스테이지 선택 버튼
    public void Stagedifficult(int i)
    {
        Difficult.SetActive(false);
        diffonoff = false;

        nowdif = i;
        global::StageInfo.difficulty = (StageInfo.Difficulty)i;

        StagePanelon = true;
        StagePanel.SetActive(true);
        StageIntroduction.text = StageTopText[nowStage];
    }

    public void BtnEvt_LoadStage(int index)
    {
        global::StageInfo.bossIndex = index;
        //LoadingSceneController.LoadScene("Stage");
    }

    public void BtnEvt_ChooseDifficulty(int index)
    {
        global::StageInfo.difficulty = (StageInfo.Difficulty)index;
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
        StartCoroutine(StagePanelCloseClick());
    }

    IEnumerator StagePanelCloseClick()
    {
        StagePanelon = false;
        Movenow = true;
        diffonoff = true;
        StagePanel.SetActive(false);
        Difficult.SetActive(true);

        yield return new WaitForSeconds(1f);

        Movenow = false;
    }

    public void DifficultClose()
    {
        StartCoroutine(DifficultCloseClick());
    }

    IEnumerator DifficultCloseClick()
    {
        Difficult.SetActive(false);
        diffonoff = false;
        Tower.transform.DOLocalMoveX(0, 1);
        Movenow = true;

        yield return new WaitForSeconds(1f);

        Movenow = false;
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
            Tower.transform.DOLocalMoveY(Tower.transform.localPosition.y - 798, 1);
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
            Tower.transform.DOLocalMoveY(Tower.transform.localPosition.y + 798, 1);
            yield return new WaitForSeconds(1f);

            Movenow = false;
        }
    }

    public void BtnEvt_LoadMainScene()
    {
        LoadingSceneController.LoadScene("Main");
        SoundManager.Instance.SetBGM(1);
    }
    public void GoBattleScene()
    {
        global::StageInfo.bossIndex = nowStage;
        global::StageInfo.difficulty = (StageInfo.Difficulty)nowdif;
        LoadingSceneController.LoadScene("Stage");
        SoundManager.Instance.SetBGM(nowStage + 3);
    }
}
