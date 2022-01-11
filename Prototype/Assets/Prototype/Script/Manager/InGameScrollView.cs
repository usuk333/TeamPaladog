using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InGameScrollView : MonoBehaviour, IDragHandler //인게임 뷰 횡스크롤 기능 스크립트
{
    private float startPosX;
    private float endPosX;
    private Transform camera;
    [SerializeField] private float speed;
    private void Awake()
    {
        startPosX = 0f;
        camera = GameObject.Find("Main Camera").transform;
        endPosX = GameObject.Find("EndPoint").transform.position.x;
    }
    private void FixedUpdate()
    {
        if(camera.position.x < startPosX)
        {
            camera.position = new Vector3(startPosX, camera.position.y, camera.position.z);
        }
        else if(camera.position.x > endPosX)
        {
            camera.position = new Vector3(endPosX, camera.position.y, camera.position.z);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if(eventData.delta.x > 0f)
        {
            camera.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }
        else
        {
            camera.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
    }
}
