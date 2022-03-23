using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] private List<GameObject> collisionObjects = new List<GameObject>(); //�������� ���� ���� ���� ��ü�� ���� ����Ʈ
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
    [SerializeField] public bool isInvincible = true; //�������� ���� ���� ����. true�� ������. ���� ĳ������ ���� n�ʰ� false�� ��
    private IEnumerator Co_ExplosionSlime() // �������� ������ �� ��Ʈ���ظ� ������ �����ϴ� ��� �Լ�
    {
        while (true)
        {
            if (isExplosion)
            {
                Debug.Log("��Ʈ �ڷ�ƾ ����");
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
    private void ExplosionSlime() // �������� ���� ������ �Լ�
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
    private void OnEnable() //Ǯ�� ���ư� �������� �ٽ� �������� �ڷ�ƾ�� ���������. ������ ������ ��ü�� Ȱ��ȭ �� ������ �ڷ�ƾ�� ���������.
    {
        StartCoroutine(MoveAI());
        StartCoroutine(Co_ExplosionSlime());
    }
    private void FixedUpdate()
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
    private IEnumerator MoveAI() //�������� �ൿ ���� �ڷ�ƾ. 1/4 Ȯ���� '���� �̵�, ����, ������ �̵�, ����' �� �ൿ�� ����
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
                    Debug.Log("���� �̵�");
                }
                else if (nextBehaviour > 30 && nextBehaviour <= 60)
                {
                    direction = Vector3.zero;
                    Debug.Log("����");
                }
                else if (nextBehaviour > 60 && nextBehaviour <= 90)
                {
                    direction = Vector3.right;
                    Debug.Log("������ �̵�");
                }
                else if (nextBehaviour > 90)
                {
                    direction = Vector3.zero;
                    ExplosionSlime();
                    Debug.Log("����");
                }*/
                nextBehaviour = Random.Range(0, 4);
                Debug.Log(nextBehaviour);
                switch (nextBehaviour)
                {
                    case 0:
                        direction = Vector3.left;
                        Debug.Log("���� �̵�");
                        break;
                    case 1:
                        direction = Vector3.zero;
                        Debug.Log("����");
                        break;
                    case 2:
                        direction = Vector3.right;
                        Debug.Log("������ �̵�");
                        break;
                    case 3:
                        direction = Vector3.zero;
                        ExplosionSlime();
                        Debug.Log("����");
                        break;
                    default:
                        break;
                }
                moveInterval = Random.Range(1f, 4f);//1�ʿ��� 3�� ������ �ð��� ����
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
