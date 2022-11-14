using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using DG.Tweening;

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
    private Vector3 flip;
    private SkeletonAnimation skeletonAnimation;
    [SerializeField] public bool isInvincible = true; //슬라임의 무적 상태 변수. true면 무적임. 제단 캐스팅을 통해 n초간 false가 됨
    [SerializeField] private ParticleSystem poisonEffect;
    private MeshRenderer mesh;
    private IEnumerator Co_ExplosionSlime() // 슬라임이 폭발한 후 도트피해를 입히고 분열하는 기능 함수
    {
        while (true)
        {
            yield return null;
            if (isExplosion)
            {
                StartCoroutine(Co_SetColor(2f, Color.white, Color.red));
                yield return new WaitForSeconds(2f);
                mesh.enabled = false;
                poisonEffect.Play();
                if (collisionObjects.Contains(InGameManager.Instance.Player.gameObject))
                {
                    InGameManager.Instance.Player.DecreaseHp(mushroom.mushroomStatus.thirdPatternDamage);
                }
                foreach (var item in InGameManager.Instance.Units)
                {
                    if (collisionObjects.Contains(item.gameObject))
                    {
                        item.CommonStatus.DecreaseHp(mushroom.mushroomStatus.thirdPatternDamage);
                    }
                }
                yield return new WaitForSeconds(2f);
                for (int i = 0; i < mushroom.mushroomStatus.thirdPatternCount; i++)
                {
                    mushroom.SummonSlime(transform.localPosition);
                }
                ReturnThis();
            }
        }        
    }
    private IEnumerator Co_SetColor(float time, Color a, Color b)
    {
        float progress = 0;
        while (progress < time)
        {
            skeletonAnimation.skeleton.SetColor(Color.Lerp(a, b, 1 / time * progress));
            progress += Time.deltaTime;
            yield return null;//new WaitForSeconds(Time.deltaTime);
        }
    }
    private void ReturnThis()
    {
        isExplosion = false;
        if (mushroom.isCounting)
        {
            mesh.enabled = true;
            StartCoroutine(Co_SetColor(mushroom.progress, Color.white, Color.red));
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
    private void Awake()
    {
        mushroom = GetComponentInParent<Mushroom>();
        mushroom.dReturnAllSlime += new Mushroom.ReturnAllSlime(ReturnThis);
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        flip = transform.localScale;
        mesh = GetComponent<MeshRenderer>();
    }
    private void OnEnable() //풀로 돌아간 슬라임을 다시 꺼내오면 코루틴이 멈춰버린다. 때문에 슬라임 객체가 활성화 될 때마다 코루틴을 실행해줬다.
    {
        StartCoroutine(Co_SetColor(1f, Color.clear, Color.white));
        mesh.enabled = true;
        StartCoroutine(MoveAI());
        StartCoroutine(Co_ExplosionSlime());
        skeletonAnimation.AnimationState.SetAnimation(0, "Moving", true);
    }
    private void FixedUpdate()
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
    private IEnumerator MoveAI() //슬라임의 행동 선택 코루틴. 1/4 확률로 '왼쪽 이동, 멈춤, 오른쪽 이동, 자폭' 의 행동을 취함
    {
        while (true)
        {
            yield return null;
            if (mushroom.isCounting)
            {
                direction = Vector3.zero;
                mesh.enabled = true;
                StartCoroutine(Co_SetColor(mushroom.SporeTimerMaxValue, Color.white, Color.red));
                yield break;
            }
            nextBehaviour = Random.Range(0, 4);
            Debug.Log(nextBehaviour);
            switch (nextBehaviour)
            {
                case 0:
                    direction = Vector3.left;
                    Debug.Log("왼쪽 이동");
                    Flip(false);
                    break;
                case 1:
                    direction = Vector3.zero;
                    Debug.Log("멈춤");
                    break;
                case 2:
                    direction = Vector3.right;
                    Debug.Log("오른쪽 이동");
                    Flip(true);
                    break;
                case 3:
                    direction = Vector3.zero;
                    yield return new WaitForSeconds(2f);
                    if (mushroom.isCounting)
                    {
                        continue;
                    }
                    isExplosion = true;
                    yield break;
                default:
                    break;
            }
            moveInterval = Random.Range(1f, 4f);//1초에서 3초 사이의 시간이 나옴
            yield return new WaitForSeconds(moveInterval);
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
