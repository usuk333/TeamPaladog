using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest : MonoBehaviour //사제 유닛의 기능 스크립트
{
    private Unit unit;
    private Enemy enemy;
    private Player player;
    private Boss boss;
    [SerializeField] private EParent eParent;
    [SerializeField] private List<Unit> units = new List<Unit>();
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();
    public void UpdateList(Unit unit)
    {
        units.Remove(unit);
    }
    public void UpdateEnemyList(Enemy enemy)
    {
        enemies.Remove(enemy);
    }
    public void ClearList()
    {
        units.Clear();
    }
    public void ClearEnemyList()
    {
        enemies.Clear();
    }
    public void Heal()
    {
        if (eParent == EParent.Unit)
        {
            foreach (var item in units)
            {
                item.IncreaseHp(unit.AttackPower);
            }
            if (player != null)
            {
                player.IncreaseHp(unit.AttackPower);
            }
        }
        else
        {
            foreach (var item in enemies)
            {
                item.IncreaseHp(enemy.AttackPower);
            }
            if (boss != null)
            {
                boss.IncreaseHp(enemy.AttackPower);
            }
        }
    }
    private void Awake()
    {
        if(eParent == EParent.Unit)
        {
            unit = GetComponent<Unit>();
        }
        else
        {
            enemy = GetComponent<Enemy>();
        }
        
    }
    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(eParent == EParent.Unit)
        {
            if (unit.UnitState == EUnitState.Battle)
            {
                if (collision.tag == "UNIT")
                {
                    if (units.Contains(collision.GetComponent<Unit>()))
                    {
                        return;
                    }
                    units.Add(collision.GetComponent<Unit>());
                }
                else if(collision.tag == "PLAYER")
                {
                    player = collision.GetComponent<Player>();
                }
            }
        }
        else
        {
            if (enemy.EnemyState == EUnitState.Battle)
            {
                if (collision.tag == "ENEMY")
                {
                    if (enemies.Contains(collision.GetComponent<Enemy>()))
                    {
                        return;
                    }
                    enemies.Add(collision.GetComponent<Enemy>());
                }
                else if(collision.tag == "BOSS")
                {
                    boss = collision.GetComponent<Boss>();
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(eParent == EParent.Unit)
        {
            if (collision.tag == "UNIT")
            {
                if (units.Contains(collision.GetComponent<Unit>()))
                {
                    units.Remove(collision.GetComponent<Unit>());
                }
            }
            else if(collision.tag == "PLAYER")
            {
                player = null;
            }
        }
        else
        {
            if (collision.tag == "ENEMY")
            {
                if (enemies.Contains(collision.GetComponent<Enemy>()))
                {
                    enemies.Remove(collision.GetComponent<Enemy>());
                }
            }
            else if (collision.tag == "BOSS")
            {
                boss = null;
            }
        }        
    }
}
