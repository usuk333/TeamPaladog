using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

public class Inside : MonoBehaviour
{
    [SerializeField] private List<GameObject> fallingObjectDummys = new List<GameObject>();
    [SerializeField] private List<GameObject> fallingObjects = new List<GameObject>();
    [SerializeField] private List<InsideFallingObj> castedFallingObjList = new List<InsideFallingObj>();
    private Transform fallingObjectDummysParent;
    private bool isInsideReady;
    private bool isInside;
    private int explosionCount = 0;
    private Boss dummyBoss;
    private Vector3 insidePlayerPos = new Vector3(-5.6f,-9.17f);
    private Vector3 originPlayerPos = new Vector3(-5.6f, 0f);
    private Vector3 insideCameraPos = new Vector3(0, -10.17f, -10f);
    private Vector3 originCameraPos = new Vector3(0, 0, -10f);
    [SerializeField] private Vector3[] insideFallingObjectsPos;
    [SerializeField] private Vector3[] fallingObjectPos;
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
    [SerializeField] private Image explosionImg;
    [SerializeField] private int minExplosionTime;
    [SerializeField] private int maxExplosionTime;
    [SerializeField] private float manaRegen;
    [SerializeField] private int[] insidePercentage = { 95, 40 };
    private Color originColor;
    private SkeletonAnimation skeletonAnimation;
    [SerializeField] private ParticleSystem manaExplosionEffect;
    public List<InsideFallingObj> CastedFallingObjList { get => castedFallingObjList; set => castedFallingObjList = value; }

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
    private void SuffleArray()
    {
        int randX;
        int randY;
        Vector3 pos;
        for (int i = 0; i < fallingObjectPos.Length; ++i)
        {
            randX = Random.Range(0, fallingObjectPos.Length);
            randY = Random.Range(0, fallingObjectPos.Length);

            pos = fallingObjectPos[randX];
            fallingObjectPos[randX] = fallingObjectPos[randY];
            fallingObjectPos[randY] = pos;
        }
    }
    private void SuffleOrder(int x, int y, List<GameObject> list, GameObject obj)
    {
        obj = list[x];
        list[x] = list[y];
        list[y] = obj;
    }
    private void SuffleFallingObjects()
    {
        int randX;
        int randY;
        GameObject obj = null;
        GameObject dummy = null;
        for (int i = 0; i < fallingObjects.Count; i++)
        {
            randX = Random.Range(0, fallingObjects.Count);
            randY = Random.Range(0, fallingObjects.Count);
            SuffleOrder(randX, randY, fallingObjects, obj);
            SuffleOrder(randX, randY, fallingObjectDummys, dummy);
        }
        for (int i = 0; i < fallingObjectDummys.Count; i++)
        {
            fallingObjectDummys[i].transform.position = insideFallingObjectsPos[i];
        }
    }
    private void SetFallingObjects(bool isActive)
    {
        if (isActive)
        {
            SuffleArray();
            for (int i = 0; i < fallingObjects.Count; i++)
            {
                fallingObjects[i].transform.position = fallingObjectPos[i];
                Debug.Log(fallingObjects[i].name + fallingObjects[i].transform.position + -insideFallingObjectsPos[i]);
                // fallingObjects[i].transform.position = -fallingObjects[i].transform.position;
            }
        }
        fallingObjects[0].transform.parent.gameObject.SetActive(isActive);
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
    private void KillAllUnitAndPlayer()
    {
        foreach (var item in InGameManager.Instance.Units)
        {
            item.CommonStatus.DecreaseHp(item.CommonStatus.MaxHp);
        }
        InGameManager.Instance.Player.DecreaseHp(InGameManager.Instance.Player.MaxHp);
    }
    private IEnumerator Co_InsidePattern()
    {
        Text moveInsideTimertext = insideMoveObj.GetComponentInChildren<Text>();
        float insideTimer = insideDuration;
        float moveInsideTimer = moveToInsideDuration;
        int patternCount = 0;
        float mpRegenerative = InGameManager.Instance.Player.MpRegenerative;
        while (true)
        {
            if(patternCount > insidePercentage.Length - 1)
            {
                yield break;
            }
            yield return new WaitUntil(() => InGameManager.Instance.Boss.CommonStatus.CurrentHp <= InGameManager.Instance.Boss.CommonStatus.MaxHp * insidePercentage[patternCount] / 100);
            skeletonAnimation.Skeleton.SetColor(Color.clear);
            isInsideReady = true;
            InGameManager.Instance.Boss.isPattern = true;
            InGameManager.Instance.StopAllUnitCoroutines();
            originHpBar.gameObject.SetActive(false);
            insidePortal.gameObject.SetActive(true);
            insideMoveObj.gameObject.SetActive(true);
            insideInstance.gameObject.SetActive(true);
            InGameManager.Instance.Boss.CommonStatus.IsInvincibility = true;
            //InGameManager.Instance.Boss = null;
            while (moveInsideTimer > 0)
            {
                moveInsideTimer -= Time.deltaTime;
                moveInsideTimertext.text = string.Format("보스의 내면으로 이동해야 합니다! 남은 시간 : {0:0}초", System.Math.Ceiling(moveInsideTimer));
                if (isInside)
                {
                    moveInsideTimer = moveToInsideDuration;
                    InGameManager.Instance.Boss = dummyBoss.GetComponent<Boss>();
                    InGameManager.Instance.Player.CoolTimeLimit = true;
                    insideMoveObj.gameObject.SetActive(false);
                    insideHpBar.gameObject.SetActive(true);
                    sceneMoveImg.gameObject.SetActive(true);
                    sceneMoveImg.DOColor(Color.clear, 2f);
                    yield return new WaitForSeconds(2f);
                    sceneMoveImg.gameObject.SetActive(false);
                    sceneMoveImg.color = Color.black;

                    break;
                }
                yield return null;
            }
            if (!isInside)
            {
                InGameManager.Instance.Player.DecreaseHp(InGameManager.Instance.Player.MaxHp);
                Debug.Log("플레이어 사망");
                yield break;
            }
            InGameManager.Instance.Player.MpRegenerative = mpRegenerative * manaRegen;
            fallingObjectDummysParent.gameObject.SetActive(true);
            //마나 재생량 5배 증가
            //InGameManager.Instance.StartAllUnitCoroutines();          
            yield return new WaitUntil(() => dummyBoss.CommonStatus.CurrentHp <= 0);
            Debug.Log("현실로 돌아갑니다");
            InGameManager.Instance.Player.MpRegenerative = mpRegenerative;
            InGameManager.Instance.Boss = GetComponent<Boss>();
            InGameManager.Instance.Player.CoolTimeLimit = false;
            patternCount++;
            Debug.Log(patternCount);
            sceneMoveImg.gameObject.SetActive(true);
            sceneMoveImg.DOColor(Color.clear, 2f);
            MoveToOrigin();
            insidePortal.gameObject.SetActive(false);
            skeletonAnimation.Skeleton.SetColor(originColor); 
            InGameManager.Instance.StartAllUnitCoroutines();
            originHpBar.gameObject.SetActive(true);
            insideInstance.gameObject.SetActive(false);
            fallingObjectDummysParent.gameObject.SetActive(false);
            yield return new WaitForSeconds(2f);
            sceneMoveImg.gameObject.SetActive(false);
            sceneMoveImg.color = Color.black;
            dummyBoss.CommonStatus.CurrentHp = dummyBoss.CommonStatus.MaxHp;
            yield return new WaitForSeconds(3f);
            InGameManager.Instance.Boss.isPattern = false;
            InGameManager.Instance.Boss.CommonStatus.SetAttackDamage(InGameManager.Instance.Boss.CommonStatus.AttackDamage * 1.5f);
            SetFallingObjects(true);
            isInsideReady = false;
            yield return new WaitUntil(() => castedFallingObjList.Count >= 4);
            Debug.Log("cast finish");
            bool isCastSucess = true;
            for (int i = 0; i < castedFallingObjList.Count; i++)
            {
                if(castedFallingObjList[i].gameObject != fallingObjects[i])
                {
                    explosionImg.DOColor(Color.white, 0.8f);
                    yield return new WaitForSeconds(0.8f);
                    explosionImg.color = Color.clear;
                    KillAllUnitAndPlayer();
                    isCastSucess = false;
                    break;
                }
            }
            SetFallingObjects(false);
            foreach (var item in castedFallingObjList)
            {
                item.SetDefault();
            }
            castedFallingObjList.Clear();
            SuffleFallingObjects();
            InGameManager.Instance.Boss.CommonStatus.IsInvincibility = false;
            InGameManager.Instance.Boss.CommonStatus.SetAttackDamage(InGameManager.Instance.Boss.CommonStatus.AttackDamage);
            if (isCastSucess)
            {
                InGameManager.Instance.Boss.isPattern = true;
                yield return new WaitForSeconds(5f);
                InGameManager.Instance.Boss.isPattern = false;
            }          
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
                int rand = Random.Range(minExplosionTime, maxExplosionTime);
                yield return new WaitForSeconds(rand);
                InGameManager.Instance.Boss.isPattern = true;
                skeletonAnimation.AnimationState.SetAnimation(0, "Skill", false);
                if (isInsideReady)
                {
                    Debug.Log("Pattern Stop Cause inside");
                    continue;
                }
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
                manaExplosion.position = new Vector3(InGameManager.Instance.Units[index].transform.position.x,manaExplosion.position.y);
                yield return new WaitForSeconds(manaExplosionDuration);
                manaExplosionEffect.Play();
                yield return new WaitForSeconds(0.4f);
                InGameManager.Instance.Boss.isPattern = false;
                ManaExplosion();
            }
            else
            {
                manaExplosion.position = new Vector3(InGameManager.Instance.Player.transform.position.x, manaExplosion.position.y);
                yield return new WaitForSeconds(manaExplosionDuration);
                manaExplosionEffect.Play();
                yield return new WaitForSeconds(0.25f);
                InGameManager.Instance.Boss.isPattern = false;
                ManaExplosion();
            }
        }
    }
    private void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        originColor = skeletonAnimation.Skeleton.GetColor();
        dummyBoss = transform.Find("InsideInstance").GetComponentInChildren<Boss>();
        SuffleFallingObjects();
        fallingObjectDummysParent = fallingObjectDummys[0].transform.parent;
        //SetFallingObjects();
    }
    private void Start()
    {
        StartCoroutine(Co_ManaExplosion());
        StartCoroutine(Co_InsidePattern());
    }
}
