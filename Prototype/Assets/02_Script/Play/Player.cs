using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool isRight; //������ �̵� üũ
    private bool isLeft; // ���� �̵� üũ
    [SerializeField] private float moveSpeed; //�̵� �ӵ�
    private void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            isRight = true;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            isRight = false;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            isLeft = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            isLeft = false;
        }
    }
    private void FixedUpdate()
    {
        if (isRight)
        {
            Move(Vector3.right);
        }
        if (isLeft)
        {
            Move(Vector3.left);
        }
    }
    public void Move(Vector3 direction) // �̵� �Լ� (�Ű������� �̵��� ���� ���͸� ����)
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
