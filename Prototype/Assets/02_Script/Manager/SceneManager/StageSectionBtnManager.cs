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

        //백그라운드 버튼
        public void BtnEvt_BackGround()
        {
            StartCoroutine(stageManager.Co_Difficultdelay());
        }

        //스테이지 누르는 버튼
        public void BtnEvt_StageInfo(int i)
        {
            int k = i;
            StartCoroutine(stageManager.Co_StageInfoClick(k));
        }

        //스테이지 난이도 선택 버튼
        public void BtnEvt_Stagedifficult(int i)
        {
            StartCoroutine(stageManager.Co_Stagedifficult(i));
        }

        //난이도 창 닫기
        public void BtnEvt_DifficultClose()
        {
            StartCoroutine(stageManager.Co_DifficultCloseClick());
        }

        //스테이지 X 버튼
        public void BtnEvt_StagePanelClose()
        {
            StartCoroutine(stageManager.Co_StagePanelCloseClick());
        }


        public void BtnEvt_LoadStage(int index)
        {
            global::StageInfo.bossIndex = index;
            //LoadingSceneController.LoadScene("Stage");
        }

        //난이도 고르기
        public void BtnEvt_ChooseDifficulty(int index)
        {
            global::StageInfo.difficulty = (StageInfo.Difficulty)index;
        }

        //세팅 버튼
        public void BtnEvt_SettingPanelOpen(GameObject obj)
        {
            obj.SetActive(true);
        }
        public void BtnEvt_SettingPanelClose(GameObject obj)
        {
            obj.SetActive(false);
        }

        //타워 화살표 버튼
        public void BtnEvt_TowerUpDown(int i)
        {
            int k = i;
            StartCoroutine(stageManager.Co_UpDownBtnManager(k));
        }

        //메인으로 버튼
        public void BtnEvt_LoadMainScene()
        {
            LoadingSceneController.LoadScene("Main");
            SoundManager.Instance.SetBGM(1);
        }

        //스타트 버튼
        public void GoBattleScene()
        {
            StartCoroutine(stageManager.Co_GoBattleScene());
        }
    }
}
