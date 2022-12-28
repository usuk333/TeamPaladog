using UnityEngine;
using UnityEngine.SceneManagement;
using Data;
public class GameManager : MonoBehaviour
{
    private string version;

    private static GameManager instance;

    private FirebaseData firebaseData;

    private StageInfo stageInfo;

    public string Version { get => version; }
    public static GameManager Instance { get => instance; }
    public FirebaseData FirebaseData { get => firebaseData; set => firebaseData = value; }
    public StageInfo StageInfo { get => stageInfo; set => stageInfo = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else if(instance != this)
        {
            Destroy(this);
            return;
        }
        Application.targetFrameRate = 60;
        Screen.SetResolution(1280, 720, true);
        version = Application.version;
    }
    /// <summary>
    /// 0 = 메인, 1 = 유닛, 2 = 스킬, 3 = 스테이지 섹션, 4 = 스테이지
    /// </summary>
    /// <param name="index"></param>
    public void LoadScene(int index)
    {
        switch (index)
        {
            case 0:
                SceneManager.LoadScene("Main");
                break;
            case 1:
                SceneManager.LoadScene("UnitShop");
                break;
            case 2:
                SceneManager.LoadScene("Skill");
                break;
            case 3:
                SceneManager.LoadScene("StageSection");
                break;
            case 4:
                SceneManager.LoadScene("Stage");
                break;
            default:
                Debug.LogError("존재하지 않는 씬 인덱스");
                break;
        }
    }
    /// <summary>
    /// 0 = 메인, 1 = 스테이지
    /// </summary>
    /// <param name="index"></param>
    public void LoadSceneThroughLoadingScene(int index)
    {
        switch (index)
        {
            case 0:
                LoadingSceneController.LoadScene("Main");
                break;
            case 1:
                LoadingSceneController.LoadScene("Stage");
                break;
            default:
                Debug.LogError("존재하지 않는 씬 인덱스");
                break;
        }
    }
}
