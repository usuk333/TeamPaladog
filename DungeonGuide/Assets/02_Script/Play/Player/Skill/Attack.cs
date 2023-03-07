using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : PlayerSkill
{
    public override IEnumerator Co_UseSkill()
    {
        if (isCoolTime || !CheckCurrentMana()) yield break;
        StartCoroutine(base.Co_UseSkill());
        float rand = Random.Range(0f, 100f);
        int index;
        if (rand <= 30) index = 0;           
        else if (rand > 30 && rand <= 70) index = 1;
        else index = 2;
        player.SetState(index + 5);
        yield return new WaitForSeconds(1.167f);
        player.useSkill = false;
        player.PlayAudio(index > 1? 1 : 0);
        if(index > 1) player.PlayerParticleArray[3].Play();
        InGameManager.Instance.Boss.CommonStatus.DecreaseHp(valueArray[0] + (50 * index));
    }
}
