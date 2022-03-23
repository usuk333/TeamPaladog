using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] private List<GameObject> collisionObjects = new List<GameObject>(); //슬라임의 폭발 범위 내의 객체를 담을 리스트
    private bool isExplosion = false;
    private float nextBehaviour;
    private Vector3 direction;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float dotDamage;
    [SerializeField] private float dotInterval;
    [SerializeField] private int dotCount;
    private Mushroom mushroom;
    private float moveInterval;
    [SerializeField] public bool isInvincible = true; //슬라임의 무적 상태 변수. true면 무적임. 제단 캐스팅을 통해 n초간 false가 됨
    private IEnumerator Co_ExplosionSlime() // 슬라임이 폭발한 후 도트피해를 입히고 분혈하는 기능 함수
    {
        while (true)
        {
            if (isExplosion)
            {
                Debug.Log("도트 코루틴 들어옴");
                //mushroom.SummonSlime(transform.position);
                if (collisionObjects.Contains(InGameManager.Instance.Player.gameObject))
                {
                    InGameManager.Instance.Player.DecreaseHp(dotDamage);
                }
                foreach (var item in InGameManager.Instance.Units)
                {
                    if (collisionObjects.Contains(item.gameObject))
                    {
                        item.CommonStatus.DecreaseHp(dotDamage);
                    }
                }
                dotCount++;
                yield return new WaitForSeconds(dotInterval);
                if(dotCount >= 3)
                {
                    isExplosion = false;
                    dotCount = 0;
                    mushroom.SummonSlime(transform.localPosition);
                    mushroom.SummonSlime(transform.localPosition);
                    ReturnThis();
                }
            }
            yield return null;
        }        
    }
    private void ReturnThis()
    {
        if (mushroom.isCounting)
        {
            return;
        }
        mushroom.ReturnSlime(this);
    }
    private void ExplosionSlime() // 슬라임의 폭발 데미지 함수
    {
        if (collisionObjects.Contains(InGameManager.Instance.Player.gameObject))
        {
            InGameManager.Instance.Player.DecreaseHp(damage);
        }
        foreach (var item in InGameManager.Instance.Units)
        {
            if(collisionObjects.Contains(item.gameObject))
            {
                item.CommonStatus.DecreaseHp(damage);
            }
        }
        isExplosion = true;
        //transform.gameObject.SetActive(false);
    }
    private void Awake()
    {
        mushroom = GetComponentInParent<Mushroom>();
        mushroom.dReturnAllSlime += new Mushroom.ReturnAllSlime(ReturnThis);
    }
    private void OnEnable() //풀로 돌아간 슬라임을 다시 꺼내오면 코루틴이 멈춰버린다. 때문에 슬라임 객체가 활성화 될 때마다 코루틴을 실행해줬다.
    {
        StartCoroutine(MoveAI());
        StartCoroutine(Co_ExplosionSlime());
    }
    private void FixedUpdate()
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
    private IEnumerator MoveAI() //슬라임의 행동 선택 코루틴. 1/4 확률로 '왼쪽 이동, 멈춤, 오른쪽 이동, 자폭' 의 행동을 취함
    {
        while (true)
        {
            if (!isExplosion)
            {
                /*nextBehaviour = (float)Random.Range(0f, 1f) * 100;
                Debug.Log(nextBehaviour);
                if (nextBehaviour <= 30)
                {
                    direction = Vector3.left;
                    Debug.Log("왼쪽 이동");
                }
                else if (nextBehaviour > 30 && nextBehaviour <= 60)
                {
                    direction = Vector3.zero;
                    Debug.Log("멈춤");
                }
                else if (nextBehaviour > 60 && nextBehaviour <= 90)
                {
                    direction = Vector3.right;
                    Debug.Log("오른쪽 이동");
                }
                else if (nextBehaviour > 90)
                {
                    direction = Vector3.zero;
                    ExplosionSlime();
                    Debug.Log("폭발");
                }*/
                nextBehaviour = Random.Range(0, 4);
                Debug.Log(nextBehaviour);
                switch (nextBehaviour)
                {
                    case 0:
                        direction = Vector3.left;
                        Debug.Log("왼쪽 이동");
                        break;
                    case 1:
                        direction = Vector3.zero;
                        Debug.Log("멈춤");
                        break;
                    case 2:
                        direction = Vector3.right;
                        Debug.Log("오른쪽 이동");
                        break;
                    case 3:
                        direction = Vector3.zero;
                        ExplosionSlime();
                        Debug.Log("폭발");
                        break;
                    default:
                        break;
                }
                moveInterval = Random.Range(1f, 4f);//1초에서 3초 사이의 시간이 나옴
                yield return new WaitForSeconds(moveInterval);
            }
            yield return null;
        }      
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PLAYER" || collision.tag == "UNIT")
        {
            if (!collisionObjects.Contains(collision.gameObject))
            {
                collisionObjects.Add(collision.gameObject);
            }
        }
        if(collision.tag == "WALL")
        {
            direction = direction * (-1);
        }
        if (!isInvincible)
        {
            if(collision.tag == "PLAYER")
            {
                mushroom.ReturnSlime(this);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PLAYER" || collision.tag == "UNIT")
        {
            if (collisionObjects.Contains(collision.gameObject))
            {
                collisionObjects.Remove(collision.gameObject);
            }
        }
    }
}
