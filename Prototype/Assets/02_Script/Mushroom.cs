using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Mushroom : MonoBehaviour
{
    //for test
    private bool isButtonDown;
    private Vector3 summonPos = new Vector3(0, -0.5f);
    private int requiredSlimeCount;
    private bool isActiveBuff;
    public bool isCounting { get; set; }
    public delegate void ReturnAllSlime();
    public ReturnAllSlime dReturnAllSlime;
    [SerializeField] private GameObject altar;
    [SerializeField] private float sporeCount;
    [SerializeField] private float sporeInterval;
    [SerializeField] private float sporeDamage;
    [SerializeField] private float buffDuration;
    [SerializeField] private int maxSlimeCount;
    [SerializeField] private float sporeTimerMaxValue;
    [SerializeField] private Text slimeCountText;
    [SerializeField] private Text sporeTimerText;
    [SerializeField] private Transform slimePool;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private Transform typhoon;// 태풍 오브젝트 레퍼런스
    [SerializeField] private float typhoonInterval;// 태풍 패턴 간격
    [SerializeField] private float slimeSummonInterval;
    [SerializeField] private List<Slime> slimeList = new List<Slime>(); // 필드에 존재하는 슬라임 리스트
    [SerializeField] private ParticleSystem sporeEffect;
    private Queue<Slime> slimeQueue = new Queue<Slime>();
    //for test
    public void BtnEvt_SummonSlime()
    {
        isButtonDown = true;
    }
    public void SetSlimeVincible()
    {
        Debug.Log("함수 호출");
        foreach (var item in slimeList)
        {
            item.isInvincible = false;
        }
        isActiveBuff = true;
    }
    public void SummonSlime(Vector3 summonPos)
    {
        if(slimeList.Count <= maxSlimeCount)
        {
            Slime slime = GetSlime();
            slime.gameObject.SetActive(true);
            slime.transform.localPosition = summonPos;
            if (isActiveBuff)
            {
                slime.isInvincible = false;
            }
        }
    }
    public void ReturnSlime(Slime slime)
    {
        slime.gameObject.SetActive(false);
        slime.transform.position = transform.position;
        slime.transform.SetParent(slimePool);
        slime.isInvincible = true;
        slimeQueue.Enqueue(slime);
        slimeList.Remove(slime);
        slimeCountText.text = slimeList.Count.ToString();
    }
    private void Spore()
    {
        InGameManager.Instance.Player.DecreaseHp(sporeDamage);
        foreach (var item in InGameManager.Instance.Units)
        {
            item.CommonStatus.DecreaseHp(sporeDamage);
        }
    }
    private IEnumerator Co_SporeTimer()
    {
        float time;
        while (true)
        {
            time = sporeTimerMaxValue;
            isActiveBuff = false;
           // maxSlimeCount = Random.Range(15, 25);
            yield return new WaitUntil(() => slimeList.Count >= maxSlimeCount);
            isCounting = true;
            sporeTimerText.transform.parent.gameObject.SetActive(true);
            altar.SetActive(true);
            while (time > 0f)
            {
                time -= Time.deltaTime;
                sporeTimerText.text = string.Format("슬라임 제거 남은 시간 : {0:0}초", System.Math.Ceiling(time));
                yield return null;
            }
            isCounting = false;
            sporeTimerText.transform.parent.gameObject.SetActive(false);
            altar.SetActive(false);
            if (slimeList.Count <= 0)
            {
                slimeList.Clear();
                dReturnAllSlime();
                continue;
            }
            slimeList.Clear();
            dReturnAllSlime();
            sporeEffect.Play();
            for (int i = 0; i < sporeCount; i++)
            {
                Spore();
                yield return new WaitForSeconds(sporeInterval);
            }
            yield return null;
        }
    }
    private IEnumerator Co_SetSlimeInvincible()
    {
        while (true)
        {
            yield return new WaitUntil(() => isActiveBuff);
            yield return new WaitForSeconds(buffDuration);
            foreach (var item in slimeList)
            {
                item.isInvincible = true;
            }
        }
    }
    private void InitializeObject() //투사체 초기화. 각 투사체 큐에 생성된 투사체를 인큐. 맨 처음에 호출
    {
        for (int i = 0; i < 5; i++)
        {
            slimeQueue.Enqueue(CreateNewSlime());
        }
    }
    private Slime CreateNewSlime() //투사체 오브젝트 생성
    {
        Slime slime = Instantiate(slimePrefab).GetComponent<Slime>();
        slime.gameObject.SetActive(false);
        slime.transform.SetParent(slimePool);
        return slime;
    }
    private Slime GetSlime()
    {
        Slime slime;
        slime = slimeQueue.Count > 0 ? slimeQueue.Dequeue() : CreateNewSlime();
        slimeList.Add(slime);
        slimeCountText.text = slimeList.Count.ToString();
        return slime;
    }
    private IEnumerator Co_Typhoon()
    {
        float typhoonSpeed = 4f;
        Vector3 typhoonDefaultPos = new Vector3(35f, 5.14f);
        while (true)
        {
            typhoon.localPosition = typhoonDefaultPos;
            yield return new WaitForSeconds(typhoonInterval);
            typhoon.DOMoveX(-12f, typhoonSpeed);
            yield return new WaitForSeconds(typhoonSpeed);
        }
    }
    private IEnumerator Co_SummonSlime()
    {
        while (true)
        {
            //yield return new WaitForSeconds(slimeSummonInterval);
            yield return new WaitUntil(() => isButtonDown);
            SummonSlime(summonPos);
            isButtonDown = false;
        }
    }
    private void Awake()
    {
        InitializeObject();
    }
    private void Start()
    {
        StartCoroutine(Co_Typhoon());
        StartCoroutine(Co_SummonSlime());
        StartCoroutine(Co_SetSlimeInvincible());
        StartCoroutine(Co_SporeTimer());
    }
}
