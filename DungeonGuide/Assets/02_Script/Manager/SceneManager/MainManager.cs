using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    [SerializeField] private Text[] unitLevelTextArray;
    private string[] pathArray = { "WarriorLevel", "AssassinLevel", "MagicianLevel", "ArchorLevel" };
    private void Start()
    {
        SoundManager.Instance.SetBGM(1);
        for (int i = 0; i < unitLevelTextArray.Length; i++)
        {
            unitLevelTextArray[i].text = "Lv : " + GameManager.Instance.FirebaseData.UnitDictionary[pathArray[i]].ToString();
        }
    }
    public void BtnEvt_ActiveObj(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }
    public void BtnEvt_LoadScene(int index)
    {
        GameManager.Instance.LoadScene(index);
    }
    public void BtnEvt_LoadingSecene()
    {
        GameManager.Instance.LoadSceneThroughLoadingScene(2);
    }
}
