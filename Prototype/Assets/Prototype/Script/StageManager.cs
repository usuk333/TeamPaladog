using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour //스킬을 아무것도 안 고른 경우 0번 스킬(스피드 증가)이 넘어가는 거 막아야 함
{
    private GameObject skillSelectPopUp;
    [SerializeField] private int firstSkill;
    [SerializeField] private int secondSkill;
    [SerializeField] private Sprite defaultSkillSprite;
    [SerializeField] private Button firstSkillButton;
    [SerializeField] private Button secondSkillButton;
    [SerializeField] private Button currentSkillButton;
    [SerializeField] private Button[] skillButtons;
    [SerializeField] private PlayerSkill[] playerSkills;
    public void BtnEvt_ActiveSkillSelectPopUp(bool isActive)
    {
        skillSelectPopUp.SetActive(isActive);
    }
    public void BtnEvt_SelectSkill(int index)
    {
        if(currentSkillButton == firstSkillButton)
        {
            if (secondSkill == index)
            {
                secondSkill = firstSkill;
                secondSkillButton.image.sprite = firstSkillButton.image.sprite;
            }
            firstSkillButton.image.sprite = playerSkills[index].Icon;
            firstSkill = index;
        }
        else if(currentSkillButton == secondSkillButton)
        {
            if(firstSkill == index)
            {
                firstSkill = secondSkill;
                firstSkillButton.image.sprite = secondSkillButton.image.sprite;
            }
            secondSkillButton.image.sprite = playerSkills[index].Icon;
            secondSkill = index;
        }
    }
    public void BtnEvt_LoadInGameScene()
    {
        /* SkillData.Instance.FirstSkill = firstSkill + 2;
         SkillData.Instance.SecondSkill = secondSkill + 2;*/
        SkillData.Instance.First = playerSkills[firstSkill];
        SkillData.Instance.Second = playerSkills[secondSkill];
        SceneManager.LoadScene("SampleScene");
    }
    public void BtnEvt_SetCurrentSkillButton()
    {
        currentSkillButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
    }
    private void Awake()
    {
        skillSelectPopUp = GameObject.Find("SkillSelectPopUpBackGround");
        skillSelectPopUp.SetActive(false);
        InitSkillButtons();
    }
    private void InitSkillButtons()
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            skillButtons[i].image.sprite = playerSkills[i].Icon;
        }
    }
}
public class SkillData
{
    private static SkillData instance;
    private PlayerSkill first;
    private PlayerSkill second;
    private int firstSkill;
    private int secondSkill;
    public static SkillData Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new SkillData();
            }
            return instance;
        }
    }

    public int FirstSkill { get => firstSkill; set => firstSkill = value; }
    public int SecondSkill { get => secondSkill; set => secondSkill = value; }
    public PlayerSkill First { get => first; set => first = value; }
    public PlayerSkill Second { get => second; set => second = value; }
}

