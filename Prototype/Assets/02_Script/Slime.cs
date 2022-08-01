using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using DG.Tweening;

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
    private Vector3 flip;
    private SkeletonAnimation skeletonAnimation;
    [SerializeField] public bool isInvincible = true; //�������� ���� ���� ����. true�� ������. ���� ĳ������ ���� n�ʰ� false�� ��
    [SerializeField] private ParticleSystem poisonEffect;
    private MeshRenderer mesh;
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
    private void Flip(bool isLeft)
    {
        Vector3 scale = flip;
        if (isLeft)
        {
            scale.x *= -1;
        }
        transform.localScale = scale;
    }
    private void ExplosionSlime() // �������� ���� ������ �Լ�
    {
        poisonEffect.Play();
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
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        flip = transform.localScale;
        mesh = GetComponent<MeshRenderer>();
        
    }
    private void OnEnable() //Ǯ�� ���ư� �������� �ٽ� �������� �ڷ�ƾ�� ���������. ������ ������ ��ü�� Ȱ��ȭ �� ������ �ڷ�ƾ�� ���������.
    {
        skeletonAnimation.Skeleton.SetColor(Color.white);
        mesh.enabled = true;
        StartCoroutine(MoveAI());
        StartCoroutine(Co_ExplosionSlime());
        skeletonAnimation.AnimationState.SetAnimation(0, "Moving", true);
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
                nextBehaviour = Random.Range(0, 4);
                Debug.Log(nextBehaviour);
                switch (nextBehaviour)
                {
                    case 0:
                        direction = Vector3.left;
                        Debug.Log("���� �̵�");
                        Flip(false);
                        break;
                    case 1:
                        direction = Vector3.zero;
                        Debug.Log("����");
                        break;
                    case 2:
                        direction = Vector3.right;
                        Debug.Log("������ �̵�");
                        Flip(true);
                        break;
                    case 3:
                        direction = Vector3.zero;
                        skeletonAnimation.Skeleton.SetColor(Color.red);
                        yield return new WaitForSeconds(2f);
                        ExplosionSlime();
                        Debug.Log("����");
                        mesh.enabled = false;
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
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
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
