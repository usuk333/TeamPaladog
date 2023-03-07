using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrior : PlayerSkill
{
    public override IEnumerator Co_UseSkill()
    {
        if (isCoolTime || !CheckCurrentMana()) yield break;
        StartCoroutine(base.Co_UseSkill());
        player.SetState(2);
        yield return new WaitForSeconds(2f);
        player.useSkill = false;
        if (player.CurrentHp <= 0) yield break;
        player.PlayAudio(2);
        player.PlayerParticleArray[0].startLifetime = valueArray[3];
        player.PlayerParticleArray[0].Play();
        foreach (var item in InGameManager.Instance.Units)
        {
            item.CommonStatus.Shield = item.CommonStatus.MaxHp * valueArray[0] / 100;
            item.ShieldEffect.startLifetime = valueArray[3];
            item.ShieldEffect.Play();
        }
        player.Shield = player.MaxHp * valueArray[0] / 100;
        yield return new WaitForSeconds(valueArray[3]);
        foreach (var item in InGameManager.Instance.Units)
        {
            item.CommonStatus.Shield = 0;
        }
        player.Shield = 0;

    }

}
