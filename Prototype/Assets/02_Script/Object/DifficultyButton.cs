using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    [SerializeField] private Sprite[] buttonSprite;
    private Transform[] buttonArray = new Transform[3];
    private string[] difArray = { "E", "N", "H" };
    private void Awake()
    {
        for (int i = 0; i < buttonArray.Length; i++)
        {
            buttonArray[i] = transform.GetChild(i);
        }
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void UpdateDifficultyButton(int nowStage)
    {
        int index = nowStage <= 1 ? 1 : nowStage - 1;
        if(nowStage == 1)
        {
            if (!System.Convert.ToBoolean(GameManager.Instance.FirebaseData.StageDictionary[$"S{nowStage}{difArray[0]}Clear"]))
            {
                ChangeButtonStatus(0, true);
                for (int i = 1; i >= 0; i--)
                {
                    ChangeButtonStatus(i+1, false);
                }
                return;
            }
        }
        if (System.Convert.ToBoolean(GameManager.Instance.FirebaseData.StageDictionary[$"S{index}{difArray[0]}Clear"]))
        {
            ChangeButtonStatus(0, true);
        }
        else
        {
            ChangeButtonStatus(0, false);
        }
        for (int i = 1; i >= 0; i--)
        {
            if (System.Convert.ToBoolean(GameManager.Instance.FirebaseData.StageDictionary[$"S{nowStage}{difArray[i]}Clear"]))
            {
                ChangeButtonStatus(i+1, true);
            }
            else
            {
                ChangeButtonStatus(i+1, false);
            }
        }
    }
    private void ChangeButtonStatus(int index, bool isEnabled)
    {
        buttonArray[index].GetComponent<Image>().sprite = buttonSprite[isEnabled ? 0 : 1];
        buttonArray[index].GetComponent<Button>().enabled = isEnabled;
    }
}
