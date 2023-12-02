using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

using Data;

namespace StageSection
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] StagePanel stagepanel;
        [SerializeField] private GameObject stagePanel;

        [SerializeField] private GameObject tower;

        [SerializeField] private Text stageIntroduction;
        private string[] StageTopText = new string[4];

        [SerializeField] private GameObject towerUpBtn;
        [SerializeField] private GameObject towerDownBtn;
        [SerializeField] private DifficultyButton difficultyButton;

        [SerializeField] public bool MoveNow = false;
        [SerializeField] private int NowFloor = 1;

        private bool DifficultOnOff;
        private bool StagePanelOn;

        private int nowDifficult;
        private int nowStage;

        [SerializeField] public int NowStage { get => nowStage; }
        [SerializeField] public int NowDifficult { get => nowDifficult; }

        private void Awake()
        {
            StageTopText[0] = "�������� 1. ������ ���ڵ��� ��\n\n�ΰ��� ��ɿ� �� ���� ���� ���� ������ ��ȿ�Ѵ�.\n\n�׸��� �˸���.\n\n�ͼ��� ����, �̰��� �����߳���.";
            StageTopText[1] = "�������� 2. �������� ������ ���\n\n���������� �����Ǿ� ���븦 ǳ��� ��ü�� ��.\n\n�Ѹ���� ���� ����.\n\n��� ���� �׸� ���״�. ";
            StageTopText[2] = "�������� 3. �������� ������ ��\n\n���� ��.\n\n���� ��.\n\n������ ��ȭ.\n\n�� ������ �����ǰ� ���� �ʱ���.��Ÿ��� ���̷μ�.";
            StageTopText[3] = "�������� 4. ��� ������ ������\n\n������ ������ �ִٱ⿡ ã�ư���.\n������ �ݺ����� ������ �����꿡��\n���� ������ ����.\n\n���� ������� ���� �����ڿ���.\n\n- �ʴ� ����� �ڼ������� ���� - ";
            MoveNow = false;
            DifficultOnOff = false;

        }

        public IEnumerator Co_UpDownBtnManager(int k)
        {
            if (DifficultOnOff == false && k == 1 && MoveNow == false && StagePanelOn == false)
            {
                MoveNow = true;
                //up
                NowFloor++;
                if (NowFloor == 4)
                {
                    towerUpBtn.SetActive(false);
                }
                towerDownBtn.SetActive(true);
                TowerUpDown(k);
                yield return new WaitForSeconds(1f);

                MoveNow = false;
            }
            if (DifficultOnOff == false && k == -1 && MoveNow == false && StagePanelOn == false)
            {
                MoveNow = true;
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
                TowerUpDown(k);
                yield return new WaitForSeconds(1f);

                MoveNow = false;
            }
        }

        private void TowerUpDown(int k)
        {
            MoveNow = true;
            tower.transform.DOLocalMoveY(tower.transform.localPosition.y + 798 * -k, 1);
        }

        public IEnumerator Co_StageInfoClick(int k)
        {
            if (MoveNow == false)
            {
                StagePanelOn = false;
                MoveNow = true;

                tower.transform.DOLocalMoveX(-350, 1);

                nowStage = k;
                difficultyButton.UpdateDifficultyButton(k + 1);
                stageIntroduction.text = StageTopText[k];

                yield return new WaitForSeconds(1f);

                MoveNow = false;

                if (DifficultOnOff == false)
                {
                    DifficultOnOff = true;
                    difficultyButton.gameObject.SetActive(true);
                    stagePanel.SetActive(false);
                }
                else
                {
                    stagePanel.SetActive(false);
                    difficultyButton.gameObject.SetActive(true);
                }
            }
        }

        public IEnumerator Co_Difficultdelay()
        {
            if (DifficultOnOff == true && MoveNow == false)
            {
                MoveNow = true;
                difficultyButton.gameObject.SetActive(false);
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

        public void Co_Stagedifficult(int i)
        {
            nowDifficult = i;
            stagepanel.SetReward(nowStage, nowDifficult);

            difficultyButton.gameObject.SetActive(false);
            DifficultOnOff = false;

            GameManager.Instance.StageInfo.Difficulty = (StageInfo.eDifficulty)i;

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
            difficultyButton.gameObject.SetActive(true);

            yield return new WaitForSeconds(1f);

            MoveNow = false;
        }


        public IEnumerator Co_DifficultCloseClick()
        {
            difficultyButton.gameObject.SetActive(false);
            DifficultOnOff = false;
            tower.transform.DOLocalMoveX(0, 1);
            MoveNow = true;

            yield return new WaitForSeconds(1f);

            MoveNow = false;
        }
    }
}