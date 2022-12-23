using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    static string nextScene;

    [SerializeField] Image progressBar;
    [SerializeField] Text Percentage;
    [SerializeField] Text GameTip;

    [SerializeField] private string[] LoadingTip;

    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    IEnumerator LoadSceneProcess()
    {
        GameTip.text = LoadingTip[Random.Range(0, 2)];
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;


        float timer = 0f;
        float timers = 0f;
        float timert = 0f;

        int Countone = 0;
        int Counttwo = 0;
        while (!op.isDone)
        {
            yield return null;

            if (progressBar.fillAmount < 0.33f)
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0f, 0.33f, timer / 0.7f);
                Percentage.text = (progressBar.fillAmount * 100f).ToString("F0") + " %";
            }
            else if (progressBar.fillAmount >= 0.33f && progressBar.fillAmount < 0.8f)
            {
                if(Countone == 0)
                {
                    Countone++;
                    yield return new WaitForSeconds(0.5f);
                }
                timers += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.33f, 0.8f, timers / 0.7f);
                Percentage.text = (progressBar.fillAmount * 100f).ToString("F0") + " %";
            }
            else
            {
                if (Counttwo == 0)
                {
                    Counttwo++;
                    yield return new WaitForSeconds(0.7f);
                }
                timert += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.8f, 1f, timert / 0.5f);
                Percentage.text = (progressBar.fillAmount * 100f).ToString("F0") + " %";
                if (progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }


















        /*float timer = 0f;
        while(!op.isDone)
        {
            yield return null;

            if (op.progress < 0.1f)
            {
                progressBar.fillAmount = op.progress;
                Percentage.text = op.progress * 100f + " %";
            }
            else
            {
                timer += Time.unscaledDeltaTime;

                progressBar.fillAmount = Mathf.Lerp(0.1f, 1f, timer / 2f);
                Percentage.text = (progressBar.fillAmount * 100f).ToString("F0") + " %";
                if (progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }   */
    }
}
