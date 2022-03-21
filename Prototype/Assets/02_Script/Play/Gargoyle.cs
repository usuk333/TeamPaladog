using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Gargoyle : MonoBehaviour
{
    private bool isShout;
    [SerializeField] private GameObject veneer;
    [SerializeField] private float veneerDuration;
    [SerializeField] private float dotInterval;
    [SerializeField] private float dotDamage;
    [SerializeField] private float slowDownValue;
    [SerializeField] private BossUtility bossUtility;
    [SerializeField] private Transform trackingObj;
    [SerializeField] private float trackingInterval;
    [SerializeField] private float trackingDelay;
    [SerializeField] private float trackingDamage;
    [SerializeField] private float shieldValue;
    [SerializeField] private float shieldDuration;
    [SerializeField] private GameObject shieldObj;
    [SerializeField] private float laserDelay;
    [SerializeField] private float laserDamage;
    [SerializeField] private Transform laserObj;
    [SerializeField] private Transform secondLaserObj;

    private void Shouting()
    {
        bossUtility.KnockBack();
        isShout = true;
    }
    private void ResetVeneer()
    {
        veneer.SetActive(false);
        isShout = false;
        foreach (var item in InGameManager.Instance.Units)
        {
            item.CommonStatus.SlowDown(true);
            InGameManager.Instance.Player.SlowDown(true);
        }
    }
    private void AttackVeneer()
    {
        if (InGameManager.Instance.Boss.CollisionsArray[0].Contains(InGameManager.Instance.Player.gameObject))
        {
            InGameManager.Instance.Player.DecreaseHp(dotDamage);
            InGameManager.Instance.Player.SlowDown(false, slowDownValue);
            return;
        }
        InGameManager.Instance.Player.SlowDown(true);
        foreach (var item in InGameManager.Instance.Units)
        {
            if (InGameManager.Instance.Boss.CollisionsArray[0].Contains(item.gameObject))
            {
                item.CommonStatus.DecreaseHp(dotDamage);
                item.CommonStatus.SlowDown(false, slowDownValue);
            }
        }
    }
    private void AttackUnit(float damage)
    {
        foreach (var item in InGameManager.Instance.Units)
        {
            if(item != null)
            {
                item.CommonStatus.DecreaseHp(damage);
            }
        }
    }
    private bool AttackPlayer(int index, float damage)
    {
        if (InGameManager.Instance.Boss.CollisionsArray[index].Contains(InGameManager.Instance.Player.gameObject))
        {
            InGameManager.Instance.Player.DecreaseHp(damage);
            return true;
        }
        return false;
    }
    private IEnumerator Co_Tracking()
    {
        Vector3 trackingOffset = Vector3.up + Vector3.up;
        float timer = 0f;
        GameObject obj = trackingObj.GetChild(0).gameObject;
        while (true)
        {
            yield return null;
            trackingObj.position = InGameManager.Instance.Player.transform.position + trackingOffset;
            timer += Time.deltaTime;
            if(timer >= trackingInterval)
            {
                UpdateObjToDG(obj, true, -1.5f, 2, trackingDelay);
                yield return new WaitForSeconds(trackingDelay);
                AttackPlayer(1,trackingDamage);
                UpdateObjToDG(obj, false, -0.5f, 0);
                timer = 0f;
            }
        }
    }
    private void UpdateObjToDG(GameObject obj, bool isActive, float move, float scale, float delay = 0)
    {
        obj.SetActive(isActive);
        obj.transform.DOMoveY(move, delay);
        obj.transform.DOScaleY(scale, delay);
    }
    private IEnumerator Co_ActiveVeneer()
    {
        while (true)
        {
            yield return null;
            if (isShout)
            {
                InGameManager.Instance.Boss.isPattern = true;
                yield return new WaitForSeconds(2f);
                InGameManager.Instance.Boss.isPattern = false;
                veneer.SetActive(true);
                while (veneer.activeSelf)
                {
                    AttackVeneer();
                    yield return new WaitForSeconds(dotInterval);
                }
            }
        }
    }
    private IEnumerator Co_RemoveVeneer()
    {
        bool isCast = false;
        while (true)
        {
            if (veneer.activeSelf)
            {
                if (InGameManager.Instance.Boss.CollisionsArray[0].Contains(InGameManager.Instance.Player.gameObject) && !isCast)
                {
                    InGameManager.Instance.Player.Casting(veneerDuration);
                    isCast = true;
                }
                else if (!InGameManager.Instance.Boss.CollisionsArray[0].Contains(InGameManager.Instance.Player.gameObject))
                {
                    InGameManager.Instance.Player.isCast = false;
                    isCast = false;
                }
                if (InGameManager.Instance.Player.isCastFinish)
                {
                    ResetVeneer();
                }
            }
            yield return null;
        }
    }
    private IEnumerator Co_Shield()
    {
        Text text = shieldObj.GetComponentInChildren<Text>();
        Image image = shieldObj.transform.GetChild(0).GetComponent<Image>();
        float timer = shieldDuration;
        yield return new WaitUntil(() => InGameManager.Instance.Boss.CommonStatus.CurrentHp <= InGameManager.Instance.Boss.CommonStatus.MaxHp * 90 / 100);
        shieldObj.SetActive(true);
        InGameManager.Instance.Boss.CommonStatus.Shield = shieldValue;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            text.text = string.Format("�ǵ� ���� �ð� : {0:0}��", System.Math.Ceiling(timer));
            image.fillAmount = 1 / shieldValue * InGameManager.Instance.Boss.CommonStatus.Shield;
            if (InGameManager.Instance.Boss.CommonStatus.Shield <= 0)
            {
                InGameManager.Instance.Boss.CommonStatus.Shield = 0;
                break;
            }
            yield return null;
        }
        shieldObj.SetActive(false);
        if (InGameManager.Instance.Boss.CommonStatus.Shield > 0)
        {
            foreach (var item in InGameManager.Instance.Units)
            {
                item.CommonStatus.DecreaseHp(item.CommonStatus.MaxHp);
            }
            InGameManager.Instance.Player.DecreaseHp(InGameManager.Instance.Player.MaxHp);
        }
    }
    private IEnumerator Co_Laser()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Q));
            laserObj.gameObject.SetActive(true);
            Vector3 target = new Vector3(InGameManager.Instance.Player.transform.position.x, laserObj.position.y);
            laserObj.position = target;
            yield return new WaitForSeconds(laserDelay);
            bool isAvoid = AttackPlayer(2, laserDamage);
            float reinforceLaser = 0;
            if (isAvoid)
            {
                reinforceLaser = Mathf.Pow(laserDamage, 2);
            }
            else
            {
                reinforceLaser = laserDamage;
            }
            laserObj.gameObject.SetActive(false);
            secondLaserObj.gameObject.SetActive(true);
            secondLaserObj.DOScaleX(1.6f, laserDelay);
            yield return new WaitForSeconds(laserDelay);
            AttackUnit(reinforceLaser);
            secondLaserObj.gameObject.SetActive(false);
            secondLaserObj.DOScaleX(0, 0);
        }
    }
    private void Start()
    {
        StartCoroutine(Co_ActiveVeneer());
        StartCoroutine(Co_RemoveVeneer());
        StartCoroutine(Co_Tracking());
        StartCoroutine(Co_Shield());
        StartCoroutine(Co_Laser());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Shouting();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            secondLaserObj.DOScaleX(1.6f, 3f);
        }

    }
}