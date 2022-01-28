using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour //인게임에서 플레이어와 상호작용하는 버튼들의 기능 스크립트
{
    private Player player;
    [SerializeField] private Button[] skillButtons;
    public void BtnEvt_InstantiateUnit(int index)
    {
        InGameManager.Instance.InstantiateUnit(index);
    }
    public void BtnEvt_UseSkill(int index)
    {
        player.UseSkill(index);
        StartCoroutine(Co_CoolTime(index));
    }
    public void BtnEvt_Left()
    {
        player.IsLeft = !player.IsLeft;
    }
    public void BtnEvt_Right()
    {
        player.IsRight = !player.IsRight;
    }
    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }
    private IEnumerator Co_CoolTime(int index)
    {
        skillButtons[index].interactable = false;
        var coolTimeImage = skillButtons[index].transform.Find("Image");
        coolTimeImage.gameObject.SetActive(true);
        float coolTime = player.PlayerSkills[index].CoolTime;
        while (coolTime > 0)
        {
            coolTime -= Time.deltaTime;
            coolTimeImage.GetComponent<Image>().fillAmount -= 1 / player.PlayerSkills[index].CoolTime * Time.deltaTime;
            yield return null;
        }
        coolTimeImage.gameObject.SetActive(false);
        coolTimeImage.GetComponent<Image>().fillAmount = 1;
        skillButtons[index].interactable = true;
    }
}
