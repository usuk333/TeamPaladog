using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] private FallingObjectPool fallingObjectPool;
    [SerializeField] private float fallingSpeed;
    [SerializeField] private float damage;
    private void OnEnable()
    {
        fallingObjectPool = GetComponentInParent<FallingObjectPool>();
        transform.SetParent(null);
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
            collision.GetComponent<Unit>().CommonStatus.DecreaseHp(damage); // GetComponent랑 배열 탐색 중에 뭐가 더 비용이 많이 들지 모르겠음..
            /*for (int i = 0; i < InGameManager.Instance.Units.Length; i++)
            {
                if(collision.gameObject == InGameManager.Instance.Units[i].gameObject)
                {
                    InGameManager.Instance.Units[i].CommonStatus.DecreaseHp(damage);
                }
            }*/
            fallingObjectPool.ReturnFallingObj(gameObject);
        }
    }
}
