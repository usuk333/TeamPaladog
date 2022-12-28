using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

namespace StartScene
{
    public class LoginObject : MonoBehaviour
    {
        private Text loginText;
        private StartManager startManager;

        private void Awake()
        {
            loginText = GetComponentInChildren<Text>();
            startManager = FindObjectOfType<StartManager>();
        }
        public void ActiveLoginObj()
        {
            gameObject.SetActive(true);
            StartCoroutine(Co_LoginAnim());
        }
        private IEnumerator Co_LoginAnim()
        {
            while (true)
            {
                loginText.DOFade(1, 1f);
                yield return new WaitForSeconds(1.2f);
                loginText.DOFade(0, 1f);
                yield return new WaitForSeconds(1.2f);
            }
        }
        public void BtnEvt_Login()
        {
            startManager.SetPlayerData();
        }
    }
}
