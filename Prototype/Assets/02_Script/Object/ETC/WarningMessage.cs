using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ETC
{
    public class WarningMessage : MonoBehaviour
    {
        [SerializeField] private Text warningText;
        public void SetWarningText(string text)
        {
            warningText.text = text;
            gameObject.SetActive(true);
            StartCoroutine(Co_WarningMessage());
        }
        private IEnumerator Co_WarningMessage()
        {
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
    }
}
