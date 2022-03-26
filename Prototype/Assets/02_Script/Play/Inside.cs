using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inside : MonoBehaviour
{
    private bool isInsideReady;
    private bool isInside;
    private int explosionCount = 0;
    private Boss dummyBoss;
    private Vector3 insidePlayerPos = new Vector3(-5.6f,-9.17f);
    private Vector3 originPlayerPos = new Vector3(-5.6f, 0f);
    private Vector3 insideCameraPos = new Vector3(0, -9.17f, -10f);
    private Vector3 originCameraPos = new Vector3(0, 0, -10f);
    [SerializeField] private Transform originHpBar;
    [SerializeField] private Transform insideHpBar;
    [SerializeField] private Transform insideInstance;
    [SerializeField] private Transform insidePortal;
    [SerializeField] private float insideDuration;
    [SerializeField] private float moveToInsideDuration;
    [SerializeField] private Transform insideMoveObj;
    [SerializeField] private Transform manaExplosion;
    [SerializeField] private float manaExplosionDuration;
    [SerializeField] private float decreaseMpPercent;
    [SerializeField] private Image sceneMoveImg;
    private int[] insidePercentage = { 80, 40 };
    private Color originColor;
    public void MoveToInside()
    {
        Debug.Log("내면으로 이동");
        isInside = true;
        InGameManager.Instance.Player.transform.position = insidePlayerPos;
        Camera.main.transform.position = insideCameraPos;
    }
    private void MoveToOrigin()
    {
        Debug.Log("현실로 이동");
        isInside = false;
        InGameManager.Instance.Player.transform.position = originPlayerPos;
        Camera.main.transform.position = originCameraPos;
    }
    private void ManaExplosion()
    {
        if (InGameManager.Instance.Boss.CollisionsArray[0].Contains(InGameManager.Instance.Player.gameObject))
        {
            InGameManager.Instance.Player.DecreaseMp(InGameManager.Instance.Player.MaxMp * decreaseMpPercent / 100);
            return;
        }
        foreach (var item in InGameManager.Instance.Units)
        {
            if (InGameManager.Instance.Boss.CollisionsArray[0].Contains(item.gameObject))
            {
                item.CommonStatus.DecreaseHp(item.CommonStatus.MaxHp);
                explosionCount++;
            }
        }
    }
    private IEnumerator Co_InsidePattern()
    {
        Text moveInsideTimertext = insideMoveObj.GetComponentInChildren<Text>();
        float insideTimer = insideDuration;
        float moveInsideTimer = moveToInsideDuration;
        int index = 0;
        while (true)
        {
            yield return new WaitUntil(() => InGameManager.Instance.Boss.CommonStatus.CurrentHp <= InGameManager.Instance.Boss.CommonStatus.MaxHp * insidePercentage[index] / 100);
            GetComponent<SpriteRenderer>().color = Color.clear;
            isInsideReady = true;
            InGameManager.Instance.StopAllUnitCoroutines();
            originHpBar.gameObject.SetActive(false);
            insidePortal.gameObject.SetActive(true);
            insideMoveObj.gameObject.SetActive(true);
            insideInstance.gameObject.SetActive(true);
            while (moveInsideTimer > 0)
            {
                moveInsideTimer -= Time.deltaTime;
                moveInsideTimertext.text = string.Format("보스의 내면으로 이동해야 합니다! 남은 시간 : {0:0}초", System.Math.Ceiling(moveInsideTimer));
                if (isInside)
                {
                    moveInsideTimer = moveToInsideDuration;
                    insideMoveObj.gameObject.SetActive(false);
                    insideHpBar.gameObject.SetActive(true);
                    sceneMoveImg.gameObject.SetActive(true);
                    yield return new WaitForSeconds(2f);
                    sceneMoveImg.gameObject.SetActive(false);
                    break;
                }
                yield return null;
            }
            if (!isInside)
            {
                Debug.Log("플레이어 사망");
                yield break;
            }
            //InGameManager.Instance.StartAllUnitCoroutines();
            yield return new WaitUntil(() => dummyBoss.CommonStatus.CurrentHp <= 0);
            Debug.Log("현실로 돌아갑니다");
            index++;
            sceneMoveImg.gameObject.SetActive(true);
            MoveToOrigin();
            GetComponent<SpriteRenderer>().color = originColor;
            InGameManager.Instance.StartAllUnitCoroutines();
            originHpBar.gameObject.SetActive(true);
            insideInstance.gameObject.SetActive(false);
            yield return new WaitForSeconds(2f);
            sceneMoveImg.gameObject.SetActive(false);       
            //Debug.Log("내면으로 들어왔다.");
        }      
    }
    private IEnumerator Co_ManaExplosion()
    {
        while (true)
        {
            manaExplosion.gameObject.SetActive(false);
            if (isInsideReady)
            {
                yield return null;
                continue;
            }
            if (explosionCount < 4)
            {
                int rand = Random.Range(5, 10);
                yield return new WaitForSeconds(rand);
                manaExplosion.gameObject.SetActive(true);
                int index;
                while (true)
                {
                    index = Random.Range(0, InGameManager.Instance.Units.Length);
                    if (InGameManager.Instance.Units[index].CommonStatus.CurrentHp > 0)
                    {
                        break;
                    }
                    yield return null;
                }
                manaExplosion.position = InGameManager.Instance.Units[index].transform.position;
                yield return new WaitForSeconds(manaExplosionDuration);
                ManaExplosion();
            }
            else
            {
                manaExplosion.position = InGameManager.Instance.Player.transform.position;
                yield return new WaitForSeconds(manaExplosionDuration);
                ManaExplosion();
            }
        }
    }
    private void Awake()
    {
        originColor = GetComponent<SpriteRenderer>().color;
        dummyBoss = transform.Find("InsideInstance").GetComponentInChildren<Boss>();
    }
    private void Start()
    {
        StartCoroutine(Co_ManaExplosion());
        StartCoroutine(Co_InsidePattern());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            dummyBoss.CommonStatus.DecreaseHp(10);
        }
    }
}
