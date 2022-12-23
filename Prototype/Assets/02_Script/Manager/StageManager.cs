using System.Collections;
using UnityEngine;

using UnityEngine.UI;

using DG.Tweening;

namespace StageSection
{
    public class StageManager : MonoBehaviour
    {
        //�������� �г� ����
        [SerializeField] private GameObject stagePanel;
        [SerializeField] private string[] StageTopText = new string[4];

        //Ÿ�� ������Ʈ
        [SerializeField] private GameObject tower;

        //�� �����, ���� ��ü ����
        [SerializeField] private Text stageIntroduction;

        private int nowStage;

        public GameObject[] bossSection = new GameObject[4];

        [SerializeField] private GameObject Difficult;
        private bool DifficultOnOff;
        private int NowDifficult;
        [SerializeField] private bool MoveNow;
        [SerializeField] private int NowFloor;
        [SerializeField] private GameObject towerUpBtn;
        [SerializeField] private GameObject towerDownBtn;
        private bool StagePanelOn;

        [SerializeField] private DifficultyButton difficultyButton;


        public int NowStage { get => nowStage; }
        public int Nowdif { get => NowDifficult; }

        void Awake()
        {
            StageTopText[0] = "�������� 1. ������ ���ڵ��� ��\n\n�ΰ��� ��ɿ� �� ���� ���� ���� ������ ��ȿ�Ѵ�.\n\n�׸��� �˸���.\n\n�ͼ��� ����, �̰��� �����߳���.";
            StageTopText[1] = "�������� 2. �������� ������ ���\n\n���������� �����Ǿ� ���븦 ǳ��� ��ü�� ��.\n\n�Ѹ���� ���� ����.\n\n��� ���� �׸� ���״�. ";
            StageTopText[2] = "�������� 3. �������� ������ ��\n\n���� ��.\n\n���� ��.\n\n������ ��ȭ.\n\n�� ������ �����ǰ� ���� �ʱ���.��Ÿ��� ���̷μ�.";
            StageTopText[3] = "�������� 4. ��� ����� ������\n\n������ ������ �ִٱ⿡ ã�ư���.\n������ �ݺ����� ������ �����꿡��\n���� ������ ����.\n\n��� ������� ���� �����ڿ���.\n\n- �ʴ� ����� �ڼ������� ���� - ";

            NowFloor = 1;
        }

        public IEnumerator Co_Difficultdelay()
        {
            if (DifficultOnOff == true && MoveNow == false)
            {
                MoveNow = true;
                Difficult.SetActive(false);
                tower.transform.DOLocalMoveX(0, 1);

                yield return new WaitForSeconds(1f);

                MoveNow = false;
                if (DifficultOnOff == false)
                {
                    DifficultOnOff = true;
                    stagePanel.SetActive(false);
                }
                else
                {
                    stagePanel.SetActive(false);
                }
            }
        }

        public IEnumerator Co_StageInfoClick(int k)
        {
            if (MoveNow == false)
            {
                StagePanelOn = false;
                MoveNow = true;

                tower.transform.DOLocalMoveX(-350, 1);

                yield return new WaitForSeconds(1f);

                stageIntroduction.text = StageTopText[k];

                nowStage = k;
                difficultyButton.UpdateDifficultyButton(k + 1);

                yield return new WaitForSeconds(1f);

                MoveNow = false;

                if (DifficultOnOff == false)
                {
                    DifficultOnOff = true;
                    Difficult.SetActive(true);
                    stagePanel.SetActive(false);
                }
                else
                {
                    stagePanel.SetActive(false);
                    Difficult.SetActive(true);
                }
            }
        }

        public IEnumerator Co_Stagedifficult(int i)
        {
            yield return null;

            Difficult.SetActive(false);
            DifficultOnOff = false;

            NowDifficult = i;
            global::StageInfo.difficulty = (StageInfo.Difficulty)i;

            StagePanelOn = true;
            stagePanel.SetActive(true);
            stageIntroduction.text = StageTopText[nowStage];
        }


        public IEnumerator Co_StagePanelCloseClick()
        {
            StagePanelOn = false;
            MoveNow = true;
            DifficultOnOff = true;
            stagePanel.SetActive(false);
            Difficult.SetActive(true);

            yield return new WaitForSeconds(1f);

            MoveNow = false;
        }


        public IEnumerator Co_DifficultCloseClick()
        {
            Difficult.SetActive(false);
            DifficultOnOff = false;
            tower.transform.DOLocalMoveX(0, 1);
            MoveNow = true;

            yield return new WaitForSeconds(1f);

            MoveNow = false;
        }


        public IEnumerator Co_UpDownBtnManager(int k)
        {
            if (DifficultOnOff == false && k == 0 && MoveNow == false && StagePanelOn == false)
            {
                //up
                NowFloor++;
                if (NowFloor == 4)
                {
                    towerUpBtn.SetActive(false);
                    towerDownBtn.SetActive(true);
                }
                else
                {
                    towerDownBtn.SetActive(true);
                }
                MoveNow = true;
                tower.transform.DOLocalMoveY(tower.transform.localPosition.y - 798, 1);
                yield return new WaitForSeconds(1f);

                MoveNow = false;
            }
            if (DifficultOnOff == false && k == 1 && MoveNow == false && StagePanelOn == false)
            {
                //down
                NowFloor--;
                if (NowFloor == 1)
                {
                    towerDownBtn.SetActive(false);
                    towerUpBtn.SetActive(true);
                }
                else
                {
                    towerUpBtn.SetActive(true);
                }
                MoveNow = true;
                tower.transform.DOLocalMoveY(tower.transform.localPosition.y + 798, 1);
                yield return new WaitForSeconds(1f);

                MoveNow = false;
            }
        }

        public IEnumerator Co_GoBattleScene()
        {
            yield return null;

            global::StageInfo.bossIndex = nowStage;
            global::StageInfo.difficulty = (StageInfo.Difficulty)NowDifficult;
            LoadingSceneController.LoadScene("Stage");
            SoundManager.Instance.SetBGM(nowStage + 3);
        }
    }
}
