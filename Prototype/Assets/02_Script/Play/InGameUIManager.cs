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

    private bool goMain;

    public static InGameUIManager instance;
    private void Awake()
    {
        instance = this;
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
    public void BtnEvt_GoMain()
    {
        LoadingSceneController.LoadScene("Test_StageSelect");
    }
    public void BtnEvt_GoMainGameOver()
    {
        if (!goMain) return;
        LoadingSceneController.LoadScene("Test_StageSelect");
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
