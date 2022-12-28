using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

namespace StageSection
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private GameObject stagePanel;

        [SerializeField] private GameObject tower;

        [SerializeField] private Text stageIntroduction;
        private string[] StageTopText = new string[4];

        [SerializeField] private GameObject towerUpBtn;
        [SerializeField] private GameObject towerDownBtn;
        [SerializeField] private GameObject Difficult;
        [SerializeField] private DifficultyButton difficultyButton;

        [SerializeField] private bool MoveNow;
        [SerializeField] private int NowFloor = 1;

        private bool DifficultOnOff;
        private bool StagePanelOn;

        private int nowDifficult;
        private int nowStage;

        public int NowStage { get => nowStage; }
        public int NowDifficult { get => nowDifficult; }

        private void Awake()
        {
            StageTopText[0] = "스테이지 1. 잊혀진 사자들의 왕\n\n인간의 욕심에 긴 잠을 방해 받은 짐승이 포효한다.\n\n그리고 알린다.\n\n맹수의 왕이, 이곳에 군림했노라고.";
            StageTopText[1] = "스테이지 2. 끈적끈적 보랏빛 재앙\n\n보랏빛으로 변색되어 악취를 풍기는 시체의 산.\n\n겉모습에 속지 마라.\n\n당신 또한 그리 될테니. ";
            StageTopText[2] = "스테이지 3. 깨달음을 저버린 자\n\n선과 악.\n\n음과 양.\n\n균형과 조화.\n\n그 무엇도 유지되고 있지 않구나.안타까운 일이로세.";
            StageTopText[3] = "스테이지 4. 깨어난 고대의 감시자\n\n진귀한 보옥이 있다기에 찾아갔다.\n하지만 금빛으로 빛나는 바위산에서\n내가 마주한 것은.\n\n깊게 가라앉은 용의 눈동자였다.\n\n- 초대 용사의 자서전에서 발췌 - ";
        }

        public IEnumerator Co_UpDownBtnManager(int k)
        {
            if (DifficultOnOff == false && k == 1 && MoveNow == false && StagePanelOn == false)
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
                TowerUpDown(k);
                yield return new WaitForSeconds(1f);

                MoveNow = false;
            }
            if (DifficultOnOff == false && k == -1 && MoveNow == false && StagePanelOn == false)
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
                TowerUpDown(k);
                yield return new WaitForSeconds(1f);

                MoveNow = false;
            }
        }

        private void TowerUpDown(int k)
        {
            MoveNow = true;
            tower.transform.DOLocalMoveY(tower.transform.localPosition.y + 798 * k, 1);
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

        public IEnumerator Co_Stagedifficult(int i)
        {
            yield return null;

            Difficult.SetActive(false);
            DifficultOnOff = false;

            nowDifficult = i;
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
    }
}
