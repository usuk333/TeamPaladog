using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMark : MonoBehaviour
{
   /* [SerializeField] private Unit.EUnitType eUnitType;
    private Player player;
    [SerializeField] private CastingObject castingObject;
    private bool isMoveMark = false;
    private Vector3 markOffset = new Vector3(0, 2);
    private BoxCollider2D boxCollider2D;
    private Boss_TimeJudge boss_TimeJudge;
    private Boss boss;
    public Unit.EUnitType MarkType { get => eUnitType; }
    public void InitMark()
    {
        isMoveMark = false;
        boxCollider2D.enabled = true;
        transform.SetParent(boss_TimeJudge.transform);
    }
    private void MoveMark(Transform unit)
    {
        isMoveMark = true;
        boxCollider2D.enabled = false;
        transform.SetParent(unit);
        transform.position = new Vector3(unit.position.x, transform.position.y);
        for (int i = 0; i < boss_TimeJudge.Units.Length; i++)
        {
            if(unit.gameObject == boss_TimeJudge.Units[i].gameObject)
            {
                boss_TimeJudge.UnitMarks[i] = this;
            }
        }
    }
    private void Awake()
    {
        boss = FindObjectOfType<Boss>();
        player = FindObjectOfType<Player>(); //���߿� �ð� ���� ������ player �ν��Ͻ��� �����ϵ��� ��������.
        boxCollider2D = GetComponent<BoxCollider2D>();
        boss_TimeJudge = GetComponentInParent<Boss_TimeJudge>();
    }
    private void LateUpdate()
    {
        if (transform.gameObject.activeSelf)
        {
            if (!isMoveMark)
            {
                transform.position = player.transform.position + markOffset;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == castingObject.UnitTag)
        {
            if (isMoveMark || collision.GetComponentInChildren<UnitMark>())
            {
                return;
            }
            player.Casting(false, castingObject.CastTime);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == castingObject.UnitTag)
        {
            if (player.isCastFinish)
            {
                if (isMoveMark || collision.GetComponentInChildren<UnitMark>())
                {
                    return;
                }
                player.isCastFinish = false;
                MoveMark(collision.transform);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == castingObject.UnitTag)
        {
            if (isMoveMark)
            {
                return;
            }
            player.Casting(true);
        }
    }*/
}
