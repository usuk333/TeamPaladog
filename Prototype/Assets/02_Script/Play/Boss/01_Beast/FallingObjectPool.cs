using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject fallingObject;
    [SerializeField] private float spawnCount;
    private Beast beast;
    private float spawnIntervalMin;
    private float spawnIntervalMax;
    private float damage;
    private Queue<GameObject> fallingQueue = new Queue<GameObject>();
    public void ReturnFallingObj(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.position = transform.position;
        obj.transform.SetParent(transform);
        fallingQueue.Enqueue(obj);
    }
    private void InitializeObject() //투사체 초기화. 각 투사체 큐에 생성된 투사체를 인큐. 맨 처음에 호출
    {
        for (int i = 0; i < 5; i++)
        {
            fallingQueue.Enqueue(CreateNewFallingObj());
        }
    }
    private GameObject CreateNewFallingObj() //투사체 오브젝트 생성
    {
        GameObject obj = Instantiate(fallingObject);
        obj.gameObject.SetActive(false);
        obj.transform.position = transform.position;
        obj.transform.SetParent(transform);
        obj.GetComponent<FallingObject>().Damage = damage;
        return obj;
    }
    private GameObject GetFallingObj()
    {
        if(fallingQueue.Count > 0)
        {
            var obj = fallingQueue.Dequeue();
            return obj;
        }
        else
        {
            var obj = CreateNewFallingObj();
            return obj;
        }
    }
    private IEnumerator Co_Falling()
    {
        while (true)
        {        
            for (int i = 0; i < spawnCount; i++)
            {
                float randPosX = Random.Range(-6, 6);
                transform.position = new Vector3(randPosX, transform.position.y);
                var obj = GetFallingObj();
                obj.SetActive(true);
            }
            yield return new WaitForSeconds(Random.Range(spawnIntervalMin,spawnIntervalMax));
        }
    }
    private void Awake()
    {
        beast = GetComponentInParent<Beast>();
    }
    private void Start()
    {
        spawnIntervalMin = beast.beastStatus.thirdPatternMinTime;
        spawnIntervalMax = beast.beastStatus.thirdPatternMaxTime;
        damage = beast.beastStatus.thirdPatternDamage;
        InitializeObject();
        StartCoroutine(Co_Falling());
    }

}
