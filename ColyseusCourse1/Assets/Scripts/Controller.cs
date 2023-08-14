using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private PlayerGun _gun;
    [SerializeField] private float _mouseSencetivity = 2f;

    private float _inputH;
    private float _inputV;

    private float _mouseX;
    private float _mouseY;

    private void Update()
    {

        _inputH = Input.GetAxisRaw("Horizontal");
        _inputV = Input.GetAxisRaw("Vertical");

        _mouseX = Input.GetAxis("Mouse X");
        _mouseY = Input.GetAxis("Mouse Y");

        bool spacePressed = Input.GetKey(KeyCode.Space);

        bool leftClickPressed = Input.GetMouseButtonDown(0);

        _player.SetInput(_inputH, _inputV, _mouseX * _mouseSencetivity);
        _player.RotateX(-1 * _mouseY * _mouseSencetivity); // Инвертируем значение поворота

        if (spacePressed) _player.Jump();

        if (leftClickPressed) _gun.Shoot();

        SendMove();


    }

    private void SendMove()
    {
        _player.GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY);

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"pX", position.x},
            {"pY", position.y},
            {"pZ", position.z},
            {"vX", velocity.x},
            {"vY", velocity.y},
            {"vZ", velocity.z},
            {"rX", rotateX},
            {"rY", rotateY},
        };

        MultiplayerManager.Instance.SendMessage("move", data);
    }
}

