using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float maxResauce = 25f;
    [SerializeField] private float currentResauce = 0f;
    [SerializeField] private float resauceChargingTime;

    private Vector3 moveDirection = Vector3.zero;

    [SerializeField] private float maxHp;
    [SerializeField] private float maxMp;
    [SerializeField] private float currentHp;
    [SerializeField] private float currentMp;


    [SerializeField] private PlayerSkill[] playerSkills;

    private bool isLeft = false;
    private bool isRight = false;

    [SerializeField] private Transform startPoint;

    [SerializeField] private Enemy currentEnemy;
    [SerializeField] private Boss boss;

    private bool isStun = false;

    public Enemy CurrentEnemy { get => currentEnemy; set => currentEnemy = value; }

    [SerializeField] private float moveSpeed;
    public bool IsLeft { get => isLeft; set => isLeft = value; }
    public bool IsRight { get => isRight; set => isRight = value; }
    public float CurrentHp { get => currentHp; set => currentHp = value; }
    public float CurrentResauce { get => currentResauce; set => currentResauce = value; }
    public float MaxResauce { get => maxResauce; set => maxResauce = value; }
    public float CurrentMp { get => currentMp; set => currentMp = value; }
    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float MaxMp { get => maxMp; set => maxMp = value; }
    public Boss Boss { get => boss; set => boss = value; }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPoint.position;
        StartCoroutine(Co_UpdateResauceInternal());
        currentHp = maxHp;
        currentMp = maxMp;
    }

    // Update is called once per frame
    void FixedUpdate()
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
    public void MoveLeft()
    {
        moveDirection = Vector3.left;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;       
    }
    public void MoveRight()
    {
        moveDirection = Vector3.right;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
    public void UseSkill(int index)
    {
        if (!isStun)
        {
            //받아온 인덱스로 스킬 배열 참조해서 종류랑 값 로직에 적용
            switch (playerSkills[index].SkillKind)
            {
                case ESkillKind.Attack:
                    Skill_Attack(index);
                    break;
                case ESkillKind.PowerUp:
                    break;
                case ESkillKind.Resource:
                    break;
                case ESkillKind.Shield:
                    break;
                case ESkillKind.SpeedUp:
                    break;
                case ESkillKind.Revival:
                    break;
                case ESkillKind.Invincibility:
                    break;
                default:
                    break;
            }
        }
       
    }
    private IEnumerator Co_UpdateResauceInternal()
    {
        while (true)
        {
            currentResauce += 0.3f * Time.deltaTime;
            yield return null;
        }
    }
    private void Skill_Attack(int index)
    {
        currentMp -= playerSkills[index].Mp;
        InGameManager.Instance.UpdatePlayerHpMpUI(false);
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
    public void DecreaseHp(float damage)
    {
        currentHp -= damage;
        InGameManager.Instance.UpdatePlayerHpMpUI(true);
    }
    public void IncreaseHp(float value)
    {
        currentHp += value;
        if(currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        InGameManager.Instance.UpdatePlayerHpMpUI(true);
    }
    public void Stun()
    {
        isStun = true;
        Invoke("Invoke_WakeUp", 2f);
    }
    private void Invoke_WakeUp()
    {
        isStun = false;
    }
    
}
