using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Boss_TimeJudge : MonoBehaviour
{
    private Player player;
    private Vector3 timeBombOffset = Vector3.up;
    private Camera camera;
    private Canvas canvas;
    private Camera uiCamera;
    private RectTransform rectParent;
    private Text timeBombSecText;
    private Unit[] units;
    public bool isPresent { get; set; } = true;
    [SerializeField] private float timeLimitToFutuer;
    [SerializeField] private float timeBombInvincibleSec;
    [SerializeField] private float timeBombDamage;
    [SerializeField] private float timeBombSec;
    [SerializeField] private GameObject timeBomb;
    [SerializeField] private Transform field;
    [SerializeField] private Image timeLimitImage;
    [SerializeField] private Text timeLimitText;
    [SerializeField] private float cameraWorkTime;
    [SerializeField] private Transform leftInvinsibleWall;
    [SerializeField] private Transform rightInvinsibleWall;
    private void ExplosionTimeBomb()
    {
        foreach (var item in units)
        {
            item.DecreaseHp(timeBombDamage);
            item.SetInvincibility(timeBombInvincibleSec);
        }
        player.DecreaseHp(timeBombDamage);
        player.SetInvincibility(timeBombInvincibleSec);
    }
    private void AttackLaser()
    {
        Debug.Log("레이저 발사");
        foreach (var item in units)
        {
            item.DecreaseHp(item.MaxHp);
        }
        player.DecreaseHp(player.MaxHp);
    }
    private void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        camera = Camera.main;
        player = FindObjectOfType<Player>();
        timeBombSecText = timeBomb.GetComponentInChildren<Text>();
    }
    private IEnumerator Start()
    {
        units = InGameManager.Instance.Units;
        yield return new WaitForSeconds(5f);
        StartCoroutine(Co_TimeBomb());
        StartCoroutine(Co_TimeLaser());
    }
    private IEnumerator Co_TimeLaser()
    {
        yield return new WaitForSeconds(9f);
        while (true)
        {
            AttackLaser();
            yield return new WaitForSeconds(2f);           
            MoveCameraToFuture();
            yield return new WaitForSeconds(cameraWorkTime);
            StartCoroutine(Co_FutuerTimer());      
            yield return new WaitForSeconds(timeLimitToFutuer);
            MoveCameraToPresent();
            yield return new WaitForSeconds(120f);          
        }
    }
    private IEnumerator Co_FutuerTimer()
    {
        timeLimitImage.transform.parent.gameObject.SetActive(true);
        float time = timeLimitToFutuer;
        while(time > 0)
        {
            timeLimitImage.fillAmount = 1 / timeLimitToFutuer * time;
            timeLimitText.text = string.Format("시간이동 남은 시간 : {0:0}초", System.Math.Ceiling(time));
            time -= Time.deltaTime;
            yield return null;
        }
        timeLimitImage.transform.parent.gameObject.SetActive(false);
        if (!isPresent)
        {
            player.DecreaseHp(player.MaxHp);
        }
    }
    private IEnumerator Co_TimeBomb()
    {
        timeBomb.SetActive(true);
        float time = timeBombSec;
        while(time > 0)
        {
            time -= Time.deltaTime;
            timeBombSecText.text = string.Format("{0:0}", System.Math.Ceiling(time));
            yield return null;
        }
        timeBombSecText.text = "";
        yield return new WaitForSeconds(3f);
        ExplosionTimeBomb();
    }
    private void MoveCameraToFuture()
    {
        camera.DOOrthoSize(7.2f, cameraWorkTime);
        camera.transform.DOMoveY(-2.6f, cameraWorkTime);
        field.DOMove(new Vector3(0, -2.7f, 0), cameraWorkTime);
        field.DOScaleY(1.1f, cameraWorkTime);
        timeBomb.transform.DOScale(new Vector3(0.6f, 0.6f), cameraWorkTime);
        leftInvinsibleWall.DOMove(new Vector3(-13f,leftInvinsibleWall.position.y), cameraWorkTime);
        rightInvinsibleWall.DOMove(new Vector3(13f, rightInvinsibleWall.position.y), cameraWorkTime);
    }
    private void MoveCameraToPresent()
    {
        camera.DOOrthoSize(3.6f, cameraWorkTime);
        camera.transform.DOMoveY(0, cameraWorkTime);
        field.DOMove(new Vector3(0, -2.9f, 0), cameraWorkTime);
        field.DOScaleY(1.5f, cameraWorkTime);
        timeBomb.transform.DOScale(new Vector3(1f, 1f), cameraWorkTime);
        leftInvinsibleWall.DOMove(new Vector3(-6.9f, leftInvinsibleWall.position.y), cameraWorkTime);
        rightInvinsibleWall.DOMove(new Vector3(6.9f, rightInvinsibleWall.position.y), cameraWorkTime);
    }
    void LateUpdate()
    {
        if (timeBomb.activeInHierarchy)
        {
            var screenPos = Camera.main.WorldToScreenPoint(player.transform.position + timeBombOffset);
            var localPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);
            timeBomb.transform.localPosition = localPos;
        }
    }
}
