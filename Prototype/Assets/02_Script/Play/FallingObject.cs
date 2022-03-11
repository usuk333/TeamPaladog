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
            collision.GetComponent<Player>().DecreaseHp(damage);
            fallingObjectPool.ReturnFallingObj(gameObject);
        }
        else if(collision.tag == "UNIT")
        {
            collision.GetComponent<Unit>().CommonStatus.DecreaseHp(damage);
            fallingObjectPool.ReturnFallingObj(gameObject);
        }
    }
}
