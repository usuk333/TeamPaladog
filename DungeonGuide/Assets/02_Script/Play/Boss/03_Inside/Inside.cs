using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

public class Inside : MonoBehaviour
{
    [SerializeField] private Image warningImage;
    [SerializeField] private Text warningText;

    [SerializeField] private List<GameObject> fallingObjectDummys = new List<GameObject>();
    [SerializeField] private List<GameObject> fallingObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> castedFallingObjList = new List<GameObject>();
    [SerializeField] private List<CastingObject> castingObjects = new List<CastingObject>();
    private Transform fallingObjectDummysParent;
    private bool isInsideReady;
    private bool isInside;
    private int explosionCount = 0;
    private Boss dummyBoss;
    private Vector3 insidePlayerPos = new Vector3(-5.6f,-13f);
    private Vector3 originPlayerPos = new Vector3(-5.6f, -2f);
    private Vector3 insideCameraPos = new Vector3(0, -10.17f, -10f);
    private Vector3 originCameraPos = new Vector3(0, 0, -10f);
    [SerializeField] private Vector3[] insideFallingObjectsPos;
    [SerializeField] private Vector3[] fallingObjectPos;
    [SerializeField] private Transform originHpBar;
    [SerializeField] private Transform insideHpBar;
    [SerializeField] private Transform insideInstance;
    [SerializeField] private Transform insidePortal;
    [SerializeField] private float insideDuration;
    [SerializeField] private Transform insideMoveObj;
    [SerializeField] private Transform insideLimitTimerObj;
    [SerializeField] private Transform manaExplosion;
    [SerializeField] private float manaExplosionDuration;
    [SerializeField] private Image sceneMoveImg;
    [SerializeField] private Image explosionImg;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClipArray;

    private Color originColor;
    private SkeletonAnimation skeletonAnimation;
    public bool difficulty { get; set; }

    [SerializeField] private ParticleSystem manaExplosionEffect;

    public InsideStatus insideStatus;

    private IEnumerator Co_MoveToInside()
    {
        CastingObject castingObject = insidePortal.GetComponent<CastingObject>();
        while (true)
        {
            yield return new WaitUntil(() => castingObject.CastFinish);
            Debug.Log("내면으로 이동");
            isInside = true;
            InGameManager.Instance.Player.transform.position = insidePlayerPos;
            Camera.main.transform.position = insideCameraPos;
            castingObject.CastFinish = false;
        }
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
            InGameManager.Instance.Player.DecreaseHp(insideStatus.firstPatternDamage);
            InGameManager.Instance.Player.DecreaseMp(InGameManager.Instance.Player.MaxMp * insideStatus.firstPatternMana / 100);
            foreach (var item in InGameManager.Instance.Units)
            {
                if (InGameManager.Instance.Boss.CollisionsArray[0].Contains(item.gameObject))
                {
                    item.CommonStatus.DecreaseHp(insideStatus.firstPatternDamage);
                    if (item.CommonStatus.CurrentHp <= 0)
                    {
                        explosionCount++;
                    }
                }
            }
        }
        else
        {
            foreach (var item in InGameManager.Instance.Units)
            {
                if (InGameManager.Instance.Boss.CollisionsArray[0].Contains(item.gameObject))
                {
                    item.CommonStatus.DecreaseHp(insideStatus.firstPatternTrueDamage);
                    if(item.CommonStatus.CurrentHp <= 0)
                    {
                        explosionCount++;
                    }
                    return;
                }
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
    private void ReadyToInside()
    {
        isInsideReady = true;
        InGameManager.Instance.Boss.IsPattern = true;
        InGameManager.Instance.StopAllUnitCoroutines();
        originHpBar.gameObject.SetActive(false);
        insidePortal.gameObject.SetActive(true);
        insideMoveObj.gameObject.SetActive(true);
        insideInstance.gameObject.SetActive(true);
        InGameManager.Instance.Boss.CommonStatus.IsInvincibility = true;
    }
    private void SettingInside()
    {
        InGameManager.Instance.Boss = dummyBoss.GetComponent<Boss>();
        InGameManager.Instance.Boss.CommonStatus.MaxHp = insideStatus.secondPatternDummyHp;
        InGameManager.Instance.Boss.CommonStatus.CurrentHp = insideStatus.secondPatternDummyHp;
        InGameManager.Instance.Player.CoolTimeLimit = true;
        insideMoveObj.gameObject.SetActive(false);
        insideHpBar.gameObject.SetActive(true);
        sceneMoveImg.gameObject.SetActive(true);
        sceneMoveImg.DOFade(0, 2f);
    }
    private IEnumerator Co_InsidePattern()
    {
        Text moveInsideTimertext = insideMoveObj.GetChild(1).GetComponent<Text>();
        Text insideLimitTimerText = insideLimitTimerObj.GetChild(1).GetComponent<Text>();
        float insideTimer = insideDuration;
        float moveInsideTimer = insideStatus.secondPatternPortalDuration;
        float insideLimitTimer = insideStatus.secondPatternPortalDuration;
        int patternCount = 0;
        float mpRegenerative = InGameManager.Instance.Player.MpRegenerative;
        while (true)
        {
            if(patternCount > insideStatus.secondPatternPercentage.Length - 1)
            {
                isInsideReady = false;
                yield break;
            }
            yield return new WaitUntil(() => InGameManager.Instance.Boss.CommonStatus.CurrentHp <= InGameManager.Instance.Boss.CommonStatus.MaxHp * insideStatus.secondPatternPercentage[patternCount] / 100);
            PlayAudio(0);
            warningImage.gameObject.SetActive(true);
            warningImage.DOFade(1, 2f);
            warningText.DOFade(1, 2f);
            yield return new WaitForSeconds(2f);
            float progress = 1;
            while(progress > 0)
            {
                skeletonAnimation.skeleton.A = progress;
                progress -= Time.deltaTime;
                yield return null;
            }
            warningImage.DOFade(0, 1f);
            warningText.DOFade(0, 1f);
            yield return new WaitForSeconds(1f);
            warningImage.gameObject.SetActive(false);
            ReadyToInside();
            while (moveInsideTimer > 0)
            {
                moveInsideTimer -= Time.deltaTime;
                moveInsideTimertext.text = string.Format("남은 시간 : {0:0}초", System.Math.Ceiling(moveInsideTimer));
                if (isInside)
                {
                    moveInsideTimer = insideStatus.secondPatternPortalDuration;
                    SettingInside();
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
            
            InGameManager.Instance.Player.MpRegenerative = mpRegenerative + insideStatus.secondPatternManaRegen;
            fallingObjectDummysParent.gameObject.SetActive(true);
            StartCoroutine(Co_ZeoliteSound(1.3f));
            insideLimitTimerObj.gameObject.SetActive(true);

            while(insideLimitTimer > 0)
            {
                insideLimitTimer -= Time.deltaTime;
                insideLimitTimerText.text = string.Format("남은 시간 : {0:0}초", System.Math.Ceiling(insideLimitTimer));
                yield return null;
                if (dummyBoss.CommonStatus.CurrentHp <= 0) break;
            }
            insideLimitTimerObj.gameObject.SetActive(false);
            if (insideLimitTimer <= 0)
            {
                InGameManager.Instance.Player.DecreaseHp(InGameManager.Instance.Player.MaxHp);
                Debug.Log("플레이어 사망");
                yield break;
            }
            
            insideLimitTimer = insideStatus.secondPatternPortalDuration;
            yield return new WaitForSeconds(1f);
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
            insideHpBar.gameObject.SetActive(false);
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
            InGameManager.Instance.Boss.IsPattern = false;
            InGameManager.Instance.Boss.CommonStatus.SetAttackDamage(InGameManager.Instance.Boss.CommonStatus.AttackDamage * 1.5f);
            SetFallingObjects(true);
            StartCoroutine(Co_ZeoliteSound(1.2f));
            StartCoroutine(Co_DotDamageWhileZeoliteActive());
            StartCoroutine(Co_CheckCastedFallingObj());
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
            SetDefaultFalliingObjects();
            castedFallingObjList.Clear();
            SuffleFallingObjects();
            InGameManager.Instance.Boss.CommonStatus.IsInvincibility = false;
            InGameManager.Instance.Boss.CommonStatus.SetAttackDamage(InGameManager.Instance.Boss.CommonStatus.AttackDamage);
            if (isCastSucess)
            {
                InGameManager.Instance.Boss.IsPattern = true;
                yield return new WaitForSeconds(5f);
                InGameManager.Instance.Boss.IsPattern = false;
            }          
        }      
    }
    private IEnumerator Co_DotDamageWhileZeoliteActive()
    {
        while (InGameManager.Instance.Boss.CommonStatus.IsInvincibility)
        {
            InGameManager.Instance.Player.DecreaseHp(InGameManager.Instance.Player.MaxHp * 1 / 100);
            foreach (var item in InGameManager.Instance.Units)
            {
                item.CommonStatus.DecreaseHp(item.CommonStatus.MaxHp * 1 / 100);
            }
            yield return new WaitForSeconds(2f);
        }
    }
    private IEnumerator Co_CheckCastedFallingObj()
    {
        while (castedFallingObjList.Count < 4)
        {
            yield return null;
            foreach (var item in castingObjects)
            {
                if (castedFallingObjList.Contains(item.gameObject))
                {
                    continue;
                }
                if (item.CastFinish)
                {
                    castedFallingObjList.Add(item.gameObject);
                }
            }
        }
    }
    private void SetDefaultFalliingObjects()
    {
        foreach (var item in castingObjects)
        {
            item.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            item.GetComponent<BoxCollider2D>().isTrigger = false;
            item.CastFinish = false;
            item.transform.gameObject.layer = 8;
            item.GetComponent<SpriteRenderer>().color = Color.white;
            item.gameObject.SetActive(true);
            StartCoroutine(item.GetComponent<InsideFallingObj>().Co_FadeAnim());
        }
    }
    private IEnumerator Co_DifficultyPattern()
    {
        while (true)
        {
            InGameManager.Instance.Player.DecreaseHp(55);
            foreach (var item in InGameManager.Instance.Units)
            {
                item.CommonStatus.DecreaseHp(55);
            }
            yield return new WaitForSeconds(1f);
        }
    }
    private IEnumerator Co_ManaExplosion()
    {
        var sprite = manaExplosion.GetComponent<SpriteRenderer>();
        Vector3 manaExplosionOriginScale = new Vector3(manaExplosion.localScale.x, 0);
        Vector3 manaExplosionOriginPos = new Vector3(manaExplosion.position.x, 36.2f);
        while (true)
        {
            manaExplosion.gameObject.SetActive(false);
            sprite.enabled = true;
            manaExplosion.localScale = manaExplosionOriginScale;
            manaExplosion.localPosition = manaExplosionOriginPos;
            if (isInsideReady)
            {
                yield return null;
                continue;
            }
            if (explosionCount < 4)
            {
                int rand = (int)Random.Range(insideStatus.firstPatternMinTime, insideStatus.firstPatternMaxTime);
                yield return new WaitForSeconds(rand);
                InGameManager.Instance.Boss.IsPattern = true;
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
                    index = Random.Range(0, InGameManager.Instance.Units.Count);
                    if (InGameManager.Instance.Units[index].CommonStatus.CurrentHp > 0)
                    {
                        break;
                    }
                    yield return null;
                }
                manaExplosion.position = new Vector3(InGameManager.Instance.Units[index].transform.position.x,manaExplosion.position.y);
                PatternTweening();
                yield return new WaitForSeconds(manaExplosionDuration);
                sprite.enabled = false;
                manaExplosionEffect.Play();
                yield return new WaitForSeconds(0.4f);
                if (!isInsideReady)
                {
                    InGameManager.Instance.Boss.IsPattern = false;
                }
                ManaExplosion();
            }
            else
            {
                manaExplosion.position = new Vector3(InGameManager.Instance.Player.transform.position.x, manaExplosion.position.y);
                PatternTweening();
                yield return new WaitForSeconds(manaExplosionDuration);
                sprite.enabled = false;
                manaExplosionEffect.Play();
                yield return new WaitForSeconds(0.25f);
                if (!isInsideReady)
                {
                    InGameManager.Instance.Boss.IsPattern = false;
                }
                ManaExplosion();
            }
        }
    }
    private void PatternTweening()
    {
        manaExplosion.DOScaleY(37, manaExplosionDuration - 0.5f);
        manaExplosion.DOLocalMoveY(17.9f, manaExplosionDuration - 0.5f);
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
        StartCoroutine(Co_MoveToInside());
        if (difficulty)
        {
            StartCoroutine(Co_DifficultyPattern());
        }
    }
    private void PlayAudio(int index)
    {
        audioSource.volume = SoundManager.Instance.SfxAudio.volume;
        audioSource.Stop();
        audioSource.clip = audioClipArray[index];
        audioSource.Play();
    }
    private IEnumerator Co_ZeoliteSound(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayAudio(1);
    }
}
