using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool isInvincibility;
    private bool isInpacable;
    private bool isRight; //������ �̵� üũ
    private bool isLeft; // ���� �̵� üũ
    private Boss boss;
    public bool isCast { get; set; } = false;
    public bool isCastFinish { get; set; } = false;
    private PlayerUI playerUI;
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float maxMp;
    [SerializeField] private float currentMp;
    [SerializeField] private float mpRegenerative;
    [SerializeField] private float mpRegenerationTime;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float currentMoveSpeed; //�̵� �ӵ�
    private float castingTime;
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
        transform.position += direction * currentMoveSpeed * Time.deltaTime;
    }
    public void DecreaseHp(float damage) // ü�� ���� �Լ� (�����ϴ� �ʿ��� ȣ��)
    {
        if (isInvincibility)
        {
            return;
        }
        currentHp -= damage;
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

}
