using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InGameScrollView : MonoBehaviour, IDragHandler
{
    [SerializeField] private Transform camera;

    [SerializeField] private float speed;
    private float startPosX;
    private float endPosX;

    // Start is called before the first frame update
    void Awake()
    {
        camera = GameObject.Find("Main Camera").transform;
        startPosX = 0f;
        endPosX = GameObject.Find("EndPoint").transform.position.x;
    }

    // Update is called once per frame
    void Update()
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
