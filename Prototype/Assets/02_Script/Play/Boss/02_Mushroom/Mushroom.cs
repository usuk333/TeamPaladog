using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Mushroom : MonoBehaviour
{
    private Vector3 summonPos = new Vector3(0, -0.5f);
    private bool isActiveBuff;
    public bool isCounting { get; set; }
    public float SporeTimerMaxValue { get => sporeTimerMaxValue; set => sporeTimerMaxValue = value; }

    public float progress;
    public delegate void ReturnAllSlime();
    public ReturnAllSlime dReturnAllSlime;
    [SerializeField] private GameObject altar;
    [SerializeField] private float sporeInterval;
    [SerializeField] private int maxSlimeCount;
    [SerializeField] private float sporeTimerMaxValue;
    [SerializeField] private Text slimeCountText;
    [SerializeField] private Text sporeTimerText;
    [SerializeField] private Transform slimePool;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private Transform typhoon;// 태풍 오브젝트 레퍼런스
    [SerializeField] private List<Slime> slimeList = new List<Slime>(); // 필드에 존재하는 슬라임 리스트
    [SerializeField] private ParticleSystem sporeEffect;
    private Queue<Slime> slimeQueue = new Queue<Slime>();

    [SerializeField] private Image warningImage;
    [SerializeField] private Text warningText;

    public MushroomStatus mushroomStatus;
    //for test
    private IEnumerator Co_SetSlimeVincible()
    {
        CastingObject castingObject = altar.GetComponent<CastingObject>();
        while (true)
        {
            yield return new WaitUntil(() => castingObject.CastFinish);
            Debug.Log("제단 버프 활성화");
            foreach (var item in slimeList)
            {
                item.isInvincible = false;
            }
            isActiveBuff = true;
        }
    }
    public void SummonSlime(Vector3 summonPos)
    {
        if (isCounting)
        {
            return;
        }
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
        slime.isInvincible = true;
        slimeQueue.Enqueue(slime);
        slimeList.Remove(slime);
        slimeCountText.text = "x" + slimeList.Count.ToString();
    }
    private void Spore()
    {
        InGameManager.Instance.Player.DecreaseHp(mushroomStatus.forthPatternDotDamage);
        foreach (var item in InGameManager.Instance.Units)
        {
            item.CommonStatus.DecreaseHp(mushroomStatus.forthPatternDotDamage);
        }
    }
    private IEnumerator Co_SporeTimer()
    {
        float time;
        while (true)
        {
            time = sporeTimerMaxValue;
            progress = sporeTimerMaxValue;
            isActiveBuff = false;
           // maxSlimeCount = Random.Range(15, 25);
            yield return new WaitUntil(() => slimeList.Count >= maxSlimeCount);
            warningImage.gameObject.SetActive(true);
            warningImage.DOFade(1, 2f);
            warningText.DOFade(1, 2f);
            yield return new WaitForSeconds(2f);
            warningImage.DOFade(0, 2f);
            warningText.DOFade(0, 2f);
            altar.SetActive(true);
            altar.GetComponent<SpriteRenderer>().DOFade(1, 2f);
            yield return new WaitForSeconds(2f);
            warningImage.gameObject.SetActive(false);
            isCounting = true;
            sporeTimerText.transform.parent.gameObject.SetActive(true);
            while (time > 0f)
            {
                time -= Time.deltaTime;
                progress = time;
                sporeTimerText.text = string.Format("슬라임 제거 남은 시간 : {0:0}초", System.Math.Ceiling(time));
                yield return null;
            }
            isCounting = false;
            sporeTimerText.transform.parent.gameObject.SetActive(false);
            altar.GetComponent<SpriteRenderer>().DOFade(0, 1f);
            yield return new WaitForSeconds(1f);
            altar.SetActive(false);
            if (slimeList.Count <= 0)
            {
                continue;
            }
            slimeList.Clear();
            dReturnAllSlime();
            sporeEffect.Play();
            InGameManager.Instance.Player.DecreaseHp(mushroomStatus.forthPatternDamage);
            foreach (var item in InGameManager.Instance.Units)
            {
                item.CommonStatus.DecreaseHp(mushroomStatus.forthPatternDamage);
            }
            for (int i = 0; i < mushroomStatus.forthPatternDotDuration; i++)
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
            yield return new WaitForSeconds(mushroomStatus.fifthPatternDuration);
            foreach (var item in slimeList)
            {
                item.isInvincible = true;
            }
            altar.GetComponent<CastingObject>().CastFinish = false;
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
        slimeCountText.text = "x" + slimeList.Count.ToString();
        return slime;
    }
    private IEnumerator Co_Typhoon()
    {
        float typhoonSpeed = 4f;
        Vector3 typhoonDefaultPos = new Vector3(45f, 5.14f);
        while (true)
        {
            typhoon.localPosition = typhoonDefaultPos;
            float rand = Random.Range(mushroomStatus.firstPatternMinTime, mushroomStatus.firstPatternMaxTime);
            yield return new WaitForSeconds(rand);
            typhoon.DOMoveX(-12f, typhoonSpeed);
            yield return new WaitForSeconds(typhoonSpeed);
        }
    }
    private IEnumerator Co_SummonSlime()
    {
        float count = mushroomStatus.secondPatternCount;
        yield return new WaitForSeconds(4f);
        SummonSlime(summonPos);
        SummonSlime(summonPos);
        while (true)
        {
            yield return new WaitForSeconds(count);
            SummonSlime(summonPos);
        }
    }
    private void Awake()
    {
        InitializeObject();
    }
    private void Start()
    {
        maxSlimeCount = (int)Random.Range(mushroomStatus.forthPatternMinCount, mushroomStatus.forthPatternMaxCount);
        sporeTimerMaxValue = Random.Range(mushroomStatus.forthPatternMinTime, mushroomStatus.forthPatternMaxTime);

        typhoon.GetComponent<CollisionObject>().Damage = mushroomStatus.firstPatternDamage;
        StartCoroutine(Co_Typhoon());
        StartCoroutine(Co_SummonSlime());
        StartCoroutine(Co_SetSlimeInvincible());
        StartCoroutine(Co_SetSlimeVincible());
        StartCoroutine(Co_SporeTimer());
    }
}
