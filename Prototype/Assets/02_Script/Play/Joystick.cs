using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    RectTransform m_rectBack;
    RectTransform m_rectJoystick;

    //Transform m_trCube;
    float m_fRadius;
    float m_fSpeed = 5.0f;
    float m_fSqr = 0f;

    Vector3 m_vecMove;

    Vector2 m_vecNormal;

    bool m_bTouch = false;


    void Start()
    {
        m_rectBack = GetComponent<RectTransform>();
        m_rectJoystick = transform.Find("Knob").GetComponent<RectTransform>();

       // m_trCube = GameObject.Find("Cube").transform;

        // JoystickBackground의 반지름입니다.
        m_fRadius = m_rectBack.rect.width * 0.5f;
    }

    void Update()
    {
        if (m_bTouch)
        {
            InGameManager.Instance.Player.transform.position += m_vecMove;
        }

    }

    void OnTouch(Vector2 vecTouch)
    {
        Vector2 vec = new Vector2(vecTouch.x - m_rectBack.position.x, 0);
        // vec값을 m_fRadius 이상이 되지 않도록 합니다.
        vec = Vector2.ClampMagnitude(vec, m_fRadius);
        m_rectJoystick.localPosition = vec;

        // 터치위치 정규화
        Vector2 vecNormal = vec.normalized;
        if(vecNormal.x > 0)
        {
            InGameManager.Instance.Player.IsLeft = false;
            InGameManager.Instance.Player.IsRight = true;
            InGameManager.Instance.Player.Flip(false);
        }
        else if(vecNormal.x < 0)
        {
            InGameManager.Instance.Player.IsRight = false;
            InGameManager.Instance.Player.IsLeft = true;
            InGameManager.Instance.Player.Flip(true);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnTouch(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        InGameManager.Instance.Player.PlayerAnimation(0);
        OnTouch(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 원래 위치로 되돌립니다.
        m_rectJoystick.localPosition = Vector2.zero;
        InGameManager.Instance.Player.IsLeft = false;
        InGameManager.Instance.Player.IsRight = false;
    }
}
