using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inside : MonoBehaviour
{
    private int explosionCount = 0;
    [SerializeField] private Transform manaExplosion;
    [SerializeField] private float manaExplosionDuration;
    [SerializeField] private float decreaseMpPercent;

    private void ManaExplosion()
    {
        if (InGameManager.Instance.Boss.CollisionsArray[0].Contains(InGameManager.Instance.Player.gameObject))
        {
            InGameManager.Instance.Player.DecreaseMp(InGameManager.Instance.Player.MaxMp * decreaseMpPercent / 100);
            return;
        }
        foreach (var item in InGameManager.Instance.Units)
        {
            if (InGameManager.Instance.Boss.CollisionsArray[0].Contains(item.gameObject))
            {
                item.CommonStatus.DecreaseHp(item.CommonStatus.MaxHp);
                explosionCount++;
            }
        }
    }
    private IEnumerator Co_ManaExplosion()
    {
        while (true)
        {
            manaExplosion.gameObject.SetActive(false);
            int rand = Random.Range(5, 10);
            yield return new WaitForSeconds(rand);
            manaExplosion.gameObject.SetActive(true);
            int index;
            while (true)
            {
                index = Random.Range(0, InGameManager.Instance.Units.Length);
                if(InGameManager.Instance.Units[index].CommonStatus.CurrentHp > 0)
                {
                    break;
                }
                yield return null;
            }     
            manaExplosion.position = InGameManager.Instance.Units[index].transform.position;
            yield return new WaitForSeconds(manaExplosionDuration);
            ManaExplosion();
            if(explosionCount >= 4)
            {
                yield break;
            }
        }
    }
    private void Start()
    {
        StartCoroutine(Co_ManaExplosion());
    }
}
