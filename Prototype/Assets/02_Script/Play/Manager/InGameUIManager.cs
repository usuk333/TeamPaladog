using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject clearPopUpObj;
    [SerializeField] private GameObject rewardObj;
    [SerializeField] private Image[] clearImageArray;
    [SerializeField] private Text clearText;
    [SerializeField] private GameObject gameOverPopUp;
    [SerializeField] private Image[] gameOverImageArray;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text mainText;
    [SerializeField] private GameObject pauseObj;
    [SerializeField] private GameObject settingObj;
    [SerializeField] private Image sceneMoveImage;

    private bool goMain;

    private Text stageInfoText;
    private Text stageTitleText;
    [SerializeField] private GameObject[] rewardObjArray;
    private Slider expSlider;
    private Text expText;

    public static InGameUIManager instance;
    private void Awake()
    {
        instance = this;
        stageInfoText = rewardObj.transform.Find("Info").GetChild(0).GetComponent<Text>();
        stageTitleText = rewardObj.transform.Find("Info").GetChild(1).GetComponent<Text>();
        expSlider = rewardObj.transform.Find("Exp").GetComponentInChildren<Slider>();
        expText = expSlider.GetComponentInChildren<Text>();
    }
    private IEnumerator Start()
    {
        stageInfoText.text = $"{StageInfo.bossIndex + 1} - {StageInfo.difficulty}";
        stageTitleText.text = InGameManager.Instance.Title;
        CheckReward();
        SetExpData();
        yield return new WaitForSeconds(0.5f);
        sceneMoveImage.DOFade(0, 2f);
        yield return new WaitForSeconds(2f);
        sceneMoveImage.gameObject.SetActive(false);
    }
    private void CheckReward()
    {
        int[] rewardArray = InGameManager.Instance.StageReward.GetReward();

        for (int i = 0; i < rewardArray.Length; i++)
        {
            if(rewardArray[i] <= 0)
            {
                rewardObjArray[i].SetActive(false);
                continue;
            }
            rewardObjArray[i].GetComponentInChildren<Text>().text = $"x{rewardArray[i]}";
        }
    }
    private void SetExpData()
    {
        expSlider.maxValue = DataEquation.PlayerMaxExpToLevel();
        expSlider.value = System.Convert.ToInt32(GameManager.Instance.FirebaseData.InfoDictionary["EXP"]);
        expText.text = $"{expSlider.value} / {expSlider.maxValue}";
    }
    public void BtnEvt_UseSkill(int index)
    {
        InGameManager.Instance.Player.UseSkill(index);
    }
    public void EventTrigger_CastingButtonUp()
    {
        InGameManager.Instance.Player.castingButtonDown = false;
        //InGameManager.Instance.Player.isCastFinish = false;
    }
    public void EventTrigger_CastingButtonDown()
    {
        InGameManager.Instance.Player.castingButtonDown = true;
    }
    public void BtnEvt_Pause()
    {
        pauseObj.SetActive(!pauseObj.activeSelf);
        if (pauseObj.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1 ;
        }
    }
    public void BtnEvt_GoMainGameClear()
    {
        LoadingSceneController.LoadScene("StageSection");
        SoundManager.Instance.SetBGM(2);
    }
    public void BtnEvt_GoMain()
    {
        pauseObj.SetActive(!pauseObj.activeSelf);
        Time.timeScale = 1;
        InGameManager.Instance.SetGameOver();
    }
    public void BtnEvt_GoMainGameOver()
    {
        if (!goMain) return;
        LoadingSceneController.LoadScene("StageSection");
        SoundManager.Instance.SetBGM(2);
    }
    public void BtnEvt_ActiveSetting()
    {
        settingObj.SetActive(!settingObj.activeSelf);
    }
    public void BtnEvt_Quit()
    {
        Application.Quit();
    }
    public void ShowClearPopUp()
    {
        clearPopUpObj.SetActive(true);
        foreach (var item in clearImageArray)
        {
            item.DOFade(1, 2f);
        }
        clearText.DOFade(1, 2f);
        clearImageArray[0].transform.DOLocalMoveY(263, 0.5f).SetDelay(2);
        rewardObj.transform.DOScale(1, 1f).SetDelay(3);
        StartCoroutine(Co_RewardAnim());
    }
    private IEnumerator Co_RewardAnim()
    {
        yield return new WaitForSeconds(4f);
        StartCoroutine(Co_ExpTextAnim());
        for (int i = 0; i < rewardObjArray.Length; i++)
        {
            if (!rewardObjArray[i].activeSelf) continue;
            rewardObjArray[i].transform.DOScale(1, 0.5f);
            yield return new WaitForSeconds(0.5f);
        }
        if(expSlider.value + InGameManager.Instance.StageReward.exp >= expSlider.maxValue)
        {
            for (int i = GameManager.Instance.FirebaseData.GetLevelUpCount((int)expSlider.value + InGameManager.Instance.StageReward.exp); i > 0; i--)
            {
                expSlider.DOValue(expSlider.maxValue, 1f);
                yield return new WaitForSeconds(1.2f);
                expSlider.value = 0;
                expSlider.maxValue = DataEquation.PlayerMaxExpToLevel();
            }
        }
        expSlider.DOValue(System.Convert.ToInt32(GameManager.Instance.FirebaseData.InfoDictionary["EXP"]), 1f);
    }
    private IEnumerator Co_ExpTextAnim()
    {
        while (true)
        {
            yield return null;
            expText.text = $"{expSlider.value.ToString("F0")} / {expSlider.maxValue}";
        }
    }
    public void ShowGameOverPopUp()
    {
        gameOverPopUp.SetActive(true);
        foreach (var item in gameOverImageArray)
        {
            item.DOFade(1, 2f);
        }
        gameOverText.DOFade(1, 2f);
        mainText.DOFade(1, 1f).SetDelay(2f);
        StartCoroutine(Co_GameOver());
    }
    private IEnumerator Co_GameOver()
    {
        yield return new WaitForSeconds(1f);
        goMain = true;
        while (true)
        {
            mainText.DOFade(0, 1f);
            yield return new WaitForSeconds(1.5f);
            mainText.DOFade(1, 1f);
            yield return new WaitForSeconds(1.5f);
        }
    }
}
