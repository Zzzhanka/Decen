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

    public static PlayerController Instance { get; private set; }  // Статическая переменная для доступа к игроку

    private void Awake()
    {
        Instance = this;  // Инициализируем статическую переменную
    }

    void Update()
    {
        // Получаем значения осей джойстика
        _movement.x = _joystick.Horizontal;
        _movement.y = _joystick.Vertical;

        // Если есть движение, включаем анимацию
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
        // Двигаем персонажа с заданной скоростью
        _rigidbody2D.velocity = _movement * _moveSpeed;
    }
}
