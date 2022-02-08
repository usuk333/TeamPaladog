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
    public void Initialze(Boss target, Transform parent, float damage) //������ ����ü�� Ǯ�� ��û�� ��쿡 �ʱ�ȭ��
    {
        boss = target;
        attackDamage = damage;
        transform.position = parent.position;
        transform.gameObject.SetActive(true);
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, boss.transform.position, speed * Time.deltaTime);
        //1. ���� ������ ���� ����ü�� ���� ������Ʈ�� �ٶ󺸴� ���� ���͸� ����
        Vector2 direction = new Vector2(transform.position.x - boss.transform.position.x, transform.position.y - boss.transform.position.y);
        //2. Mathf.Atan2 �Լ��� ���� 1���� ���� ���⺤���� y, x������ ����(ȣ��) ���� ���� �� �ش� ���� Mathf.Rad2Deg �� ���� ������ ��������
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //3. Quaternion.AngleAxis�� ���� z���� �������� 2���� ���� ��������ŭ ȸ���ϴ� ȸ������ ����. ����ü�� rotation�� ��������
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    private void OnTriggerEnter2D(Collider2D collision) // ����ü�� ������ �浹�ϸ� ������ ���� �� Ǯ�� ��ȯ�ǵ��� ��
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
