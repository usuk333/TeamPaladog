using UnityEngine;

namespace StageSection
{
    public class StageSectionBtnManager : MonoBehaviour
    {
        StageSection.StageManager stageManager;

        private void Awake()
        {
            stageManager = GetComponent<StageSection.StageManager>();
        }

        //��׶��� ��ư
        public void BtnEvt_BackGround()
        {
            StartCoroutine(stageManager.Co_Difficultdelay());
        }

        //�������� ������ ��ư
        public void BtnEvt_StageInfo(int i)
        {
            int k = i;
            StartCoroutine(stageManager.Co_StageInfoClick(k));
        }

        //�������� ���̵� ���� ��ư
        public void BtnEvt_Stagedifficult(int i)
        {
            StartCoroutine(stageManager.Co_Stagedifficult(i));
        }

        //���̵� â �ݱ�
        public void BtnEvt_DifficultClose()
        {
            StartCoroutine(stageManager.Co_DifficultCloseClick());
        }

        //�������� X ��ư
        public void BtnEvt_StagePanelClose()
        {
            StartCoroutine(stageManager.Co_StagePanelCloseClick());
        }


        public void BtnEvt_LoadStage(int index)
        {
            global::StageInfo.bossIndex = index;
            //LoadingSceneController.LoadScene("Stage");
        }

        //���̵� ����
        public void BtnEvt_ChooseDifficulty(int index)
        {
            global::StageInfo.difficulty = (StageInfo.Difficulty)index;
        }

        //���� ��ư
        public void BtnEvt_SettingPanelOpen(GameObject obj)
        {
            obj.SetActive(true);
        }
        public void BtnEvt_SettingPanelClose(GameObject obj)
        {
            obj.SetActive(false);
        }

        //Ÿ�� ȭ��ǥ ��ư
        public void BtnEvt_TowerUpDown(int i)
        {
            int k = i;
            StartCoroutine(stageManager.Co_UpDownBtnManager(k));
        }

        //�������� ��ư
        public void BtnEvt_LoadMainScene()
        {
            LoadingSceneController.LoadScene("Main");
            SoundManager.Instance.SetBGM(1);
        }

        //��ŸƮ ��ư
        public void GoBattleScene()
        {
            StartCoroutine(stageManager.Co_GoBattleScene());
        }
    }
}
