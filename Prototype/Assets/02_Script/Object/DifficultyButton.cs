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
                buttonArray[0].GetComponent<Image>().sprite = buttonSprite[0];
                buttonArray[0].GetComponent<Button>().enabled = true;
                for (int i = 1; i >= 0; i--)
                {
                    buttonArray[i + 1].GetComponent<Image>().sprite = buttonSprite[1];
                    buttonArray[i + 1].GetComponent<Button>().enabled = false;
                }
                return;
            }
        }
        if (System.Convert.ToBoolean(GameManager.Instance.FirebaseData.StageDictionary[$"S{index}{difArray[0]}Clear"]))
        {
            buttonArray[0].GetComponent<Image>().sprite = buttonSprite[0];
            buttonArray[0].GetComponent<Button>().enabled = true;
        }
        else
        {
            buttonArray[0].GetComponent<Image>().sprite = buttonSprite[1];
            buttonArray[0].GetComponent<Button>().enabled = false;
        }
        for (int i = 1; i >= 0; i--)
        {
            if (System.Convert.ToBoolean(GameManager.Instance.FirebaseData.StageDictionary[$"S{nowStage}{difArray[i]}Clear"]))
            {
                buttonArray[i+1].GetComponent<Image>().sprite = buttonSprite[0];
                buttonArray[i+1].GetComponent<Button>().enabled = true;
            }
            else
            {
                buttonArray[i+1].GetComponent<Image>().sprite = buttonSprite[1];
                buttonArray[i+1].GetComponent<Button>().enabled = false;
            }
        }
    }
}
