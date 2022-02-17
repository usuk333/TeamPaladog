using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMark : MonoBehaviour
{
    [SerializeField] private Unit.EUnitType eUnitType;
    private Player player;
    [SerializeField] private CastingObject castingObject;
    private bool isMoveMark = false;
    private Vector3 markOffset = new Vector3(0, 2);
    private BoxCollider2D boxCollider2D;
    private Boss_TimeJudge boss_TimeJudge;
    private Boss boss;
    public Unit.EUnitType MarkType { get => eUnitType; }
    private void MoveMark(Transform unit)
    {
        isMoveMark = true;
        boxCollider2D.enabled = false;
        transform.SetParent(unit);
        transform.position = new Vector3(unit.position.x, transform.position.y);
        boss_TimeJudge.UnitMarks.Add(this);
    }
    private void Awake()
    {
        boss = FindObjectOfType<Boss>();
        player = FindObjectOfType<Player>(); //나중에 시간 법관 보스의 player 인스턴스를 참조하도록 변경하자.
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
            if (isMoveMark)
            {
                return;
            }
            player.Casting(castingObject.CastTime);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == castingObject.UnitTag)
        {
            if (player.isCastFinish)
            {
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
            player.isCast = false;
        }
    }
}
