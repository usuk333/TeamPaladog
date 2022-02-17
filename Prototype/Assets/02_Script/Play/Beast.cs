using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beast : MonoBehaviour
{
    [SerializeField] private float shoutDamage;
    [SerializeField] private Transform knockBackPoint;
    [SerializeField] private GameObject crush;
    private Player player;
    private Unit[] units;
    private List<GameObject> crushCollisions = new List<GameObject>();
    private void Shouting()
    {
        Vector3 pos = new Vector3(knockBackPoint.position.x, -1.735f);
        foreach (var item in units)
        {
            item.KnockBack(pos);
            item.DecreaseHp(shoutDamage);
        }
        player.KnockBack(pos);
        StartCoroutine(Co_KnockBackCrush());
    }
    private IEnumerator Co_KnockBackCrush()
    {
        crush.SetActive(true);
        yield return null;
    }
    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }
    private void Start()
    {
        units = InGameManager.Instance.Units;
        Shouting();
    }

}
