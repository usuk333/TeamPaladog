using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInteraction : MonoBehaviour
{
    private enum ECharacterValue
    {
        Player,
        Unit,
        Enemy,
        Boss
    }
    private Unit unit;
    private Enemy enemy;
    private Player player;
    private Boss boss;

    [SerializeField] private ECharacterValue unitValue;
    private void Awake()
    {
        if (unitValue == ECharacterValue.Unit)
        {
            unit = GetComponentInParent<Unit>();
        }
        else if (unitValue == ECharacterValue.Enemy)
        {
            enemy = GetComponentInParent<Enemy>();
        }
        else if (unitValue == ECharacterValue.Player)
        {
            player = GetComponentInParent<Player>();
        }
        else if (unitValue == ECharacterValue.Boss)
        {
            boss = GetComponentInParent<Boss>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (unitValue == ECharacterValue.Unit)
        {
            UpdateUnitCurrentTarget(collision.gameObject);
        }
        else if (unitValue == ECharacterValue.Enemy)
        {
            UpdateEnemyCurrentTarget(collision.gameObject);
        }
        else if (unitValue == ECharacterValue.Player)
        {
            UpdatePlayerCurrentTarget(collision.gameObject);
        }
        else if (unitValue == ECharacterValue.Boss)
        {
            UpdateBossCurrentTarget(collision.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (unitValue == ECharacterValue.Unit)
        {
            UpdateUnitCurrentTarget(collision.gameObject);
        }
        else if (unitValue == ECharacterValue.Enemy)
        {
            UpdateEnemyCurrentTarget(collision.gameObject);
        }
        else if (unitValue == ECharacterValue.Player)
        {
            UpdatePlayerCurrentTarget(collision.gameObject);
        }
        else if (unitValue == ECharacterValue.Boss)
        {
            UpdateBossCurrentTarget(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (unitValue == ECharacterValue.Enemy)
        {
            if (collision.tag == "PLAYER")
            {
                enemy.Player = null;
            }
            else if (collision.tag == "UNIT")
            {
                if (enemy.CurrentUnit == collision.GetComponent<Unit>())
                {
                    enemy.CurrentUnit = null;
                }
            }
        }  
        else if (unitValue == ECharacterValue.Player)
        {
            if (collision.tag == "ENEMY")
            {
                if (player.CurrentEnemy == collision.GetComponent<Enemy>())
                {
                    player.CurrentEnemy = null;
                }
            }
            else if (collision.tag == "BOSS")
            {
                player.Boss = null;
            }
        }
        else if (unitValue == ECharacterValue.Boss)
        {
            if (collision.tag == "PLAYER")
            {
                boss.Player = null;
            }
            else if(collision.tag == "UNIT")
            {
                if(boss.CurrentUnit == collision.GetComponent<Unit>())
                {
                    boss.CurrentUnit = null;
                }
            }
        }
        else if (unitValue == ECharacterValue.Unit)
        {
            if (collision.tag == "ENEMY")
            {
                if (unit.CurrentEnemy == collision.GetComponent<Enemy>())
                {
                    unit.CurrentEnemy = null;
                }
            }
            else if (collision.tag == "BOSS")
            {
                unit.Boss = null;                                   
            }
        }      
    }
    private void UpdateUnitCurrentTarget(GameObject target)
    {
        if (unit.UnitKinds == Unit.EUnitKinds.Druid)
        {
            if (target.tag == "ENEMY" || target.tag == "BOSS")
            {
                if (!unit.GetComponent<Druid>().Collisions.Contains(target.gameObject))
                {
                    unit.GetComponent<Druid>().Collisions.Add(target.gameObject);
                    Debug.Log("Ãß°¡");
                }
            }
        }
        if (target.tag == "ENEMY" && target.GetComponent<Shielder>())
        {
            if (unit.CurrentEnemy != null && !unit.CurrentEnemy.GetComponent<Shielder>())
            {
                unit.CurrentEnemy = target.GetComponent<Enemy>();
            }
            }
        if (target.tag == "ENEMY")
        {
            if (unit.CurrentEnemy == null)
            {
                unit.CurrentEnemy = target.GetComponent<Enemy>();
            }
        }
        else if(target.tag == "BOSS")
        {
            if(unit.Boss == null)
            {
                unit.Boss = target.GetComponent<Boss>();
            }
        }
    }
    private void UpdateEnemyCurrentTarget(GameObject target)
    {
        if (target.tag == "UNIT" && target.GetComponent<Shielder>())
        {
            if (enemy.CurrentUnit != null && !enemy.CurrentUnit.GetComponent<Shielder>())
            {
                enemy.CurrentUnit = target.GetComponent<Unit>();
            }
        }
        if (target.tag == "UNIT")
        {
            if (enemy.CurrentUnit == null)
            {
                enemy.CurrentUnit = target.GetComponent<Unit>();
            }
        }
        else if (target.tag == "PLAYER")
        {
            if (enemy.Player == null)
            {
                enemy.Player = target.GetComponent<Player>();
            }
        }
    }
    private void UpdatePlayerCurrentTarget(GameObject target)
    {
        if (target.tag == "ENEMY" && target.GetComponent<Shielder>())
        {
            if (player.CurrentEnemy != null && !player.CurrentEnemy.GetComponent<Shielder>())
            {
                player.CurrentEnemy = target.GetComponent<Enemy>();
            }
        }
        if (target.tag == "ENEMY")
        {
            if (player.CurrentEnemy == null)
            {
                player.CurrentEnemy = target.GetComponent<Enemy>();
            }
        }
        else if (target.tag == "BOSS")
        {
            if (player.Boss == null)
            {
                player.Boss = target.GetComponent<Boss>();
            }
        }
    }
    private void UpdateBossCurrentTarget(GameObject collision)
    {
        if (collision.tag == "UNIT" && collision.GetComponent<Shielder>())
        {
            if (boss.CurrentUnit != null && !boss.CurrentUnit.GetComponent<Shielder>())
            {
                boss.CurrentUnit = collision.GetComponent<Unit>();
            }
        }
        if (collision.tag == "UNIT" && collision.GetComponent<Assasin>())
        {
            if (boss.CurrentUnit != null && !boss.CurrentUnit.GetComponent<Shielder>())
            {
                boss.CurrentUnit = collision.GetComponent<Unit>();
            }
        }
        if (collision.tag == "UNIT")
        {
            boss.CurrentUnit = collision.GetComponent<Unit>();
        }
        else if (collision.tag == "PLAYER")
        {
            boss.Player = collision.GetComponent<Player>();
        }
        
    }
}
