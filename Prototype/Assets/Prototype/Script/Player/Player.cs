using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private InGameManager inGameManager;

    private float maxResauce = 25f;
    [SerializeField] private float currentResauce = 0f;
    [SerializeField] private float resauceChargingTime;

    private Vector3 MoveDirection = Vector3.zero;
    private Rigidbody2D rigid;

    [SerializeField] private float maxHp;
    [SerializeField] private float maxMp;
    [SerializeField] private float currentHp;
    [SerializeField] private float currentMp;


    [SerializeField] private PlayerSkill[] playerSkills;

    private bool bleft = false;
    private bool bright = false;

    [SerializeField] private Transform startPoint;

    [SerializeField] private Enemy currentEnemy;

    public Enemy CurrentEnemy { get => currentEnemy; set => currentEnemy = value; }

    [SerializeField] private float moveSpeed;
    public bool isLeft { get => bleft; set => bleft = value; }
    public bool isRight { get => bright; set => bright = value; }
    public float CurrentHp { get => currentHp; set => currentHp = value; }
    public float CurrentResauce { get => currentResauce; set => currentResauce = value; }
    public float MaxResauce { get => maxResauce; set => maxResauce = value; }
    public float CurrentMp { get => currentMp; set => currentMp = value; }
    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float MaxMp { get => maxMp; set => maxMp = value; }

    private void Awake()
    {
        inGameManager = FindObjectOfType<InGameManager>();
    }
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
        if (bleft == true)
        {
            MoveLeft();
        }
        if (bright == true && !currentEnemy)
        {
            MoveRight();
        }
    }
    public void MoveLeft()
    {
        MoveDirection = Vector3.left;
        transform.position += MoveDirection * moveSpeed * Time.deltaTime;       
    }
    public void MoveRight()
    {
        MoveDirection = Vector3.right;
        transform.position += MoveDirection * moveSpeed * Time.deltaTime;
    }
    public void UseSkill(int index)
    {
        //받아온 인덱스로 스킬 배열 참조해서 종류랑 값 로직에 적용
        switch (playerSkills[index].SkillKind)
        {
            case ESkillKind.Attack:
                currentMp -= playerSkills[index].Mp;
                inGameManager.DecreasePlayerUI(false);
                if(currentEnemy != null)
                {
                    currentEnemy.CurrentHp -= playerSkills[index].Value;
                    currentEnemy.GetComponentInChildren<HpBar>().DecreaseUnitOrEnemyHpUI(false);
                    Debug.Log("때림");
                }
                else
                {
                    Debug.Log("헛스윙");
                }
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
    private IEnumerator Co_UpdateResauceInternal()
    {
        while (true)
        {
            currentResauce += 0.3f * Time.deltaTime;
            yield return null;
        }
    }
}
