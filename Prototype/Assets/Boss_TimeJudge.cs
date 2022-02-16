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
    private IEnumerator timeBombCoroutine;
    [SerializeField] private List<GameObject> unitMarksUI = new List<GameObject>();
    [SerializeField] private List<GameObject> unitMarksObj = new List<GameObject>();
    [SerializeField] private Text unitMarksCount;
    [SerializeField] private List<UnitMark> unitMarks = new List<UnitMark>();
    [SerializeField] private bool[] isMarking;
    public bool isBombMoveFutuer { get; set; } = false;
    public bool isPresent { get; set; } = true;
    public List<UnitMark> UnitMarks { get => unitMarks; set => unitMarks = value; }
    public Unit[] Units { get => units; }

    private float timeBombStack = 1;
    [SerializeField] private Transform futuerPlayer;
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
            item.DecreaseHp(Mathf.Pow(timeBombDamage, timeBombStack));
            item.SetInvincibility(timeBombInvincibleSec);
        }
        player.DecreaseHp(Mathf.Pow(timeBombDamage, timeBombStack));
        timeBombStack++;
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
    private void ShuffleMarks()
    {
        for (int i = unitMarksUI.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i);
            ShuffleList(unitMarksUI, i, rand);
            ShuffleList(unitMarksObj, i, rand);
        }
    }
    private void ShuffleList(List<GameObject> list, int index, int rand)
    {
        GameObject temp = list[index];
        list[index] = list[rand];
        list[rand] = temp;
    }
    private void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        camera = Camera.main;
        player = FindObjectOfType<Player>();
        timeBombSecText = timeBomb.GetComponentInChildren<Text>();
        timeBombCoroutine = Co_TimeBomb();
        ShuffleMarks();
    }
    private IEnumerator Start()
    {
        units = InGameManager.Instance.Units;
        yield return new WaitForSeconds(5f);
        //StartCoroutine(timeBombCoroutine);
        //StartCoroutine(Co_TimeLaser());
        StartCoroutine(Co_Marking());
    }
    private IEnumerator Co_Marking()
    {
        for (int i = 0; i < unitMarksUI.Count; i++)
        {
            unitMarksUI[i].SetActive(true);
            unitMarksObj[i].SetActive(true);
            float time = 10;
            while(time > 0)
            {
                time -= Time.deltaTime;
                unitMarksCount.text = string.Format("{0:0}", System.Math.Ceiling(time));
                yield return null;
            }
            unitMarksUI[i].SetActive(false);
            unitMarksCount.text = "";
        }
        for (int i = 0; i < unitMarks.Count; i++)
        {
           // units[i].transform.
        }
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
            yield return new WaitForSeconds(cameraWorkTime);
            if (isBombMoveFutuer)
            {
                StopCoroutine(timeBombCoroutine);
                StartCoroutine(Co_Marking());
                timeBombStack = 1;
                yield return new WaitForSeconds(10f); //과거에서 현재로 폭탄 가져다주기까지 시간
                StartCoroutine(timeBombCoroutine = Co_TimeBomb());
                isBombMoveFutuer = false;
            }
            else
            {
                StartCoroutine(Co_Marking());
            }
            yield return new WaitForSeconds(30f);          
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
        ExplosionTimeBomb();
        yield return new WaitForSeconds(timeBombInvincibleSec);
        timeBombSec = 30f;
        yield return new WaitForSeconds(cameraWorkTime);
        StartCoroutine(timeBombCoroutine = Co_TimeBomb());
    }
    private void MoveCameraToFuture()
    {
        camera.DOOrthoSize(7.2f, cameraWorkTime);
        camera.transform.DOMoveY(-3.5f, cameraWorkTime);
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
            if (!isBombMoveFutuer)
            {
                var screenPos = Camera.main.WorldToScreenPoint(player.transform.position + timeBombOffset);
                var localPos = Vector2.zero;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);
                timeBomb.transform.localPosition = localPos;
            }
            else
            {
                var screenPos = Camera.main.WorldToScreenPoint(futuerPlayer.position + timeBombOffset);
                var localPos = Vector2.zero;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);
                timeBomb.transform.localPosition = localPos;
            }
        }
    }
}
