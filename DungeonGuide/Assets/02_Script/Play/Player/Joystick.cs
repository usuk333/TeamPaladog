using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform m_rectBack;
    private RectTransform m_rectJoystick;
    private float m_fRadius;
    void Start()
    {
        m_rectBack = GetComponent<RectTransform>();
        m_rectJoystick = transform.Find("Knob").GetComponent<RectTransform>();
        m_fRadius = m_rectBack.rect.width * 0.5f;
    }
    private void OnTouch(Vector2 vecTouch)
    {
        Vector2 vec = new Vector2(vecTouch.x - m_rectBack.position.x, 0);
        // vec���� m_fRadius �̻��� ���� �ʵ��� �մϴ�.
        vec = Vector2.ClampMagnitude(vec, m_fRadius);
        m_rectJoystick.localPosition = vec;

        if (InGameManager.Instance.Player.Dead || InGameManager.Instance.Player.useSkill) return;
        // ��ġ��ġ ����ȭ
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
        OnTouch(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // ���� ��ġ�� �ǵ����ϴ�.
        m_rectJoystick.localPosition = Vector2.zero;
        if (InGameManager.Instance.Player.Dead || InGameManager.Instance.Player.useSkill)
        {
            return;
        }
        InGameManager.Instance.Player.IsLeft = false;
        InGameManager.Instance.Player.IsRight = false;
        InGameManager.Instance.Player.SetState(0);
    }
}
