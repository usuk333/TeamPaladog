using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Player : MonoBehaviour
{
    //캐스팅 관련 변수
    [SerializeField] private CastingObject castingTarget; //캐스팅 타겟이 될 캐스팅 오브젝트
    public GameObject castingButton;
    public GameObject attackButton;
    private bool isCasting; //플레이어가 캐스팅 중인지를 판단. 캐스팅 중일 땐 다른 행동 불가능
    public bool castingButtonDown { get; set; }
    
    public bool move;
    public bool useSkill;
    private bool isInvincibility;
    private bool isInpacable;
    private bool isRight; //오른쪽 이동 체크
    private bool isLeft; // 왼쪽 이동 체크
    private SkeletonAnimation skeletonAnimation;

    private PlayerUI playerUI;
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float maxMp;
    [SerializeField] private float currentMp;
    [SerializeField] private float mpRegenerative;
    [SerializeField] private float hpRegenerative;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float currentMoveSpeed; //이동 속도
    private float shield;
    private float currentShield;
    private bool[] skillCoolTime = { false, false, false, false };
    private Vector3 flip;
    [SerializeField] private GameObject rageArray;
    [SerializeField] private ParticleSystem healEffect;
    [SerializeField] private ParticleSystem shieldEffect;
    [SerializeField] private ParticleSystem rageEffect;
    [SerializeField] private ParticleSystem attackEffect;
    [SerializeField] private Vector3 attackEffectOffset;

    private float[] attackSkillValueArray;

    private float rageValue;
    private float rageMana;

    private float barriorValue;
    private float barriorDuration;
    private float barriorMana;
    private float barriorCool;

    private float healValue;
    private float healMana;
    private float healCool;

    private bool die;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClipArray;

    private bool bCantUseSkill;
    public bool CoolTimeLimit { get; set; }
    public float MaxHp { get => maxHp; }

    public float CurrentHp { get => currentHp; }
    public float MaxMp { get => maxMp; }
    public float CurrentMp { get => currentMp; }
    public bool IsRight { get => isRight; set => isRight = value; }
    public bool IsLeft { get => isLeft; set => isLeft = value; }
    public float Shield
    {
        get
        {
            return shield;
        }
        set
        {
            shield = value;
            currentShield = shield;
        }
    }
    public float CurrentShield { get => currentShield; }
    public bool[] SkillCoolTime { get => skillCoolTime; set => skillCoolTime = value; }
    public float MpRegenerative { get => mpRegenerative; set => mpRegenerative = value; }
    public float HpRegenerative { get => hpRegenerative; set => hpRegenerative = value; }
    public bool Die { get => die; }

    private void InitStatusData()
    {
        List<float[]> skillValueArrayList = new List<float[]>();
        for (int i = 0; i < 2; i++)
        {
            skillValueArrayList.Add(GameManager.Instance.FirebaseData.SkillArray[i].GetSkillValueArray());
        }
        maxHp = skillValueArrayList[0][0];
        hpRegenerative = skillValueArrayList[0][1];
        currentHp = maxHp;

        maxMp = skillValueArrayList[1][0];
        mpRegenerative = skillValueArrayList[1][1];
        currentMp = maxMp;
    }
    private void InitSkillData()
    {
        List<float[]> skillValueArrayList = new List<float[]>();
        for (int i = 2; i < 6; i++)
        {
            skillValueArrayList.Add(GameManager.Instance.FirebaseData.SkillArray[i].GetSkillValueArray());
        }
        attackSkillValueArray = skillValueArrayList[0];

        barriorValue = skillValueArrayList[1][0];
        barriorDuration = skillValueArrayList[1][1];
        barriorMana = skillValueArrayList[1][2];
        barriorCool = skillValueArrayList[1][3];

        healValue = skillValueArrayList[2][0];
        healMana = skillValueArrayList[2][1];
        healCool = skillValueArrayList[2][2];

        rageValue = skillValueArrayList[3][0];
        rageMana = skillValueArrayList[3][1];
    }
    public void KnockBack(Vector3 pos)
    {
        isInpacable = true;
        StartCoroutine(Co_KnockBack(pos));
    }
    public void SetInvincibility(float time)
    {
        isInvincibility = true;
        Invoke("Invoke_UnInvincibility", time);
    }
    public void Move(Vector3 direction) // 이동 함수 (매개변수로 이동할 방향 벡터를 받음)
    {
        if (isCasting) return;
        if (!move) PlayerAnimation(0);
        move = true;
        transform.position += direction * currentMoveSpeed * Time.deltaTime;
        if(transform.position.x > 1)
        {
            bCantUseSkill = true;
        }
        else
        {
            bCantUseSkill = false;
        }
        playerUI.ActiveCantImage(bCantUseSkill);
    }
    public void DecreaseHp(float damage) // 체력 감소 함수 (공격하는 쪽에서 호출)
    {
        if (isInvincibility)
        {
            return;
        }
        if(currentShield > 0)
        {
            currentShield -= damage;
            return;
        }
        else
        {
            shield = 0;
        }
        currentHp -= damage;
        if(currentHp <= 0)
        {
            if (die) return;
            die = true;
            StartCoroutine(Co_Die());
        }
    }
    private IEnumerator Co_Die()
    {
        useSkill = true;
        skeletonAnimation.AnimationState.SetAnimation(0, "Dead", false);
        yield return new WaitForSeconds(1.333f);
        InGameManager.Instance.SetGameOver();
        gameObject.SetActive(false);
        //this.enabled = false;
    }
    public void DecreaseMp(float value)
    {
        currentMp -= value;
        if(currentMp < 0)
        {
            currentMp = 0;
        }
    }
    public void SlowDown(bool isReturn, float percentage = 0)
    {
        if (isReturn)
        {
            currentMoveSpeed = moveSpeed;
            return;
        }
        currentMoveSpeed = moveSpeed - moveSpeed * percentage / 100;
    }
    public void PlayerAnimation(int index)
    {
        switch (index)
        {
            case 0:
                skeletonAnimation.AnimationState.TimeScale = 2.5f;
                skeletonAnimation.AnimationState.SetAnimation(0, "walk", true);
                break;
            case 1:
                skeletonAnimation.AnimationState.TimeScale = 2f;
                skeletonAnimation.AnimationState.SetAnimation(0, "skill", false);
                skeletonAnimation.AnimationState.AddAnimation(0, "Idle", true, 1.5f);
                break;
            case 2:
                skeletonAnimation.AnimationState.TimeScale = 1f;
                skeletonAnimation.AnimationState.SetAnimation(0, "Dead", false);
                break;
            case 3:
                skeletonAnimation.AnimationState.TimeScale = 1f;
                skeletonAnimation.AnimationState.SetAnimation(0, "Idle", false);
                break;
            default:
                break;
        }
    }
    private IEnumerator Co_KnockBack(Vector3 pos)
    {
        if(transform.position.x >= pos.x)
        {
            isInpacable = false;
            yield break;
        }
        while (transform.position != pos)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, currentMoveSpeed * Time.deltaTime);
            yield return null;
        }
        isInpacable = false;
    }
    private IEnumerator Co_Casting()
    {
        while (true)
        {
            yield return null;
            if (castingButtonDown)
            {
                if(castingTarget == null)
                {
                    playerUI.DisableCastingBar();
                    isCasting = false;
                    castingButtonDown = false;
                    continue;
                }
                isCasting = true;
                playerUI.ActiveCastingBar();
                float progress = 0;
                while (progress < castingTarget.CastTime)
                {
                    yield return null;
                    if (!castingButtonDown)
                    {
                        break;
                    }
                    progress += Time.deltaTime;
                    if(castingTarget == null)
                    {
                        break;
                    }
                    else
                    {
                        playerUI.UpdateCastingBar(castingTarget.CastTime, progress);
                    }
                }
                if(castingTarget != null && progress >= castingTarget.CastTime)
                {
                    castingTarget.CastFinish = true;
                    castingButtonDown = false;
                }
                playerUI.DisableCastingBar();
                isCasting = false;
            }
        }
    }
    private IEnumerator Co_UpdateHpAndMp() // 마나 갱신 코루틴
    {
        while (true)
        {
            if(currentMp < maxMp)
            {
                currentMp += mpRegenerative;
                if(currentMp > maxMp)
                {
                    currentMp = maxMp;
                }
            }
            if(currentHp < maxHp)
            {
                currentHp += hpRegenerative;
                if(currentHp > maxHp)
                {
                    currentHp = maxHp;
                }
            }
            yield return new WaitForSeconds(5f);
        }
    }
    private void Awake() // 이니셜라이징으로 뺄거임 22.02.08
    {
        playerUI = GetComponent<PlayerUI>();
        currentHp = maxHp;
        currentMp = maxMp;
        currentMoveSpeed = moveSpeed;
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        flip = transform.localScale;
        skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
        InitSkillData();
        InitStatusData();
    }
    private void Start()
    {
        StartCoroutine(Co_UpdateHpAndMp());
        StartCoroutine(Co_Casting());
        UpdateAttackEffectPosition();
    }
    private void FixedUpdate()
    {
        if (isInvincibility || isInpacable)
        {
            return;
        }
        if (isRight)
        {
            Move(Vector3.right);
        }
        if (isLeft)
        {
            Move(Vector3.left);
        }
        UpdateAttackEffectPosition();
    }
    private void UpdateAttackEffectPosition()
    {
        attackEffect.transform.position = InGameManager.Instance.Boss.transform.position + attackEffectOffset;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "CastingObject")
        {
            castingTarget = collision.GetComponent<CastingObject>();
            if (castingTarget.CastFinish) return;
            castingButton.SetActive(true);
            attackButton.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "CastingObject")
        {
           // if (castingTarget == collision.GetComponent<CastingObject>())
          //  {
            castingTarget = null;
            castingButton.SetActive(false);
            attackButton.SetActive(true);
            // }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "CastingObject")
        {
            castingTarget = collision.transform.GetComponent<CastingObject>();
            if (castingTarget.CastFinish) return;
            castingButton.SetActive(true);
            attackButton.SetActive(false);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "CastingObject")
        {
            // if (castingTarget == collision.GetComponent<CastingObject>())
            //  {
            castingTarget = null;
            castingButton.SetActive(false);
            attackButton.SetActive(true);
            // }
        }
    }
    public void Flip(bool isLeft)
    {
        if (useSkill) return;
        Vector3 scale = flip;
        if (isLeft)
        {
            scale.x *= -1;
        }
        transform.localScale = scale;
    }
    public void UseSkill(int index)
    {
        if (isCasting || bCantUseSkill) return;
        Flip(false);
        SkillTransition(index);
    }
    private void SkillTransition(int index)
    {
        if (skillCoolTime[index])
        {
            Debug.Log("스킬 쿨타임입니다!");
            return;
        }
        if(InGameManager.Instance.Boss == null)
        {
            Debug.Log("보스가 현재 필드에 존재하지 않습니다.");
            return;
        }
        if(index != 3 && CoolTimeLimit)
        {
            return;
        }
        if(useSkill)
        {
            return;
        }
        switch (index)
        {
            case 0:
                if(currentMp < barriorMana)
                {
                    Debug.Log("마나가 부족합니다!");
                    break;
                }
                useSkill = true;
                DecreaseMp(barriorMana);
                StartCoroutine(Co_Skill_Barrier());
                StartCoroutine(Co_SkillCoolTime(0, barriorCool));
                break;
            case 1:
                if (currentMp < healMana)
                {
                    Debug.Log("마나가 부족합니다!");
                    break;
                }
                useSkill = true;
                DecreaseMp(healMana);
                StartCoroutine(Co_Skill_Heal(healValue));
                StartCoroutine(Co_SkillCoolTime(1, healCool));
                break;
            case 2:
                rageArray.SetActive(!rageArray.activeSelf);
                if (rageArray.activeSelf)
                {
                    if (currentMp < rageMana)
                    {
                        Debug.Log("마나가 부족합니다!");
                        rageArray.SetActive(false);
                        rageEffect.Stop();
                        break;
                    }
                    useSkill = true;
                    DecreaseMp(rageMana);
                    StartCoroutine(Co_Skill_Rage());
                }
                else
                {
                    rageEffect.Stop();
                    useSkill = false;
                }
                StartCoroutine(Co_SkillCoolTime(2, 1));
                break;
            case 3:
                if (currentMp < 5)
                {
                    Debug.Log("마나가 부족합니다!");
                    break;
                }
                useSkill = true;
                DecreaseMp(5);
                StartCoroutine(Co_Skill_Attack());
                if (CoolTimeLimit)
                {
                    break;
                }
                StartCoroutine(Co_SkillCoolTime(3, 3));
                break;
            default:
                break;
        }
    }
    private IEnumerator Co_SkillCoolTime(int index, float value)
    {
        skillCoolTime[index] = true;
        playerUI.ShowSkillCoolTime(index, value);
        yield return new WaitForSeconds(value);
        skillCoolTime[index] = false;
    }
    private IEnumerator Co_Skill_Barrier()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, "skill-Y", false);
        skeletonAnimation.AnimationState.AddAnimation(0, "Idle", true, 2f);
        yield return new WaitForSeconds(2f);
        PlayAudio(2);
        useSkill = false;
        foreach (var item in InGameManager.Instance.Units)
        {
            item.CommonStatus.Shield = item.CommonStatus.MaxHp * barriorValue / 100;
            item.ShieldEffect.Play();
        }
        if (currentHp <= 0)
        {
            yield break;
        }
        Shield = maxHp * barriorValue / 100;
        shieldEffect.Play();
        yield return new WaitForSeconds(barriorDuration);
        foreach (var item in InGameManager.Instance.Units)
        {
            item.CommonStatus.Shield = 0;
        }
        Shield = 0;
    }
    private IEnumerator Co_Skill_Heal(float value)
    {
        skeletonAnimation.AnimationState.SetAnimation(0, "skill-G", false);
        skeletonAnimation.AnimationState.AddAnimation(0, "Idle", true, 2f);
        yield return new WaitForSeconds(2f);
        PlayAudio(2);
        useSkill = false;
        foreach (var item in InGameManager.Instance.Units)
        {
            item.CommonStatus.IncreaseHp(item.CommonStatus.MaxHp * value / 100);
            item.HealEffect.Play();
        }
        if(currentHp <= 0)
        {
            yield break;
        }
        currentHp += maxHp * value / 100;
        healEffect.Play();
        if(currentHp > maxHp)
        {
            currentHp = maxHp;
        }
    }
    private IEnumerator Co_Skill_Rage()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, "skill-R", false);
        skeletonAnimation.AnimationState.AddAnimation(0, "Idle", true, 2f);
        yield return new WaitForSeconds(2f);
        PlayAudio(3);
        useSkill = false;
        rageEffect.Play();
        while (rageArray.activeSelf)
        {
            yield return new WaitForSeconds(1f);
            if (!rageArray.activeSelf)
            {
                break;
            }
            DecreaseMp(rageMana);
            if(currentMp <= 0)
            {
                Debug.Log("마나가 부족하여 스킬이 비활성화 됩니다.");
                rageArray.SetActive(false);
                rageEffect.Stop();
            }
        }
    }
    public void Skill_Rage(Unit unit, bool isIn)
    {
        if (isIn)
        {
            unit.CommonStatus.CurrentAttackDamage += unit.CommonStatus.AttackDamage * rageValue / 100;
            if (unit.RageEffect.isPlaying)
            {
                return;
            }
            unit.RageEffect.Play();
        }
        else
        {
            unit.CommonStatus.CurrentAttackDamage = unit.CommonStatus.AttackDamage;
            print(unit.gameObject.name + "은(는) 공격력 증가 범위에서 나갔음");
            unit.RageEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
    private IEnumerator Co_Skill_Attack()
    {
        float rand = Random.Range(0f, 100f);
        string thing;
        if(rand <= 30)
        {
            skeletonAnimation.AnimationState.TimeScale = 1f;
            skeletonAnimation.AnimationState.SetAnimation(0, "Attack-apple", false);
            skeletonAnimation.AnimationState.AddAnimation(0, "Idle", true, 1.167f);
            yield return new WaitForSeconds(1.167f);
            PlayAudio(0);
            InGameManager.Instance.Boss.CommonStatus.DecreaseHp(attackSkillValueArray[0]);
            thing = "30% 확률로 사과를 뽑았다";
        }
        else if(rand > 30 && rand <= 70)
        {
            skeletonAnimation.AnimationState.TimeScale = 1f;
            skeletonAnimation.AnimationState.SetAnimation(0, "Attack-stone", false);
            skeletonAnimation.AnimationState.AddAnimation(0, "Idle", true, 1.167f);
            yield return new WaitForSeconds(1.167f);
            PlayAudio(0);
            InGameManager.Instance.Boss.CommonStatus.DecreaseHp(attackSkillValueArray[1]);
            thing = "40% 확률로 돌맹이를 뽑았다.";
        }
        else if(rand > 70)
        {
            skeletonAnimation.AnimationState.TimeScale = 1f;
            skeletonAnimation.AnimationState.SetAnimation(0, "Attack-bomb", false);
            skeletonAnimation.AnimationState.AddAnimation(0, "Idle", true, 1.167f);
            yield return new WaitForSeconds(1.167f);
            PlayAudio(1);
            attackEffect.Play();
            InGameManager.Instance.Boss.CommonStatus.DecreaseHp(attackSkillValueArray[2]);
            thing = "30% 확률로 폭탄을 뽑았다.";
        }
        else
        {
            thing = "버그";
        }
        useSkill = false;
        Debug.Log(thing);
    }
    private void PlayAudio(int index)
    {
        audioSource.volume = SoundManager.Instance.SfxAudio.volume;
        audioSource.Stop();
        audioSource.clip = audioClipArray[index];
        audioSource.Play();
    }
}
