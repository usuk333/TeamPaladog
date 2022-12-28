using UnityEngine;
using UnityEngine.UI;

using System.Text.RegularExpressions;

namespace StartScene
{
    public class CreateAccountPopup : MonoBehaviour
    {
        private InputField inputField;
        private StartManager startManager;
        [SerializeField] private WarningMessage warningMessage;
        //´Ð³×ÀÓ ÇÊÅÍ¸µ
        private char[] notFullKoreanArray = { '¤¡', '¤¤', '¤§', '¤©', '¤±', '¤²', '¤µ', '¤·', '¤¸', '¤º', '¤»', '¤¼', '¤½', '¤¾' };
        private string[] swearWordArray = { "°£³ª", "°¥º¸", "³â", "³ð", "´À°³ºñ", "´À±Ý¸¶", "´Ï¹Ì","´Ì¹Ì","´Ì±â¹Ì", "´Ï±â¹Ì", "´ÚÃÄ","´ßÃÄ", "µî½Å", "¶Ç¶óÀÌ", "¶ÊÃß", "¹ÌÄ£", "Ã¢³à","Ã¬³à","Ã¬³ð", "Ã¢³ð",
        "º´½Å","ºé½Å", "º¸Áö", "ºÒ¾Ë", "ºÎ¶ö", "»¡Åë", "»õ³¢", "½Ã¹ß","¾¾¹ß", "¾Ã", "»õ³¢", "¿¥Ã¢", "À°º¯±â", "ÀÚÁö", "Á¨Àå", "Á¿", "Áö¶ö", "Áã¶ö", "£p¶ö" };
        private void Awake()
        {
            inputField = GetComponentInChildren<InputField>();
            startManager = FindObjectOfType<StartManager>();
        }
        public void SetCreateAccountPopup()
        {
            gameObject.SetActive(true);
            SetInputField();
        }
        private void SetInputField()
        {
            inputField.onValueChanged.AddListener((word) => inputField.text = Regex.Replace(word, @"[^¤¡-¤¾°¡-ÆR]", ""));
        }
        public void BtnEvt_CreateNewAccount()
        {
            CreateNewAccount();
        }
        private void CreateNewAccount()
        {
            if (inputField.text.Length < 2)
            {
                warningMessage.SetWarningText("´Ð³×ÀÓÀº 2±ÛÀÚ ÀÌ»óÀÌ¾î¾ß ÇÕ´Ï´Ù!");
                return;
            }

            foreach (var item in notFullKoreanArray)
            {
                if (inputField.text.Contains(item.ToString()))
                {
                    warningMessage.SetWarningText("Àß¸øµÈ ´Ð³×ÀÓÀÔ´Ï´Ù!");
                    return;
                }
            }
            foreach (var item in swearWordArray)
            {
                if (inputField.text.Contains(item))
                {
                    warningMessage.SetWarningText("Àß¸øµÈ ´Ð³×ÀÓÀÔ´Ï´Ù!");
                    return;
                }
            }
            startManager.CheckNickname(inputField.text);
        }
    }
}
