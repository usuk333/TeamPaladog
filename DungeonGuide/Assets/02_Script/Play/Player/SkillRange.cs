using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRange : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("UNIT"))
        {
            InGameManager.Instance.Player.Skill_Rage(collision.GetComponent<Unit>(), true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("UNIT"))
        {
            InGameManager.Instance.Player.Skill_Rage(collision.GetComponent<Unit>(), false);
        }
    }
}
