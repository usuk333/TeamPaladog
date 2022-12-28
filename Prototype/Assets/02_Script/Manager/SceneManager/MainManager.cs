using UnityEngine;

public class MainManager : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.SetBGM(1);
    }
    public void BtnEvt_ActiveObj(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }
    public void BtnEvt_LoadScene(int index)
    {
        GameManager.Instance.LoadScene(index);
    }
}
