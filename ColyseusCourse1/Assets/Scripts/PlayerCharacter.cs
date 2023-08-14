using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacter : Character
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _cameraPoint;

    [SerializeField] private float _minHeadAngle = -90;
    [SerializeField] private float _maxHeadAngle = 80;
    
    [SerializeField] private float _jumpForce = 50f;
    [SerializeField] private float _jumpDelay = 0.2f;
    [SerializeField] private CheckGrounded _checkGrounded;
    private float _inputV;
    private float _inputH;
    private float _rotateY; // ������� ������ ������������ ���. �� ���� ���� ������� �����
    private float _currentRotateX = -90; // ������� ������ ������������ ���. �� ���� ���� ������� �����
    private float _jumpTime;

    private void Start()
    {
        Transform camera = Camera.main.transform;
        camera.parent = _cameraPoint;
        camera.localRotation = Quaternion.identity;
        camera.localPosition = Vector3.zero;
    }

    public void SetInput(float h, float v, float rotateY)
    {
        _inputH = h;
        _inputV = v;
        _rotateY += rotateY;
    }

    private void FixedUpdate()
    {
        Move();
        RotateY();
    }

    private void Move()
    {
        Vector3 velocity = (transform.forward * _inputV + transform.right * _inputH).normalized * speed;

        velocity.y = _rigidbody.velocity.y; //����� �� ��������� ������ veloicty Y = 0. ���� ������ ��� ����������
        base.velocity = velocity;

        _rigidbody.velocity = base.velocity;
    }


    public void Jump()
    {
        if (_checkGrounded.IsGrounded == false) return;

        if (Time.time - _jumpTime < _jumpDelay) return; // ���� ������� � ������� ����� update > jumpDelay, �� �� ��� �������

        _jumpTime = Time.time;
        _rigidbody.AddForce(0, _jumpForce, 0, ForceMode.VelocityChange);
    }

    private void RotateY()
    {
        _rigidbody.angularVelocity = new Vector3(0, _rotateY, 0);

        _rotateY = 0;
    }

    public void RotateX(float value)
    {
        _currentRotateX = Mathf.Clamp(_currentRotateX + value, _minHeadAngle, _maxHeadAngle);

        _head.localEulerAngles = new Vector3(_currentRotateX, 0, 0);
    }

    public void GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY)
    {
        position = transform.position;
        velocity = _rigidbody.velocity;
        rotateY = transform.eulerAngles.y;
        rotateX = _head.transform.localEulerAngles.x;

    }
}
