using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : PlayerSkill
{
    public override IEnumerator Co_UseSkill()
    {
        if (isCoolTime || !CheckCurrentMana()) yield break;
        StartCoroutine(base.Co_UseSkill());
        player.SetState(3);
        yield return new WaitForSeconds(2f);
        player.useSkill = false;
        if (player.CurrentHp <= 0) yield break;
        player.PlayAudio(2);
        player.PlayerParticleArray[1].Play();
        foreach (var item in InGameManager.Instance.Units)
        {
            item.CommonStatus.IncreaseHp(item.CommonStatus.MaxHp * valueArray[0] / 100);
            item.HealEffect.Play();
        }
        player.IncreaseCurrentHp(player.MaxHp * valueArray[0] / 100);
    }
}
