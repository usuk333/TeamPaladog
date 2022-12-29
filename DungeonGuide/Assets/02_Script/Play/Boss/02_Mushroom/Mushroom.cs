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
    [SerializeField] private Transform typhoon;// ��ǳ ������Ʈ ���۷���
    [SerializeField] private List<Slime> slimeList = new List<Slime>(); // �ʵ忡 �����ϴ� ������ ����Ʈ
    [SerializeField] private ParticleSystem sporeEffect;
    private Queue<Slime> slimeQueue = new Queue<Slime>();

    [SerializeField] private Image warningImage;
    [SerializeField] private Text warningText;

    private bool breakSummon;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClipArray;

    public MushroomStatus mushroomStatus;
    //for test
    private IEnumerator Co_SetSlimeVincible()
    {
        CastingObject castingObject = altar.GetComponent<CastingObject>();
        while (true)
        {
            yield return new WaitUntil(() => castingObject.CastFinish);
            Debug.Log("���� ���� Ȱ��ȭ");
            foreach (var item in slimeList)
            {
                item.isInvincible = false;
            }
            isActiveBuff = true;
        }
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
            breakSummon = true;
            warningImage.gameObject.SetActive(true);
            warningImage.DOFade(1, 2f);
            warningText.DOFade(1, 2f);
            PlayAudio(0);
            yield return new WaitForSeconds(2f);
            warningImage.DOFade(0, 2f);
            warningText.DOFade(0, 2f);
            altar.SetActive(true);
            altar.GetComponent<SpriteRenderer>().DOFade(1, 2f);
            yield return new WaitForSeconds(2f);
            warningImage.gameObject.SetActive(false);
            isCounting = true;
            sporeTimerText.transform.parent.gameObject.SetActive(true);
            StartCoroutine(Co_CheckInvincible());
            while (time > 0f)
            {
                time -= Time.deltaTime;
                progress = time;
                sporeTimerText.text = string.Format("������ ���� ���� �ð� : {0:0}��", System.Math.Ceiling(time));
                yield return null;
            }
            sporeTimerText.transform.parent.gameObject.SetActive(false);
            altar.GetComponent<SpriteRenderer>().DOFade(0, 1f);
            yield return new WaitForSeconds(1f);
            altar.SetActive(false);
            if (slimeList.Count <= 0)
            {
                breakSummon = false;
                isCounting = false;
            }
            else
            {
                slimeList.Clear();
                isCounting = false;
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
            }
            yield return null;
        }
    }
    private IEnumerator Co_CheckInvincible()
    {
        InGameManager.Instance.Boss.CommonStatus.IsInvincibility = true;
        while (altar.activeSelf)
        {
            yield return null;
            if(slimeList.Count <= 0)
            {
                InGameManager.Instance.Boss.CommonStatus.IsInvincibility = false;
                yield break;
            }
        }
        InGameManager.Instance.Boss.CommonStatus.IsInvincibility = false;
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
    private void InitializeObject() //����ü �ʱ�ȭ. �� ����ü ť�� ������ ����ü�� ��ť. �� ó���� ȣ��
    {
        for (int i = 0; i < 5; i++)
        {
            slimeQueue.Enqueue(CreateNewSlime());
        }
    }
    private Slime CreateNewSlime() //����ü ������Ʈ ����
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
            PlayAudio(1);
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
            if (breakSummon)
            {
                continue;
            }
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
    private void PlayAudio(int index)
    {
        audioSource.volume = SoundManager.Instance.SfxAudio.volume;
        audioSource.Stop();
        audioSource.clip = audioClipArray[index];
        audioSource.Play();
    }
}
