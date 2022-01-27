using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour //플레이어 능력치와 기능을 관리하는 스크립트
{
    private float currentMoveSpeed;
    private float maxResauce = 25f;
    private Vector3 moveDirection = Vector3.zero;
    private bool isStun = false;
    private bool isLeft = false;
    private bool isRight = false;
    private bool isSpeedUp = false;
    private bool isInvincibility = false;
    private GameObject speedUpRange;
    private GameObject invincibilityRange;
    private GameObject healingRange;
    private GameObject teleportRange;
    [SerializeField] private GameObject shield;
    [SerializeField] private float currentResauce = 0f;
    [SerializeField] private float resauceChargingTime;
    [SerializeField] private float maxHp;
    [SerializeField] private float maxMp;
    [SerializeField] private float currentHp;
    [SerializeField] private float currentMp;
    [SerializeField] private float attackPower;
    [SerializeField] private float moveSpeed;
    [SerializeField] private PlayerSkill[] playerSkills;
    [SerializeField] private Enemy currentEnemy;
    [SerializeField] private Boss boss;
    [SerializeField] private List<Unit> speedUpUnits = new List<Unit>();
    [SerializeField] private List<Unit> invincibilityUnits = new List<Unit>();
    [SerializeField] private List<Unit> healingUnits = new List<Unit>();
    [SerializeField] private List<Transform> teleportUnits = new List<Transform>();
    [SerializeField] private List<GameObject> ultimateList = new List<GameObject>();
    [SerializeField] private GameObject[] ultimates;
    public Enemy CurrentEnemy { get => currentEnemy; set => currentEnemy = value; }
    public bool IsLeft { get => isLeft; set => isLeft = value; }
    public bool IsRight { get => isRight; set => isRight = value; }
    public float CurrentHp { get => currentHp; set => currentHp = value; }
    public float CurrentResauce { get => currentResauce; set => currentResauce = value; }
    public float MaxResauce { get => maxResauce; set => maxResauce = value; }
    public float CurrentMp { get => currentMp; set => currentMp = value; }
    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float MaxMp { get => maxMp; set => maxMp = value; }
    public Boss Boss { get => boss; set => boss = value; }
    public List<Unit> SpeedUpUnits { get => speedUpUnits; set => speedUpUnits = value; }
    public List<Unit> InvincibilityUnits { get => invincibilityUnits; set => invincibilityUnits = value; }
    public List<Unit> HealingUnits { get => healingUnits; set => healingUnits = value; }
    public List<Transform> TeleportUnits { get => teleportUnits; set => teleportUnits = value; }
    public List<GameObject> UltimateList { get => ultimateList; set => ultimateList = value; }
    public PlayerSkill[] PlayerSkills { get => playerSkills; set => playerSkills = value; }

    public void DecreaseHp(float damage)
    {
        if (isInvincibility)
        {
            return;
        }
        currentHp -= damage;
        InGameManager.Instance.UpdatePlayerHpMpUI(true);
    }
    public void DecreaseHpDot(int dotCount, float damage, float second)
    {
        StartCoroutine(Co_DotDamage(dotCount, damage, second));
    }
    public void IncreaseHp(float value)
    {
        currentHp += value;
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        InGameManager.Instance.UpdatePlayerHpMpUI(true);
    }
    public void Stun(float second)
    {
        isStun = true;
        Invoke("Invoke_WakeUp", second);
    }
    public void UseSkill(int index)
    {
        if (!isStun && CheckMp(index))
        {
            DecreaseMp(index);
            //받아온 인덱스로 스킬 배열 참조해서 종류랑 값 로직에 적용
            switch (playerSkills[index].SkillKind)
            {
                case ESkillKind.Attack:
                    Skill_Attack(index);
                    break;
                case ESkillKind.Ultimate:
                    StartCoroutine(Skill_Ultimate(index));
                    break;
                case ESkillKind.SpeedUp:
                    StartCoroutine(Skill_SpeedUp(index));
                    break;
                case ESkillKind.Resauce:
                    Skill_Resauce(index);
                    break;
                case ESkillKind.Shield:
                    Skill_Shield(index);
                    break;
                case ESkillKind.Healing:
                    StartCoroutine(Skill_Healing(index));
                    break;
                case ESkillKind.Teleport:
                    StartCoroutine(Skill_Teleport(index));
                    break;
                case ESkillKind.Invincibility:
                    StartCoroutine(Skill_Invincibility(index));
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }
    }
    private void MoveLeft()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        moveDirection = Vector3.left;
        transform.position += moveDirection * currentMoveSpeed * Time.deltaTime;
    }
    private void MoveRight()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        moveDirection = Vector3.right;
        transform.position += moveDirection * currentMoveSpeed * Time.deltaTime;
    }
    private bool CheckMp(int index)
    {
        return currentMp >= playerSkills[index].Mp ? true : false;
    }
    private void DecreaseMp(int index)
    {
        currentMp -= playerSkills[index].Mp;
        InGameManager.Instance.UpdatePlayerHpMpUI(false);
    }
    private void Skill_Attack(int index)
    {
        if (currentEnemy != null && boss == null)
        {
            currentEnemy.DecreaseHp(playerSkills[index].Value);
            Debug.Log("때림");
        }
        else if (boss != null && currentEnemy == null)
        {
            boss.DecreaseHp(playerSkills[index].Value);
            Debug.Log("때림");
        }
        else if (boss != null && currentEnemy != null)
        {
            boss.DecreaseHp(playerSkills[index].Value);
            Debug.Log("때림");
        }
        else if (boss == null && currentEnemy == null)
        {
            Debug.Log("헛스윙");
        }
    }
    private void AttackUltimate(int index)
    {
        foreach (var item in ultimateList)
        {
            if (item.GetComponent<Unit>())
            {
                item.GetComponent<Unit>().DecreaseHp(playerSkills[index].Value + attackPower);
            }
            else if (item.GetComponent<Player>())
            {
                item.GetComponent<Player>().DecreaseHp(playerSkills[index].Value + attackPower);
            }
        }
        ResetUltimate();
    }
    private void ResetUltimate()
    {
        foreach (var item in ultimates)
        {
            item.transform.GetChild(0).GetComponent<Transform>().localScale = Vector2.zero;
            item.SetActive(false);
        }
        ultimateList.Clear();
    }
    private void Skill_Resauce(int index)
    {
        currentResauce += playerSkills[index].Value;
    }
    private void Skill_Shield(int index)
    {
        var obj = Instantiate(shield, new Vector3(transform.position.x + 4, transform.position.y, 0), Quaternion.identity);
        obj.SetActive(false);
        obj.transform.SetParent(GameObject.Find("UnitParent").transform);
        obj.GetComponent<Unit>().MaxHp = playerSkills[index].Value;
        obj.SetActive(true);
    }
    private void InitializeSkill() //나중에 player DB 완성되면 수정
    {
        foreach (var item in playerSkills)
        {
            if(item.SkillKind == ESkillKind.Attack)
            {
                item.Value = attackPower;
            }
            else if(item.SkillKind == ESkillKind.Ultimate)
            {
                item.Value += attackPower;
            }
        }
    }
    private void Invoke_WakeUp()
    {
        isStun = false;
    }
    private IEnumerator Skill_Ultimate(int index)
    {
        //DoSomething
        foreach (var item in ultimates)
        {
            item.SetActive(true);
            Vector3 rand = new Vector3
                (Random.Range(InGameManager.Instance.PlayerSpawnPoint.position.x, InGameManager.Instance.BossSpawnPoint.position.x),
                 0.25f, transform.position.z);
            item.transform.position = rand;
            item.transform.GetChild(0).GetComponent<Transform>().DOScale(1, 2);
            item.transform.SetParent(GameObject.Find("Field").transform);
        }
        yield return new WaitForSeconds(2.1f);
        AttackUltimate(index);
    }
    private IEnumerator Skill_SpeedUp(int index)
    {
        speedUpRange.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        speedUpRange.SetActive(false);
        foreach (var item in speedUpUnits)
        {
            item.CurrentMoveSpeed += item.MoveSpeed / 10 * playerSkills[index].Value;
            item.CurrentAttackSpeed -= item.AttackSpeed / 10 * playerSkills[index].Value;
        }
        currentMoveSpeed += 1 / playerSkills[index].Value;
        yield return new WaitForSeconds(5f);
        foreach (var item in speedUpUnits)
        {
            item.CurrentMoveSpeed = item.MoveSpeed;
            item.CurrentAttackSpeed = item.AttackSpeed;
        }
        currentMoveSpeed = moveSpeed;
        speedUpUnits.Clear();
    }
    private IEnumerator Skill_Healing(int index)
    {
        healingRange.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        healingRange.SetActive(false);
        foreach (var item in healingUnits)
        {
            item.IncreaseHp(playerSkills[index].Value);
        }
        IncreaseHp(playerSkills[index].Value);
        healingUnits.Clear();
    }
    private IEnumerator Skill_Teleport(int index)
    {
        teleportRange.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        teleportRange.SetActive(false);
        foreach (var item in teleportUnits)
        {
            item.position = new Vector3(transform.position.x, item.position.y);
        }
        teleportUnits.Clear();
    }
    private IEnumerator Skill_Invincibility(int index)
    {
        invincibilityRange.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        invincibilityRange.SetActive(false);
        foreach (var item in invincibilityUnits)
        {
            item.IsInvincibility = true;
        }
        isInvincibility = true;
        yield return new WaitForSeconds(playerSkills[index].Value);
        foreach (var item in invincibilityUnits)
        {
            item.IsInvincibility = false;
        }
        isInvincibility = false;
        invincibilityUnits.Clear();
    }
    private IEnumerator Co_UpdateResauceInternal()
    {
        while (true)
        {
            if(currentResauce < maxResauce)
            {
                currentResauce += 0.3f * Time.deltaTime;
            }
            else if (currentResauce > maxResauce)
            {
                currentResauce = maxResauce;
            }
            yield return null;
        }
    }
    private IEnumerator Co_DotDamage(int dotCount, float damage, float second)
    {
        while (dotCount >= 0)
        {
            DecreaseHp(damage);
            yield return new WaitForSeconds(second);
            dotCount--;
        }
    }
    private void Awake()
    {
        currentHp = maxHp;
        currentMp = maxMp;
        currentMoveSpeed = moveSpeed;
        speedUpRange = transform.Find("SpeedUpRange").gameObject;
        invincibilityRange = transform.Find("InvincibilityRange").gameObject;
        healingRange = transform.Find("HealingRange").gameObject;
        teleportRange = transform.Find("TeleportRange").gameObject;
    }
    private void Start()
    {
        StartCoroutine(Co_UpdateResauceInternal());
    }
    private void FixedUpdate()
    {
        if (isStun)
        {
            return;
        }
        if (isLeft == true)
        {
            MoveLeft();
        }
        if (isRight == true && !currentEnemy && !boss)
        {
            MoveRight();
        }
    }
}
