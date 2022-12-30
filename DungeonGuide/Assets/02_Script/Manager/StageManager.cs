using System.Collections;
using UnityEngine;

using Data;

namespace StageSection
{
    public class StageManager : MonoBehaviour
    {
        /*private static StageManager _instance;
        public static StageManager Instance
        {
            get
            {
                _instance = FindObjectOfType(typeof(StageManager)) as StageManager;

                if(_instance == null)
                {
                    Debug.Log("no singleton obj");
                }
                return _instance;
            }
            
        }*/

        [SerializeField] private Tower t;

        void Awake()
        {
            /*if(_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);*/
        }
        private void Start()
        {
            SoundManager.Instance.SetBGM(2);
        }

        public void BtnEvt_Difficultdelay()
        {
            StartCoroutine(t.Co_Difficultdelay());
        }

        public void BtnEvt_StageInfoClick(int k)
        {
            StartCoroutine(t.Co_StageInfoClick(k));
        }

        public void BtnEvt_Stagedifficult(int k)
        {
            t.Co_Stagedifficult(k);
        }


        public void BtnEvt_StagePanelCloseClick()
        {
            StartCoroutine(t.Co_StagePanelCloseClick());
        }


        public void BtnEvt_DifficultCloseClick()
        {
            StartCoroutine(t.Co_DifficultCloseClick());
        }

        public void BtnEvt_UpDownBtnManager(int k)
        {
            StartCoroutine(t.Co_UpDownBtnManager(k));
        }

        //난이도 고르기
        public void BtnEvt_ChooseDifficulty(int index)
        {
            GameManager.Instance.StageInfo.Difficulty = (StageInfo.eDifficulty)index;
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

        //스타트 버튼
        public void GoBattleScene()
        {
            StartCoroutine(Co_GoBattleScene());
        }

        public void BtnEvt_LoadStage(int index)
        {
            GameManager.Instance.StageInfo.BossIndex = index;
            LoadingSceneController.LoadScene("Stage");
        }

        public IEnumerator Co_GoBattleScene()
        {
            yield return null;

            GameManager.Instance.StageInfo.BossIndex = t.NowStage;
            GameManager.Instance.StageInfo.Difficulty = (StageInfo.eDifficulty)t.NowDifficult;
            GameManager.Instance.LoadSceneThroughLoadingScene(1);
            SoundManager.Instance.SetSFX(0);
        }
    }
}
