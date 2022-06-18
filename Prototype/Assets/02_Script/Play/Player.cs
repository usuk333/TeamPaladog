using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool isInvincibility;
    private bool isInpacable;
    private bool isRight; //오른쪽 이동 체크
    private bool isLeft; // 왼쪽 이동 체크
    private Boss boss;
    public bool isCast { get; set; } = false;
    public bool isCastFinish { get; set; } = false;
    private PlayerUI playerUI;
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float maxMp;
    [SerializeField] private float currentMp;
    [SerializeField] private float mpRegenerative;
    [SerializeField] private float mpRegenerationInterval;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float currentMoveSpeed; //이동 속도
    private float shield;
    private float currentShield;
    private float castingTime;
    private bool[] skillCoolTime = { false, false, false, false };
    [SerializeField] private List<Unit> barrierList = new List<Unit>();
    [SerializeField] private List<Unit> healList = new List<Unit>();
    [SerializeField] private GameObject[] skillRangeArray;
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
            currentHp = maxHp; //최대 체력 설정 시 현재 체력 최대 체력으로 초기화
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
            currentMp = maxMp;//최대 마나 설정 시 현재 마나 최대 마나로 초기화
        }
    }
    public float CurrentMp { get => currentMp; }
    public float MpRegenerative { get => mpRegenerative; set => mpRegenerative = value; }
    public bool IsRight { get => isRight; set => isRight = value; }
    public bool IsLeft { get => isLeft; set => isLeft = value; }
    public List<Unit> BarrierList { get => barrierList; set => barrierList = value; }
    public List<Unit> HealList { get => healList; set => healList = value; }
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
        transform.position += direction * currentMoveSpeed * Time.deltaTime;
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
    }
    public void DecreaseMp(float value)
    {
        currentMp -= value;
        if(currentMp < 0)
        {
            currentMp = 0;
        }
    }
    public void Casting(float time)
    {
        castingTime = time;
        isCast = true;
        playerUI.ActiveCastingBar();
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
            if (isCast)
            {
                //isCastFinish = false;
                float progress = 0;
                while (progress < castingTime)
                {
                    if (!isCast)
                    {
                        playerUI.DisableCastingBar();
                        break;
                    }
                    playerUI.UpdateCastingBar(castingTime, progress);
                    progress += Time.deltaTime;
                    yield return null;
                }
                if(progress >= castingTime)
                {
                    isCastFinish = true;
                }
                playerUI.DisableCastingBar();
            }
            yield return null;
        }
    }
    private IEnumerator Co_UpdateMp() // 마나 갱신 코루틴
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
                yield return new WaitForSeconds(mpRegenerationInterval);
            }
            yield return null;
        }
    }
    private void Awake() // 이니셜라이징으로 뺄거임 22.02.08
    {
        playerUI = GetComponent<PlayerUI>();
        boss = FindObjectOfType<Boss>();
        currentHp = maxHp;
        currentMp = maxMp;
        currentMoveSpeed = moveSpeed;
    }
    private void Start()
    {
        StartCoroutine(Co_UpdateMp());
        StartCoroutine(Co_Casting());
    }
    private void Update()
    {
        /*if (Input.GetKey(KeyCode.RightArrow))
        {
            isRight = true;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            isRight = false;
        }*/
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            isLeft = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            isLeft = false;
        }
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
    public void UseSkill(int index)
    {
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
        switch (index)
        {
            case 0:
                if(currentMp < 5)
                {
                    Debug.Log("마나가 부족합니다!");
                    break;
                }
                DecreaseMp(5);
                StartCoroutine(Co_Skill_Barrier());
                StartCoroutine(Co_SkillCoolTime(0, 15));
                break;
            case 1:
                if (currentMp < 2)
                {
                    Debug.Log("마나가 부족합니다!");
                    break;
                }
                DecreaseMp(2);
                Skill_Heal(10);
                StartCoroutine(Co_SkillCoolTime(1, 10));
                break;
            case 2:
                skillRangeArray[2].SetActive(!skillRangeArray[2].activeSelf);
                if (skillRangeArray[2].activeSelf)
                {
                    if (currentMp < 3)
                    {
                        Debug.Log("마나가 부족합니다!");
                        skillRangeArray[2].SetActive(false);
                        break;
                    }
                    DecreaseMp(3);
                    StartCoroutine(Co_Skill_Rage());
                }
                StartCoroutine(Co_SkillCoolTime(2, 1));
                break;
            case 3:
                if (currentMp < 5)
                {
                    Debug.Log("마나가 부족합니다!");
                    break;
                }
                DecreaseMp(1);
                Skill_Attack();
                if (CoolTimeLimit)
                {
                    break;
                }
                StartCoroutine(Co_SkillCoolTime(3, 5));
                break;
            default:
                break;
        }
    }
    private IEnumerator Co_SkillCoolTime(int index, float value)
    {
        skillCoolTime[index] = true;
        yield return new WaitForSeconds(value);
        skillCoolTime[index] = false;
    }
    private IEnumerator Co_Skill_Barrier()
    {
        foreach (var item in barrierList)
        {
            item.CommonStatus.Shield = 10;
        }
        Shield = 10;
        yield return new WaitForSeconds(3f);
        foreach (var item in barrierList)
        {
            item.CommonStatus.Shield = 0;
        }
        Shield = 0;
        barrierList.Clear();
    }
    private void Skill_Heal(float value)
    {
        foreach (var item in healList)
        {
            item.CommonStatus.IncreaseHp(item.CommonStatus.MaxHp * value / 100);
        }
        currentHp += maxHp * value / 100;
        if(currentHp > maxHp)
        {
            currentHp = maxHp;
        }
    }
    private IEnumerator Co_Skill_Rage()
    {
        while (skillRangeArray[2].activeSelf)
        {
            yield return new WaitForSeconds(2f);
            DecreaseMp(2);
            if(currentMp <= 0)
            {
                Debug.Log("마나가 부족하여 스킬이 비활성화 됩니다.");
                skillRangeArray[2].SetActive(false);
            }
        }
    }
    public void Skill_Rage(Unit unit, bool isIn)
    {
        if (isIn)
        {
            unit.CommonStatus.CurrentAttackDamage += unit.CommonStatus.AttackDamage * 10 / 100;
        }
        else
        {
            unit.CommonStatus.CurrentAttackDamage = unit.CommonStatus.AttackDamage;
        }
    }
    private void Skill_Attack()
    {
        float rand = Random.Range(0f, 100f);
        string thing;
        if(rand <= 30)
        {
            InGameManager.Instance.Boss.CommonStatus.DecreaseHp(1);
            thing = "30% 확률로 사과를 뽑았다";
        }
        else if(rand > 30 && rand <= 70)
        {
            InGameManager.Instance.Boss.CommonStatus.DecreaseHp(2);
            thing = "40% 확률로 돌맹이를 뽑았다.";
        }
        else if(rand > 70)
        {
            InGameManager.Instance.Boss.CommonStatus.DecreaseHp(3);
            thing = "30% 확률로 폭탄을 뽑았다.";
        }
        else
        {
            thing = "버그";
        }
        Debug.Log(thing);
    }
}
