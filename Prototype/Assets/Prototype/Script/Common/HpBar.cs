using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    private Camera uiCamera;
    private Canvas canvas;
    private RectTransform rectParent;
    private RectTransform rectHp;

    private Vector3 offset = Vector3.up;
    [SerializeField] private Transform objTr;
    private Image hpBar;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = GetComponent<RectTransform>();
        hpBar = GetComponent<Image>();
        objTr = transform.parent.parent.GetComponent<Transform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var screenPos = Camera.main.WorldToScreenPoint(objTr.position + offset);
        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);
        rectHp.localPosition = localPos;
    }
    public void DecreaseUnitOrEnemyHpUI(bool isUnit)
    {
        if (isUnit)
        {
            hpBar.fillAmount = transform.parent.parent.GetComponent<Unit>().CurrentHp * (1/ transform.parent.parent.GetComponent<Unit>().MaxHp);
        }
        else
        {
            hpBar.fillAmount = transform.parent.parent.GetComponent<Enemy>().CurrentHp * (1/transform.parent.parent.GetComponent<Enemy>().MaxHp);
        }
    }
}
