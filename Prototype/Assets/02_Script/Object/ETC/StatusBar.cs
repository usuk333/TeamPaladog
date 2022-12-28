using UnityEngine;
using UnityEngine.UI;


namespace ETC
{
    public class StatusBar : MonoBehaviour
    {
        [SerializeField] protected string statusName;
        protected Text valueText;
        private void Awake()
        {
            valueText = GetComponentInChildren<Text>();
        }
        // Start is called before the first frame update
        private void Start()
        {
            UpdateValueText();
        }
        public virtual void UpdateValueText()
        {
            if (statusName == "SkillPoints")
            {
                valueText.text = GameManager.Instance.FirebaseData.SkillDictionary[statusName].ToString();
            }
            else
            {
                valueText.text = GameManager.Instance.FirebaseData.InfoDictionary[statusName].ToString();
            }
        }
    }
}
