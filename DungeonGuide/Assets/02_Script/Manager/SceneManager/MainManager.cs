using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class MainManager : MonoBehaviour
{
    private bool isPrint;
    [SerializeField] private Text mainText;
    [SerializeField] private Text[] unitLevelTextArray;
    [SerializeField] private string[] mainStringArray;
    private string[] pathArray = { "WarriorLevel", "AssassinLevel", "MagicianLevel", "ArchorLevel" };
    private void Start()
    {
        SoundManager.Instance.SetBGM(1);
        for (int i = 0; i < unitLevelTextArray.Length; i++)
        {
            unitLevelTextArray[i].text = "Lv. " + GameManager.Instance.FirebaseData.UnitDictionary[pathArray[i]].ToString();
        }
        StartCoroutine(Co_MainTextAnim());
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
    public IEnumerator Co_MainTextAnim()
    {
        isPrint = true;
        mainText.text = null;
        int rand = Random.Range(0, mainStringArray.Length);
        char[] charArray = mainStringArray[rand].ToCharArray();
        for (int i = 0; i < charArray.Length; i++)
        {
            mainText.text += charArray[i];
            if(charArray[i] == '?' || charArray[i] == '.' || charArray[i] == '!')
            {
                mainText.text += "\n\n";
            }
            yield return new WaitForSeconds(0.1f);
        }
        isPrint = false;
    }
    public void BtnEvt_ChangeText()
    {
        if (isPrint) return;
        StartCoroutine(Co_MainTextAnim());
    }
}
