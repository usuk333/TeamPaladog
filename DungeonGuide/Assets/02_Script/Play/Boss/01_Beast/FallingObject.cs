using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] private FallingObjectPool fallingObjectPool;
    [SerializeField] private float fallingSpeed;
    [SerializeField] private float damage;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Sprite[] spriteArray;

    public float Damage { get => damage; set => damage = value; }

    private void OnEnable()
    {
        fallingObjectPool = GetComponentInParent<FallingObjectPool>();
        transform.SetParent(null);
        int rand = Random.Range(0, spriteArray.Length);
        sprite.sprite = spriteArray[rand];
    }
    private void Update()
    {
        if(transform.localPosition.y <= -10f)
        {
            fallingObjectPool.ReturnFallingObj(gameObject);
        }
    }
    private void LateUpdate()
    {
        transform.position += Vector3.down * fallingSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PLAYER")
        {
            InGameManager.Instance.Player.DecreaseHp(damage);
            fallingObjectPool.ReturnFallingObj(gameObject);
        }
        else if(collision.tag == "UNIT")
        {
            collision.GetComponent<Unit>().CommonStatus.DecreaseHp(damage); // GetComponent�� �迭 Ž�� �߿� ���� �� ����� ���� ���� �𸣰���..
            for (int i = 0; i < InGameManager.Instance.Units.Count; i++)
            {
                if(collision.gameObject == InGameManager.Instance.Units[i].gameObject)
                {
                    InGameManager.Instance.Units[i].CommonStatus.DecreaseHp(damage);
                }
            }
            fallingObjectPool.ReturnFallingObj(gameObject);
        }
    }
}