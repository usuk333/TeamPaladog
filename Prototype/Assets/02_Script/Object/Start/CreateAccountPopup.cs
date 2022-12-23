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
        //�г��� ���͸�
        private char[] notFullKoreanArray = { '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��' };
        private string[] swearWordArray = { "����", "����", "��", "��", "������", "���ݸ�", "�Ϲ�","�̹�","�̱��", "�ϱ��", "����","����", "���", "�Ƕ���", "����", "��ģ", "â��","ì��","ì��", "â��",
        "����","���", "����", "�Ҿ�", "�ζ�", "����", "����", "�ù�","����", "��", "����", "��â", "������", "����", "����", "��", "����", "���", "�p��" };
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
            inputField.onValueChanged.AddListener((word) => inputField.text = Regex.Replace(word, @"[^��-����-�R]", ""));
        }
        public void BtnEvt_CreateNewAccount()
        {
            CreateNewAccount();
        }
        private void CreateNewAccount()
        {
            if (inputField.text.Length < 2)
            {
                warningMessage.SetWarningText("�г����� 2���� �̻��̾�� �մϴ�!");
                return;
            }

            foreach (var item in notFullKoreanArray)
            {
                if (inputField.text.Contains(item.ToString()))
                {
                    warningMessage.SetWarningText("�߸��� �г����Դϴ�!");
                    return;
                }
            }
            foreach (var item in swearWordArray)
            {
                if (inputField.text.Contains(item))
                {
                    warningMessage.SetWarningText("�߸��� �г����Դϴ�!");
                    return;
                }
            }
            startManager.CheckNickname(inputField.text);
        }
    }
}
