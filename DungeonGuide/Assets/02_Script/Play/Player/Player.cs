using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Player : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particleArray;
    public ParticleSystem[] PlayerParticleArray { get => particleArray; }

    [SerializeField] private List<PlayerSkill> playerSkillList = new List<PlayerSkill>();

    private SpineAnimator spineAnimator;
    private enum State
    {
        Idle,
        Walk,
        SkillY,
        SkillG,
        SkillR,
        AttackApple,
        AttackStone,
        AttackBomb,
        Dead
    }
    private State state = State.Idle;

    /*-------------------------------------------밑에부턴 구버전 변수, 위에가 새로 추가한 변수(23.01.02)----------------------------------------------------------
     * 
     */
    //캐스팅 관련 변수
    [SerializeField] private CastingObject castingTarget; //캐스팅 타겟이 될 캐스팅 오브젝트
    public GameObject castingButton;
    public GameObject attackButton;
    private bool isCasting; //플레이어가 캐스팅 중인지를 판단. 캐스팅 중일 땐 다른 행동 불가능
    public bool castingButtonDown { get; set; }
    
    public bool useSkill;

    private bool isRight; //오른쪽 이동 체크
    private bool isLeft; // 왼쪽 이동 체크

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
    private Vector3 flip;
    [SerializeField] private Vector3 attackEffectOffset;


    private bool dead;

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
    public float MpRegenerative { get => mpRegenerative; set => mpRegenerative = value; }
    public bool Dead { get => dead; }
    public void SetState(int index)
    {
        if (index > 8 || state == State.Dead) return;
        state = (State)index;
    }
    public void IncreaseCurrentHp(float value)
    {
        currentHp += value;
        if (currentHp > maxHp) currentHp = maxHp;
    }
    private IEnumerator Co_PlayAnimationByState()
    {
        while (true)
        {
            yield return null;
            if (spineAnimator.IsCurrentAnimation((int)state)) continue;
            if((int)state == 0) spineAnimator.SetAnimation(0, true, 1f);
            else if((int)state == 1) spineAnimator.SetAnimation(1, true, 2.5f);
            else if((int)state > 1 && (int)state < 5)
            {
                spineAnimator.SetAnimation((int)state, false, 1f);
                yield return new WaitForSeconds(2f);
                SetState(0);
            }
            else if((int)state > 4 && (int)state < 8)
            {
                spineAnimator.SetAnimation((int)state, false, 1f);
                yield return new WaitForSeconds(1.167f);
                SetState(0);
            }
            else
            {
                dead = true;
                spineAnimator.SetAnimation(8, false, 1f);
                yield return new WaitForSeconds(1.333f);
                InGameManager.Instance.SetGameOver();
                gameObject.SetActive(false);
            }
        }
    }
    //-------------------------------------밑에는 구버전 함수, 위에는 새로 추가한 함수(23.01.02)------------------------------------
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
        playerSkillList.Add(GetComponentInChildren<Attack>());
        playerSkillList.Add(GetComponentInChildren<Barrior>());
        playerSkillList.Add(GetComponentInChildren<Heal>());
        playerSkillList.Add(GetComponentInChildren<Rage>());

        for (int i = 0; i < skillValueArrayList.Count; i++)
        {
            playerSkillList[i].SetSkillData(skillValueArrayList[i]);
        }
    }
    public void Move(Vector3 direction) // 이동 함수 (매개변수로 이동할 방향 벡터를 받음)
    {
        if (isCasting || useSkill) return;
        SetState(1);
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
            SetState(8);
        }
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
        currentMoveSpeed = moveSpeed;
        flip = transform.localScale;
        InitSkillData();
        InitStatusData();
        string[] animationNameArray = { "Idle", "walk", "skill-Y", "skill-G", "skill-R","Attack-apple", "Attack-stone", "Attack-bomb", "Dead"};
        spineAnimator = new SpineAnimator(GetComponent<SkeletonAnimation>(), animationNameArray);      
    }
    private void Start()
    {
        StartCoroutine(Co_PlayAnimationByState());
        StartCoroutine(Co_UpdateHpAndMp());
        StartCoroutine(Co_Casting());
    }
    private void FixedUpdate()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.RightArrow)) Move(Vector3.right);
        if (Input.GetKey(KeyCode.LeftArrow)) Move(Vector3.left);
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)) SetState(0);
#endif
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
        particleArray[3].transform.position = InGameManager.Instance.Boss.transform.position + attackEffectOffset;
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
        Vector3 scale = flip;
        if (isLeft)
        {
            scale.x *= -1;
        }
        transform.localScale = scale;
    }
    public void UseSkill(int index)
    {
        if (isCasting || bCantUseSkill || useSkill) return;
        StartCoroutine(playerSkillList[index].Co_UseSkill());
    }
    public void PlayAudio(int index)
    {
        audioSource.volume = SoundManager.Instance.SfxAudio.volume;
        audioSource.Stop();
        audioSource.clip = audioClipArray[index];
        audioSource.Play();
    }
}
