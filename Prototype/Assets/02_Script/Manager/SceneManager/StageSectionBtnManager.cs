using UnityEngine;
using Data;

namespace StageSection
{
    public class StageSectionBtnManager : StageManager
    {
        //��׶��� ��ư
        public void BtnEvt_BackGround()
        {
            Difficultdelay();
        }

        //�������� ������ ��ư
        public void BtnEvt_StageInfo(int i)
        {
            int k = i;
            StageInfoClick(k);
        }

        //�������� ���̵� ���� ��ư
        public void BtnEvt_Stagedifficult(int i)
        {
            Stagedifficult(i);
        }

        //���̵� â �ݱ�
        public void BtnEvt_DifficultClose()
        {
            DifficultCloseClick();
        }

        //�������� X ��ư
        public void BtnEvt_StagePanelClose()
        {
            StagePanelCloseClick();
        }


        public void BtnEvt_LoadStage(int index)
        {
            GameManager.Instance.StageInfo.BossIndex = index;
            LoadingSceneController.LoadScene("Stage");
        }

        //���̵� ����
        public void BtnEvt_ChooseDifficulty(int index)
        {
            GameManager.Instance.StageInfo.Difficulty = (StageInfo.eDifficulty)index;
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
            UpDownBtnManager(k);
        }

        //��ŸƮ ��ư
        public void GoBattleScene()
        {
            StartCoroutine(Co_GoBattleScene());
        }
    }
}
