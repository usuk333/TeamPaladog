using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using DG.Tweening;
using TMPro;

public class StageManager : MonoBehaviour
{
    //�������� �г� ����
    [SerializeField] private GameObject StagePanel;
    [SerializeField] private string[] StageTopText = new string[4];

    //Ÿ�� ������Ʈ
    [SerializeField] private GameObject Tower;

    //�� �����, ���� ��ü ����
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
        StageTopText[0] = "������ ���ڵ��� ��\n �ΰ��� ��ɿ� �� ���� ���� ���� ������ ��ȿ�Ѵ�.\n�׸��� �˸���.\n �ͼ��� ����, �̰��� �����߳���. ";
        StageTopText[1] = "�������� ������ ���\n ���������� �����Ǿ� ���븦 ǳ��� ��ü�� ��. \n \n �Ѹ���� ���� ����.\n��� ���� �׸� ���״�. ";
        StageTopText[2] = "�������� ������ ��\n���� ��.\n���� ��.\n������ ��ȭ.\n�� ������ �����ǰ� ���� �ʱ���.��Ÿ��� ���̷μ�.";
        StageTopText[3] = "��� ����� ������\n������ ������ �ִٱ⿡ ã�ư���.\n������ �ݺ����� ������ �����꿡�� ���� ������ ����.\n��� ������� ���� �����ڿ���.\n-�ʴ� ����� �ڼ������� ���� - ";

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

    //�������� ������ ��ư
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

    //�������� ���� ��ư
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
