using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    [SerializeField] protected Image coolTimeImage;
    [SerializeField] protected bool isCoolTime;

    [SerializeField] protected float[] valueArray;
    [SerializeField] protected Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }
    public virtual void SetSkillData(float[] valueArray)
    {
        this.valueArray = valueArray;
    }
    public virtual IEnumerator Co_UseSkill()
    {
        player.DecreaseMp(valueArray[1]);
        StartCoroutine(Co_UpdateIsCoolTime());
        player.useSkill = true;
        yield return null;
    }
    protected bool CheckCurrentMana()
    {
        if(player.CurrentMp >= valueArray[1])
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private IEnumerator Co_UpdateIsCoolTime()
    {
        isCoolTime = true;
        float timer = valueArray[2];
        coolTimeImage.gameObject.SetActive(isCoolTime);
        while(timer > 0)
        {
            coolTimeImage.fillAmount = 1 / valueArray[2] * timer; 
            timer -= Time.deltaTime;
            yield return null;
        }
        isCoolTime = false;
        coolTimeImage.gameObject.SetActive(isCoolTime);
    }
}

