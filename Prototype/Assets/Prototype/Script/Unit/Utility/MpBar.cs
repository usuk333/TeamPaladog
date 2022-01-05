using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MpBar : MonoBehaviour
{
    private enum EUnitValue
    {
        Summoner,
        Druid
    }
    [SerializeField] private EUnitValue eUnitValue;
    private Camera uiCamera;
    private Canvas canvas;
    private RectTransform rectParent;
    private RectTransform rectMp;

    private Vector3 offset = new Vector3(0, 0.8f, 0);
    [SerializeField] private Transform objTr;
    private Image mpBarImage;
    private Druid druid;
    private Summoner summoner;


    // Start is called before the first frame update
    void Start()
    {
        if(eUnitValue == EUnitValue.Druid)
        {
            druid = transform.parent.parent.GetComponent<Druid>();
        }
        else
        {
            summoner = transform.parent.parent.GetComponent<Summoner>();
        }
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectMp = GetComponent<RectTransform>();
        mpBarImage = GetComponent<Image>();
        objTr = transform.parent.parent.GetComponent<Transform>();
    }
    private void OnEnable()
    {
        StartCoroutine(Co_UpdateMpBar());
    }
    // Update is called once per frame
    void LateUpdate()
    {
        var screenPos = Camera.main.WorldToScreenPoint(objTr.position + offset);
        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);
        rectMp.localPosition = localPos;
    }
    private IEnumerator Co_UpdateMpBar()
    {
        while (true)
        {
            if (druid)
            {
                if (!druid.IsTransmog)
                {
                    mpBarImage.fillAmount += (1 / druid.MaxTransmogValue) * Time.deltaTime;
                }
                else
                {
                    mpBarImage.fillAmount -= (1 / druid.MaxTransmogValue) * Time.deltaTime;
                }
            }
            else if (summoner)
            {
                if(mpBarImage.fillAmount < 1)
                {
                    mpBarImage.fillAmount += (1 / summoner.AttakSpeed) * Time.deltaTime;
                }               
            }
            
            yield return null;
        }
    }
    public void DecreaseMpBar()
    {
        if(mpBarImage != null)
        {
            mpBarImage.fillAmount = 0;
        }
    }
}
