using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRange : MonoBehaviour
{
    private enum ERangeKind
    {
        SpeedUp,
        Invincibility,
        Healing,
        Teleport
    }
    private Player player;
    [SerializeField] private ERangeKind eRangeKind;
    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "UNIT")
        {
            switch (eRangeKind)
            {
                case ERangeKind.SpeedUp:
                    player.SpeedUpUnits.Add(collision.GetComponent<Unit>());
                    break;
                case ERangeKind.Invincibility:
                    player.InvincibilityUnits.Add(collision.GetComponent<Unit>());
                    break;
                case ERangeKind.Healing:
                    player.HealingUnits.Add(collision.GetComponent<Unit>());
                    break;
                case ERangeKind.Teleport:
                    player.TeleportUnits.Add(collision.transform);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }
    }
}
