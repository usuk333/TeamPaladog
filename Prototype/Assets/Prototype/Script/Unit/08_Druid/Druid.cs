using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Druid : MonoBehaviour
{
    public enum EDruidState
    {
        Normal,
        Wolf,
        Tiger
    }
    [SerializeField] private EDruidState druidState;

    [SerializeField] private float maxTransmogValue;
    [SerializeField] private float currentTransmogValue;
    [SerializeField] private List<GameObject> collisions = new List<GameObject>();
    private bool isTransmog = false;
    private Unit unit;
    private float attackPower;
    private float attackSpeed;
    [SerializeField] private float tigerPower;
    [SerializeField] private float tigerSpeed;
    private SpriteRenderer sprite;
    private Color originColor;

    public bool IsTransmog { get => isTransmog; set => isTransmog = value; }
    public List<GameObject> Collisions { get => collisions; set => collisions = value; }
    public float MaxTransmogValue { get => maxTransmogValue; }
    public EDruidState DruidState { get => druidState; set => druidState = value; }
    private void Awake()
    {
        unit = GetComponent<Unit>();
        attackPower = unit.AttackPower;
        attackSpeed = unit.AttackSpeed;
        sprite = GetComponent<SpriteRenderer>();
        originColor = sprite.color;// 추후 스프라이트 교체로 변경
    }
    private void Start()
    {

    }
    private void OnEnable()
    {
        StartCoroutine(Co_UpdateTrasnmogValue());
    }
    private IEnumerator Co_UpdateTrasnmogValue()
    {
        while (true)
        {
            if(druidState == EDruidState.Normal)
            {
                currentTransmogValue += Time.deltaTime;
                if(currentTransmogValue >= maxTransmogValue)
                {
                    Transmog();
                    yield return new WaitForSeconds(1f);
                    isTransmog = true;
                }
            }
            else
            {
                currentTransmogValue -= Time.deltaTime;
                if(currentTransmogValue <= 0)
                {
                    currentTransmogValue = 0;
                    druidState = EDruidState.Normal;
                    ResetPowerAndSpeed();
                    sprite.color = originColor; // 추후 스프라이트 교체로 변경
                    isTransmog = false;
                }
            }
            yield return null;
        }
    }
    private void Transmog()
    {
        int i = Random.Range(0, 2);
        if (i <= 0)
        {
            TransmogWolf();
        }
        else
        {
            TransmogTiger();
        }   
    }
    public void TransmogWolf()
    {
        sprite.color = Color.gray;// 추후 스프라이트 교체로 변경
        druidState = EDruidState.Wolf;     
    }
    public void TransmogTiger()
    {
        sprite.color = Color.magenta;// 추후 스프라이트 교체로 변경
        druidState = EDruidState.Tiger;
        AttackTiger();
    }
    public void AttackWolf()
    {
        int count = 0;        
        for (int i = 0; i < collisions.Count; i++)
        {
            if(count > 2)
            {
                break;
            }
            if (collisions[i] == null)
            {
                continue;
            }
            if (collisions[i].GetComponent<Enemy>())
            {
                collisions[i].GetComponent<Enemy>().DecreaseHp(attackPower);
            }
            else if (collisions[i].GetComponent<Boss>())
            {
                collisions[i].GetComponent<Boss>().DecreaseHp(attackPower);
            }
            count++;
        }
    }
    private void AttackTiger()
    {
        unit.AttackPower = attackPower + tigerPower;
        unit.AttackSpeed = attackSpeed - tigerSpeed;
    }
    private void ResetPowerAndSpeed()
    {
        unit.AttackPower = attackPower;
        unit.AttackSpeed = attackSpeed;
    }
    public void ResetTransmog()
    {
        currentTransmogValue = 0;
        GetComponentInChildren<MpBar>().DecreaseMpBar();
        collisions.Clear();
    }
}
