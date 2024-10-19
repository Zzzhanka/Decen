using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _moveSpeed = 5f;

    private Vector2 _movement;

    public static PlayerController Instance { get; private set; }  // ����������� ���������� ��� ������� � ������

    private void Awake()
    {
        Instance = this;  // �������������� ����������� ����������
    }

    void Update()
    {
        // �������� �������� ���� ���������
        _movement.x = _joystick.Horizontal;
        _movement.y = _joystick.Vertical;

        // ���� ���� ��������, �������� ��������
        if (_movement != Vector2.zero)
        {
            _animator.SetBool("isMoving", true);
        }
        else
        {
            _animator.SetBool("isMoving", false);
        }
    }

    private void FixedUpdate()
    {
        // ������� ��������� � �������� ���������
        _rigidbody2D.velocity = _movement * _moveSpeed;
    }
}
