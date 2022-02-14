using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float CASTING_TIME = 2f;
    private bool isRight; //������ �̵� üũ
    private bool isLeft; // ���� �̵� üũ
    private Boss boss;
    private bool isCast = false;
    public bool isCastFinish { get; set; } = false;
    private PlayerUI playerUI;
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float maxMp;
    [SerializeField] private float currentMp;
    [SerializeField] private float mpRegenerative;
    [SerializeField] private float mpRegenerationTime;
    [SerializeField] private float moveSpeed; //�̵� �ӵ�
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

    public void Move(Vector3 direction) // �̵� �Լ� (�Ű������� �̵��� ���� ���͸� ����)
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
    public void DecreaseHp(float damage) // ü�� ���� �Լ� (�����ϴ� �ʿ��� ȣ��)
    {
        currentHp -= damage;
    }
    private IEnumerator Co_UpdateMp() // ���� ���� �ڷ�ƾ
    {
        while (true)
        {
            if(currentMp < maxMp)
            {
                currentMp += mpRegenerative;
                yield return new WaitForSeconds(mpRegenerationTime);
            }
            yield return null;
        }
    }
    private void Awake() // �̴ϼȶ���¡���� ������ 22.02.08
    {
        playerUI = FindObjectOfType<PlayerUI>();
        boss = FindObjectOfType<Boss>();
        currentHp = maxHp;
        currentMp = maxMp;
    }
    private void Start()
    {
        StartCoroutine(Co_UpdateMp());
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
    }
    private void FixedUpdate()
    {
        if (isRight)
        {
            Move(Vector3.right);
        }
        if (isLeft)
        {
            Move(Vector3.left);
        }
    }
   
}
