using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HpBar : MonoBehaviour //아군, 적 유닛 Hp바 UI 갱신해주는 스크립트
{
    private Camera uiCamera;
    private Canvas canvas;
    private RectTransform rectParent;
    private RectTransform rectHp;    
    private Vector3 offset = Vector3.up;
    private Image hpBar;
    private Unit unit;
    private Enemy enemy;
    [SerializeField] private Transform objTr;
    public void UpdateUnitOrEnemyHpBar()
    {
        if (unit != null)
        {
            hpBar.fillAmount = unit.CurrentHp / unit.MaxHp;
        }
        else if(enemy != null)
        {
            hpBar.fillAmount = enemy.CurrentHp / enemy.MaxHp;
        }
    }
    public void ResetHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = 1f;
        }
    }
    private void Awake()
    {
        if (GetComponentInParent<Unit>())
        {
            unit = GetComponentInParent<Unit>();
        }
        else if(GetComponentInParent<Enemy>())
        {
            enemy = GetComponentInParent<Enemy>();
        }
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = GetComponent<RectTransform>();
        hpBar = GetComponent<Image>();
        objTr = transform.parent.parent.GetComponent<Transform>();
    }
    private void LateUpdate()
    {
        var screenPos = Camera.main.WorldToScreenPoint(objTr.position + offset);
        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);
        rectHp.localPosition = localPos;
    }
}
