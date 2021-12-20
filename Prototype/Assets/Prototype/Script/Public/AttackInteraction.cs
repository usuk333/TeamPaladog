using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInteraction : MonoBehaviour
{
    private enum UNIT_VALUE
    {
        PLAYER,
        UNIT,
        ENEMY
    }
    private Unit unit;
    private Enemy enemy;
    private Player player;
    [SerializeField] private UNIT_VALUE unitValue;
    private void Awake()
    {
        if(unitValue == UNIT_VALUE.UNIT)
        {
            unit = GetComponentInParent<Unit>();
        }
        else if(unitValue == UNIT_VALUE.ENEMY)
        {
            enemy = GetComponentInParent<Enemy>();
        }
        else if(unitValue == UNIT_VALUE.PLAYER)
        {
            player = GetComponentInParent<Player>();
        }

        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(unitValue == UNIT_VALUE.UNIT)
        {
            if (unit.UnitState == UNIT_STATE.NON_COMBAT)
            {
                if (collision.tag == "ENEMY")
                {
                    unit.UnitState = UNIT_STATE.BATTLE;
                    unit.CurrentEnemy = collision.GetComponent<Enemy>();
                }
            }
        }
        else if(unitValue == UNIT_VALUE.ENEMY)
        {
            if (enemy.EnemyState == UNIT_STATE.NON_COMBAT)
            {
                if (collision.tag == "UNIT" || collision.tag == "PLAYER")
                {
                    enemy.EnemyState = UNIT_STATE.BATTLE;
                    enemy.CurrentUnit = collision.gameObject;
                }
            }
        }
        else if(unitValue == UNIT_VALUE.PLAYER)
        {
            if(collision.tag == "ENEMY")
            {
                player.CurrentEnemy = collision.GetComponent<Enemy>();
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (unitValue == UNIT_VALUE.UNIT)
        {
            if (unit.UnitState == UNIT_STATE.NON_COMBAT)
            {
                if (collision.tag == "ENEMY")
                {
                    unit.UnitState = UNIT_STATE.BATTLE;
                    unit.CurrentEnemy = collision.GetComponent<Enemy>();
                }
            }
        }
        else if(unitValue == UNIT_VALUE.ENEMY)
        {
            if (enemy.EnemyState == UNIT_STATE.NON_COMBAT)
            {
                if (collision.tag == "UNIT" || collision.tag == "PLAYER")
                {
                    enemy.EnemyState = UNIT_STATE.BATTLE;
                    enemy.CurrentUnit = collision.gameObject;
                }
            }
        }
        else if (unitValue == UNIT_VALUE.PLAYER)
        {
            if (collision.tag == "ENEMY")
            {
                player.CurrentEnemy = collision.GetComponent<Enemy>();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(unitValue == UNIT_VALUE.ENEMY)
        {
            if(enemy.EnemyState == UNIT_STATE.BATTLE)
            {
                if(collision.tag == "PLAYER")
                {
                   if(enemy.CurrentUnit == collision.gameObject)
                    {
                        enemy.CurrentUnit = null;
                        enemy.EnemyState = UNIT_STATE.NON_COMBAT;
                    }
                }
            }
        }
        else if(unitValue == UNIT_VALUE.PLAYER)
        {
            if(collision.tag == "ENEMY")
            {
                if(player.CurrentEnemy == collision.gameObject.GetComponent<Enemy>())
                {
                    player.CurrentEnemy = null;
                }
            }
        }
    }
}
