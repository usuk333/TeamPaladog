using UnityEngine;
using UnityEngine.UI;

public class MainButton : MonoBehaviour
{
    public void BtnEvt_LoadMainScene()
    {
        GameManager.Instance.LoadScene(0);
    }
}
