using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Spine.Unity;

public class Gargoyle : MonoBehaviour
{
    private bool isShout;
    private bool isLaser;
    private bool laserOn;
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
    [SerializeField] private SpriteRenderer secondLaserObj;
    private SkeletonAnimation skeletonAnimation;
    [SerializeField] private ParticleSystem trackingEffect;
    [SerializeField] private SpriteRenderer stone;
    [SerializeField] private ParticleSystem laserEffect;
    [SerializeField] private SpriteRenderer[] shieldSprite;
    [SerializeField] private ParticleSystem[] veneerEffect;
    [SerializeField] private ParticleSystem earthquakeEffect;
    private bool isLaserOn = false;
    private bool isTracking = false;
    Vector3 trackingOffset = new Vector3(0, 2.5f, 0);
    private void Shouting()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, "earthquake", false);
        isShout = true;
    }
    private void ResetVeneer()
    {
        veneer.SetActive(false);
        isShout = false;
        InGameManager.Instance.Player.isCastFinish = false;
        InGameManager.Instance.Player.isCast = false;
        foreach (var item in InGameManager.Instance.Units)
        {
            item.CommonStatus.SlowDown(true);
        }
    }
    private void AttackVeneer()
    {
        if (InGameManager.Instance.Boss.CollisionsArray[0].Contains(InGameManager.Instance.Player.gameObject))
        {
            InGameManager.Instance.Player.DecreaseHp(dotDamage);
            foreach (var item in InGameManager.Instance.Units)
            {
                item.CommonStatus.SlowDown(true);
            }
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
    private void AttackTracking(int index, float damage)
    {
        foreach (var item in InGameManager.Instance.Units)
        {
            if (InGameManager.Instance.Boss.CollisionsArray[index].Contains(item.gameObject))
            {
                item.CommonStatus.DecreaseHp(damage);
            }
        }
        if (InGameManager.Instance.Boss.CollisionsArray[index].Contains(InGameManager.Instance.Player.gameObject))
        {
            InGameManager.Instance.Player.DecreaseHp(damage);
        }
    }
    private bool AttackPlayer(int index, float damage, bool laser)
    {
        if (laser)
        {
            print(InGameManager.Instance.Boss.CollisionsArray[index].Contains(gameObject));
            return InGameManager.Instance.Boss.CollisionsArray[index].Contains(gameObject);
        }
        if (InGameManager.Instance.Boss.CollisionsArray[index].Contains(InGameManager.Instance.Player.gameObject))
        {
            InGameManager.Instance.Player.DecreaseHp(damage);
            return true;
        }
        return false;
    }
    private IEnumerator Co_Tracking()
    {
        // -6.144 7.316544
        float timer = 0f;
        SpriteRenderer obj = trackingObj.GetChild(0).GetComponent<SpriteRenderer>();
        SpriteRenderer sprite = trackingObj.GetComponent<SpriteRenderer>();
        while (true)
        {
            yield return null;
            timer += Time.deltaTime;
            if(timer >= trackingInterval)
            {
                sprite.DOColor(new Color(1, 0.55f, 0.55f), 2f);
                yield return new WaitForSeconds(2f);
                obj.enabled = true;
                isTracking = true;
                obj.transform.DOScaleY(7.2598f, 2f);
                obj.transform.DOLocalMoveY(-6.03f, 2f);
                yield return new WaitForSeconds(2f);
                obj.enabled = false;
                obj.transform.DOScaleY(0.2f, 0f);
                obj.transform.DOLocalMoveY(-2.5f, 0f);
                trackingEffect.Play();
                AttackTracking(1, trackingDamage);
                yield return new WaitForSeconds(0.7f);
                sprite.color = Color.white;
                timer = 0f;
                isTracking = false;
            }
        }
    }
    private IEnumerator Co_ActiveVeneer()
    {
        while (true)
        {
            yield return null;
            if (isShout)
            {
                InGameManager.Instance.Boss.isPattern = true;
                earthquakeEffect.Play();
                yield return new WaitForSeconds(2.5f);
                bossUtility.KnockBack();
                yield return new WaitForSeconds(1.5f);
                InGameManager.Instance.Boss.isPattern = false;
                veneer.SetActive(true);
                print(veneer.activeSelf);
                print(InGameManager.Instance.Player.isCastFinish);
                foreach (var item in veneerEffect)
                {
                    item.gameObject.SetActive(true);
                    item.Play();
                }
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
                    foreach (var item in veneerEffect)
                    {
                        item.Pause();
                        item.gameObject.SetActive(false);
                    }
                    isCast = false;
                }
            }
            yield return null;
        }
    }
    private IEnumerator Co_Shield()
    {
        Text text = shieldObj.GetComponentInChildren<Text>();
        Image image = shieldObj.transform.GetChild(0).GetComponent<Image>();
        print(image.gameObject.name);
        float timer = shieldDuration;
        yield return new WaitUntil(() => InGameManager.Instance.Boss.CommonStatus.CurrentHp <= InGameManager.Instance.Boss.CommonStatus.MaxHp * 90 / 100);
        InGameManager.Instance.Boss.isPattern = true;
        skeletonAnimation.AnimationState.SetAnimation(1, "shield", false);
        yield return new WaitForSeconds(1.333f);
        if (!isLaser)
        {
            InGameManager.Instance.Boss.isPattern = false;
        }
        shieldObj.SetActive(true);
        InGameManager.Instance.Boss.CommonStatus.Shield = shieldValue;
        foreach (var item in shieldSprite)
        {
            item.enabled = true;
        }
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            text.text = string.Format("실드 제한 시간 : {0:0}초", System.Math.Ceiling(timer));
            image.fillAmount = 1 / shieldValue * InGameManager.Instance.Boss.CommonStatus.CurrentShield;
            print(1 / shieldValue * InGameManager.Instance.Boss.CommonStatus.CurrentShield);
            if (InGameManager.Instance.Boss.CommonStatus.Shield <= 0)
            {
                InGameManager.Instance.Boss.CommonStatus.Shield = 0;
                break;
            }
            yield return null;
        }
        shieldObj.SetActive(false);
        foreach (var item in shieldSprite)
        {
            item.enabled = false;
        }
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
        var sprite = laserObj.GetComponent<SpriteRenderer>();
        while (true)
        {
            //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Alpha2));
            yield return new WaitUntil(() => laserOn);
            isLaser = true;
            InGameManager.Instance.Boss.isPattern = true;
            skeletonAnimation.AnimationState.SetAnimation(0, "Idle-2", false);
            laserObj.gameObject.SetActive(true);
            isLaserOn = true;
            stone.enabled = true;
            yield return new WaitForSeconds(laserDelay);
            sprite.enabled = false;
            isLaserOn = false;
            yield return new WaitForSeconds(0.2f);
            stone.transform.DOLocalMoveY(2.5f, 0.3f);
            yield return new WaitForSeconds(0.8f);
            stone.DOColor(Color.clear, 0.3f);
            yield return new WaitForSeconds(0.3f);
            stone.enabled = false;
            stone.color = Color.white;
            stone.transform.localPosition = new Vector3(0, 13); 
            bool isAvoid = AttackPlayer(2, laserDamage, true);
            laserObj.gameObject.SetActive(false);
            sprite.enabled = true;
            if (isAvoid)
            {
                print("보스 스턴");
                skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                yield return new WaitForSeconds(2f);
                InGameManager.Instance.Boss.isPattern = false;
            }
            else
            {
                print("레이저");
                skeletonAnimation.AnimationState.SetAnimation(0, "breath", false);
                laserEffect.Play();
                secondLaserObj.enabled = true;
                yield return new WaitForSeconds(1f);
                secondLaserObj.enabled = false;
                AttackUnit(laserDamage);
                yield return new WaitForSeconds(3.5f);
           
                InGameManager.Instance.Boss.isPattern = false;
            }
            isLaser = false;
            laserOn = false;
        }
    }
    private void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }
    private void Start()
    {
        StartCoroutine(Co_ActiveVeneer());
        StartCoroutine(Co_RemoveVeneer());
        StartCoroutine(Co_Tracking());
        StartCoroutine(Co_Shield());
        StartCoroutine(Co_Laser());
        laserObj.SetParent(null);
    }
    private void LateUpdate()
    {
        if (isLaserOn)
        {
            laserObj.position = new Vector3(InGameManager.Instance.Player.transform.position.x, laserObj.position.y);
            stone.transform.position = new Vector3(laserObj.position.x, stone.transform.position.y);
        }
        if (!isTracking)
        {
            trackingObj.position = InGameManager.Instance.Player.transform.position + trackingOffset;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Shouting();
        }
    }
    public void Test_BtnEvt_Shout()
    {
        Shouting();
    }
    public void Test_BtnEvt_Laser()
    {
        laserOn = true;
    }
}
