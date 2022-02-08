using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private enum EProjectileKind
    {
        Arrow,
        Bullet,
        FireBall
    }
    private Boss boss;
    private float attackDamage;
    [SerializeField] private float speed;
    [SerializeField] private EProjectileKind eProjectileKind;
    public void Initialze(Boss target, Transform parent, float damage) //유닛이 투사체를 풀에 요청한 경우에 초기화됨
    {
        boss = target;
        attackDamage = damage;
        transform.position = parent.position;
        transform.gameObject.SetActive(true);
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, boss.transform.position, speed * Time.deltaTime);
        //1. 백터 뺄셈을 통해 투사체가 보스 오브젝트를 바라보는 방향 벡터를 구함
        Vector2 direction = new Vector2(transform.position.x - boss.transform.position.x, transform.position.y - boss.transform.position.y);
        //2. Mathf.Atan2 함수를 통해 1에서 구한 방향벡터의 y, x값으로 라디안(호도) 값을 구한 후 해당 값에 Mathf.Rad2Deg 를 곱해 각도로 변경해줌
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //3. Quaternion.AngleAxis를 통해 z축을 기준으로 2에서 구한 각도값만큼 회전하는 회전값을 생성. 투사체의 rotation에 대입해줌
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    private void OnTriggerEnter2D(Collider2D collision) // 투사체가 보스랑 충돌하면 데미지 입힌 후 풀로 반환되도록 함
    {
        if(collision.tag == "BOSS")
        {
            boss.DecreaseHp(attackDamage);
            switch (eProjectileKind)
            {
                case EProjectileKind.Arrow:
                    UnitProjectilePool.ReturnProjectile(this.gameObject, UnitProjectilePool.Instance.ArrowQueue);
                    break;
                case EProjectileKind.Bullet:
                    UnitProjectilePool.ReturnProjectile(this.gameObject, UnitProjectilePool.Instance.BulletQueue);
                    break;
                case EProjectileKind.FireBall:
                    UnitProjectilePool.ReturnProjectile(this.gameObject, UnitProjectilePool.Instance.FireBallQueue);
                    break;
                default:
                    break;
            }
        }
    }
}
