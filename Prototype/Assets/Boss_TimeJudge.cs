using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Boss_TimeJudge : MonoBehaviour
{
    private Camera camera;
    [SerializeField] private Transform field;
    private void Awake()
    {
        camera = Camera.main;
    }
    private IEnumerator Start()
    {
        camera.DOOrthoSize(7.2f, 3f);
        camera.transform.DOMoveY(-2.6f, 3f);
        field.DOMove(new Vector3(0, -2.7f, 0),3f);
        field.DOScaleY(1.1f, 3f);
        yield return new WaitForSeconds(3f);
        //field.DOColor(Color.clear, 2f);
    }
    void Update()
    {
        
    }
}
