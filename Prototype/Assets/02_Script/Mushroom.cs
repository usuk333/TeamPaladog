using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Mushroom : MonoBehaviour
{
    //for test
    private bool isButtonDown;
    private Vector3 summonPos = new Vector3(0, -0.33f);
    private int currentSlimeCount;
    private int requiredSlimeCount;
    private bool isActiveBuff;
    [SerializeField] private float buffDuration;
    [SerializeField] private int maxSlimeCount;
    [SerializeField] private Text slimeCountText;
    [SerializeField] private Transform slimePool;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private Transform typhoon;// 태풍 오브젝트 레퍼런스
    [SerializeField] private float typhoonInterval;// 태풍 패턴 간격
    [SerializeField] private float slimeSummonInterval;
    [SerializeField] private List<Slime> slimeList = new List<Slime>(); // 필드에 존재하는 슬라임 리스트
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
        Slime slime = GetSlime();
        slime.gameObject.SetActive(true);
        slime.transform.localPosition = summonPos;
        if (isActiveBuff)
        {
            slime.isInvincible = false;
        }
    }
    public void ReturnSlime(Slime slime)
    {
        slime.gameObject.SetActive(false);
        slime.transform.position = transform.position;
        slime.transform.SetParent(slimePool);
        slimeQueue.Enqueue(slime);
        slimeList.Remove(slime);
        currentSlimeCount--;
        slimeCountText.text = currentSlimeCount.ToString();
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
        currentSlimeCount++;
        slimeCountText.text = currentSlimeCount.ToString();        
        return slime;
    }
    private IEnumerator Co_Typhoon()
    {
        float typhoonSpeed = 4f;
        Vector3 typhoonDefalutPos = new Vector3(3f, 0);
        while (true)
        {
            typhoon.localPosition = typhoonDefalutPos;
            yield return new WaitForSeconds(typhoonInterval);
            typhoon.DOMoveX(-9f, typhoonSpeed);
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
    }
}
