using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool isRight; //오른쪽 이동 체크
    private bool isLeft; // 왼쪽 이동 체크
    private Boss boss;
    private bool isCast = false;
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float maxMp;
    [SerializeField] private float currentMp;
    [SerializeField] private float mpRegenerative;
    [SerializeField] private float mpRegenerationTime;
    [SerializeField] private float moveSpeed; //이동 속도

    public bool IsCast
    {
        get
        {
            return isCast;
        }
        set
        {
            isCast = value;

        }
    }
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

    public void Move(Vector3 direction) // 이동 함수 (매개변수로 이동할 방향 벡터를 받음)
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
    public void DecreaseHp(float damage) // 체력 감소 함수 (공격하는 쪽에서 호출)
    {
        currentHp -= damage;
    }
    private IEnumerator Co_UpdateMp() // 마나 갱신 코루틴
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
    private void Awake() // 이니셜라이징으로 뺄거임 22.02.08
    {
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
        if (isCast)
        {

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
