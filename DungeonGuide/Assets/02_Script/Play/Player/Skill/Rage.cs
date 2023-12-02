using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rage : PlayerSkill
{
    [SerializeField] private GameObject rageObj;
    public override IEnumerator Co_UseSkill()
    {
        if (isCoolTime || !CheckCurrentMana()) yield break;
        StartCoroutine(base.Co_UseSkill());
        if (rageObj.activeSelf)
        {
            rageObj.SetActive(false);
            player.PlayerParticleArray[2].Stop();
            player.useSkill = false;
            yield break;
        }
        player.SetState(4);
        yield return new WaitForSeconds(2f);
        player.useSkill = false;
        if (player.CurrentHp <= 0) yield break;
        rageObj.SetActive(true);
        player.PlayAudio(3);
        player.PlayerParticleArray[2].Play();
        StartCoroutine(Co_DecreaseMp());
        while (rageObj.activeSelf)
        {
            yield return null;
            if (!rageObj.activeSelf)
            {
                break;
            }
            if (player.CurrentMp > 0) continue;
            Debug.Log("마나가 부족하여 스킬이 비활성화 됩니다.");
            rageObj.SetActive(false);
            player.PlayerParticleArray[2].Stop();
        }
    }
    private IEnumerator Co_DecreaseMp()
    {
        while (rageObj.activeSelf)
        {
            yield return new WaitForSeconds(1f);
            player.DecreaseMp(valueArray[1]);
        }
    }
    public void Skill_Rage(Unit unit, bool isIn)
    {
        if (isIn)
        {
            unit.CommonStatus.CurrentAttackDamage += unit.CommonStatus.AttackDamage * valueArray[0]/ 100;
            if (unit.RageEffect.isPlaying)
            {
                return;
            }
            unit.RageEffect.Play();
        }
        else
        {
            unit.CommonStatus.CurrentAttackDamage = unit.CommonStatus.AttackDamage;
            unit.RageEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}
