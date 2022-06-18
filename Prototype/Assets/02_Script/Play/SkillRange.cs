using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRange : MonoBehaviour
{
    private enum SkillKind
    {
        Barrier,
        Heal,
        Rage
    }
    [SerializeField] private SkillKind skillKind;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("UNIT"))
        {
            switch (skillKind)
            {
                case SkillKind.Barrier:
                    if (InGameManager.Instance.Player.SkillCoolTime[0])
                    {
                        return;
                    }
                    InGameManager.Instance.Player.BarrierList.Add(collision.GetComponent<Unit>());                     
                    break;
                case SkillKind.Heal:
                    InGameManager.Instance.Player.HealList.Add(collision.GetComponent<Unit>());
                    break;
                case SkillKind.Rage:
                    InGameManager.Instance.Player.Skill_Rage(collision.GetComponent<Unit>(), true);
                    break;
                default:
                    break;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("UNIT"))
        {
            switch (skillKind)
            {
                case SkillKind.Barrier:
                    if (InGameManager.Instance.Player.SkillCoolTime[0])
                    {
                        return;
                    }
                    InGameManager.Instance.Player.BarrierList.Remove(collision.GetComponent<Unit>());
                    break;
                case SkillKind.Heal:
                    InGameManager.Instance.Player.HealList.Remove(collision.GetComponent<Unit>());
                    break;
                case SkillKind.Rage:
                    InGameManager.Instance.Player.Skill_Rage(collision.GetComponent<Unit>(), false);
                    break;
                default:
                    break;
            }
        }
    }
}
