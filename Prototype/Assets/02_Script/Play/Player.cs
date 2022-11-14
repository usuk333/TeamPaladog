using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Player : MonoBehaviour
{
    //ĳ���� ���� ����
    [SerializeField] private CastingObject castingTarget; //ĳ���� Ÿ���� �� ĳ���� ������Ʈ
    public GameObject castingButton;
    public GameObject attackButton;
    private bool isCasting; //�÷��̾ ĳ���� �������� �Ǵ�. ĳ���� ���� �� �ٸ� �ൿ �Ұ���
    public bool castingButtonDown { get; set; }

    public bool move;
    public bool useSkill;
    private bool isInvincibility;
    private bool isInpacable;
    private bool isRight; //������ �̵� üũ
    private bool isLeft; // ���� �̵� üũ
    private Boss boss;
    private SkeletonAnimation skeletonAnimation;

    private PlayerUI playerUI;
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float maxMp;
    [SerializeField] private float currentMp;
    [SerializeField] private float mpRegenerative;
    [SerializeField] private float hpRegenerative;
    [SerializeField] private float mpRegenerationInterval;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float currentMoveSpeed; //�̵� �ӵ�
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

    private bool die;
    [SerializeField] private float[] skillValueArray;
    [SerializeField] private float[] skillCoolTimeArray;
    [SerializeField] private float[] skillManaArray;
    [SerializeField] private float shieldDuration;
    public bool CoolTimeLimit { get; set; }
    public float MaxHp
    { 
        get
        {
            return maxHp;
        }
        set
        {
            maxHp = value;
            currentHp = maxHp; //�ִ� ü�� ���� �� ���� ü�� �ִ� ü������ �ʱ�ȭ
        }
    }
    public float CurrentHp { get => currentHp; }
    public float MaxMp
    {
        get
        {
            return maxMp;
        }
        set
        {
            maxMp = value;
            currentMp = maxMp;//�ִ� ���� ���� �� ���� ���� �ִ� ������ �ʱ�ȭ
        }
    }
    public float CurrentMp { get => currentMp; }
    public float MpRegenerative { get => mpRegenerative; set => mpRegenerative = value; }
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
    public float[] SkillValueArray { get => skillValueArray; set => skillValueArray = value; }
    public float[] SkillCoolTimeArray { get => skillCoolTimeArray; set => skillCoolTimeArray = value; }
    public float[] SkillManaArray { get => skillManaArray; set => skillManaArray = value; }
    public float HpRegenerative { get => hpRegenerative; set => hpRegenerative = value; }
    public float ShieldDuration { get => shieldDuration; set => shieldDuration = value; }

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
    public void Move(Vector3 direction) // �̵� �Լ� (�Ű������� �̵��� ���� ���͸� ����)
    {
        if (isCasting || useSkill) return;
        if (!move) PlayerAnimation(0);
        move = true;
        transform.position += direction * currentMoveSpeed * Time.deltaTime;
    }
    public void DecreaseHp(float damage) // ü�� ���� �Լ� (�����ϴ� �ʿ��� ȣ��)
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
        skeletonAnimation.AnimationState.SetAnimation(0, "Dead", false);
        yield return new WaitForSeconds(1.333f);
        InGameManager.Instance.SetGameOver();
        this.enabled = false;
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
    private void Invoke_UnInvincibility()
    {
        isInvincibility = false;
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
    private IEnumerator Co_UpdateHpAndMp() // ���� ���� �ڷ�ƾ
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
    private void Awake() // �̴ϼȶ���¡���� ������ 22.02.08
    {
        playerUI = GetComponent<PlayerUI>();
        boss = FindObjectOfType<Boss>();
        currentHp = maxHp;
        currentMp = maxMp;
        currentMoveSpeed = moveSpeed;
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        flip = transform.localScale;
        skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
    }
    private void Start()
    {
        StartCoroutine(Co_UpdateHpAndMp());
        StartCoroutine(Co_Casting());
        UpdateAttackEffectPosition();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            isRight = true;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            isRight = false;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            isLeft = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            isLeft = false;
        }
        UpdateAttackEffectPosition();
        /*if (Input.GetKeyDown(KeyCode.S))
        {
            foreach (var item in InGameManager.Instance.Units)
            {
                item.gameObject.SetActive(false);
            }       
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            foreach (var item in InGameManager.Instance.Units)
            {
                item.gameObject.SetActive(true);
            }
        }*/

    }
    private void FixedUpdate()
    {
        //attackEffect.transform.position = boss.transform.position + attackEffectOffset;
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
        if (isCasting) return;
        SkillTransition(index);
    }
    private void SkillTransition(int index)
    {
        if (skillCoolTime[index])
        {
            Debug.Log("��ų ��Ÿ���Դϴ�!");
            return;
        }
        if(InGameManager.Instance.Boss == null)
        {
            Debug.Log("������ ���� �ʵ忡 �������� �ʽ��ϴ�.");
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
        useSkill = true;
        switch (index)
        {
            case 0:
                if(currentMp < 5)
                {
                    Debug.Log("������ �����մϴ�!");
                    break;
                }
                DecreaseMp(skillManaArray[0]);
                StartCoroutine(Co_Skill_Barrier());
                StartCoroutine(Co_SkillCoolTime(0, skillCoolTimeArray[0]));
                break;
            case 1:
                if (currentMp < 2)
                {
                    Debug.Log("������ �����մϴ�!");
                    break;
                }
                DecreaseMp(skillManaArray[1]);
                StartCoroutine(Co_Skill_Heal(skillValueArray[1]));
                StartCoroutine(Co_SkillCoolTime(1, skillCoolTimeArray[1]));
                break;
            case 2:
                rageArray.SetActive(!rageArray.activeSelf);
                if (rageArray.activeSelf)
                {
                    if (currentMp < skillManaArray[2])
                    {
                        Debug.Log("������ �����մϴ�!");
                        rageArray.SetActive(false);
                        rageEffect.Stop();
                        break;
                    }
                    DecreaseMp(skillManaArray[2]);
                    StartCoroutine(Co_Skill_Rage());
                }
                else
                {
                    rageEffect.Stop();
                    useSkill = false;
                }
                StartCoroutine(Co_SkillCoolTime(2, skillCoolTimeArray[2]));
                break;
            case 3:
                if (currentMp < 5)
                {
                    Debug.Log("������ �����մϴ�!");
                    break;
                }
                DecreaseMp(skillManaArray[3]);
                StartCoroutine(Co_Skill_Attack());
                if (CoolTimeLimit)
                {
                    break;
                }
                StartCoroutine(Co_SkillCoolTime(3, skillCoolTimeArray[3]));
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
        useSkill = false;
        foreach (var item in InGameManager.Instance.Units)
        {
            item.CommonStatus.Shield = item.CommonStatus.MaxHp * skillValueArray[0] / 100;
            item.ShieldEffect.Play();
        }
        Shield = maxHp * skillValueArray[0] / 100;
        shieldEffect.Play();
        yield return new WaitForSeconds(shieldDuration);
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
        useSkill = false;
        foreach (var item in InGameManager.Instance.Units)
        {
            item.CommonStatus.IncreaseHp(item.CommonStatus.MaxHp * value / 100);
            item.HealEffect.Play();
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
        useSkill = false;
        rageEffect.Play();
        while (rageArray.activeSelf)
        {
            yield return new WaitForSeconds(1f);
            if (!rageArray.activeSelf)
            {
                break;
            }
            DecreaseMp(skillManaArray[2]);
            if(currentMp <= 0)
            {
                Debug.Log("������ �����Ͽ� ��ų�� ��Ȱ��ȭ �˴ϴ�.");
                rageArray.SetActive(false);
                rageEffect.Stop();
            }
        }
    }
    public void Skill_Rage(Unit unit, bool isIn)
    {
        if (isIn)
        {
            unit.CommonStatus.CurrentAttackDamage += unit.CommonStatus.AttackDamage * skillValueArray[2] / 100;
            if (unit.RageEffect.isPlaying)
            {
                return;
            }
            unit.RageEffect.Play();
        }
        else
        {
            unit.CommonStatus.CurrentAttackDamage = unit.CommonStatus.AttackDamage;
            print(unit.gameObject.name + "��(��) ���ݷ� ���� �������� ������");
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
            InGameManager.Instance.Boss.CommonStatus.DecreaseHp(skillValueArray[3]);
            thing = "30% Ȯ���� ����� �̾Ҵ�";
        }
        else if(rand > 30 && rand <= 70)
        {
            skeletonAnimation.AnimationState.TimeScale = 1f;
            skeletonAnimation.AnimationState.SetAnimation(0, "Attack-stone", false);
            skeletonAnimation.AnimationState.AddAnimation(0, "Idle", true, 1.167f);
            yield return new WaitForSeconds(1.167f);
            InGameManager.Instance.Boss.CommonStatus.DecreaseHp(skillValueArray[4]);
            thing = "40% Ȯ���� �����̸� �̾Ҵ�.";
        }
        else if(rand > 70)
        {
            skeletonAnimation.AnimationState.TimeScale = 1f;
            skeletonAnimation.AnimationState.SetAnimation(0, "Attack-bomb", false);
            skeletonAnimation.AnimationState.AddAnimation(0, "Idle", true, 1.167f);
            yield return new WaitForSeconds(1.167f);
            attackEffect.Play();
            InGameManager.Instance.Boss.CommonStatus.DecreaseHp(skillValueArray[5]);
            thing = "30% Ȯ���� ��ź�� �̾Ҵ�.";
        }
        else
        {
            thing = "����";
        }
        useSkill = false;
        Debug.Log(thing);
    }
}
