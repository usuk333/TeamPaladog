using System.Collections;
using UnityEngine;

using UnityEngine.UI;

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

        protected void Difficultdelay()
        {
            StartCoroutine(t.Co_Difficultdelay());
        }

        protected void StageInfoClick(int k)
        {
            StartCoroutine(t.Co_StageInfoClick(k));
        }

        protected void Stagedifficult(int k)
        {
            StartCoroutine(t.Co_Stagedifficult(k));
        }


        protected void StagePanelCloseClick()
        {
            StartCoroutine(t.Co_StagePanelCloseClick());
        }


        protected void DifficultCloseClick()
        {
            StartCoroutine(t.Co_DifficultCloseClick());
        }

        protected void UpDownBtnManager(int k)
        {
            StartCoroutine(t.Co_UpDownBtnManager(k));
        }


        public IEnumerator Co_GoBattleScene()
        {
            yield return null;

            global::StageInfo.bossIndex = t.NowStage;
            global::StageInfo.difficulty = (StageInfo.Difficulty)t.NowDifficult;
            LoadingSceneController.LoadScene("Stage");
            SoundManager.Instance.SetBGM(t.NowStage + 3);
        }
    }
}
