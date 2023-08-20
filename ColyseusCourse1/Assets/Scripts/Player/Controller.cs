using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private float _restartDelay = 2f;
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private PlayerGun _gun;
    [SerializeField] private float _mouseSencetivity = 2f;
    private MultiplayerManager _multiplayerManager;
    private bool _gameHold = false;
    private bool _cursorVisible;


    private void Start()
    {
        _multiplayerManager = MultiplayerManager.Instance;
        _cursorVisible = true;
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _cursorVisible = !_cursorVisible;

            Cursor.lockState = _cursorVisible ? CursorLockMode.Locked : CursorLockMode.None;
        }
        
        if (_gameHold) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float mouseX = 0;
        float mouseY = 0;
        bool leftClickPressed = false;

        if (_cursorVisible)
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            leftClickPressed = Input.GetMouseButtonDown(0);
        }

        bool spacePressed = Input.GetKey(KeyCode.Space);

        

        _player.SetInput(h, v, mouseX * _mouseSencetivity);
        _player.RotateX(-1 * mouseY * _mouseSencetivity); // Инвертируем значение поворота

        if (spacePressed) _player.Jump();

        if (leftClickPressed && _gun.TryShoot(out ShootInfo shootInfo))
        {
            SendShootInfo(ref shootInfo);
        }

        SendMove();


    }

    private void SendShootInfo(ref ShootInfo shootInfo)
    {
        shootInfo.key = _multiplayerManager.GetClientSessionId();
        string json = JsonUtility.ToJson(shootInfo);

        _multiplayerManager.SendMessage("shoot", json);
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

        _multiplayerManager.SendMessage("move", data);
    }

    public void Restart(int spawnIndex)
    {
        //Получаем точку спавна, где нужно заспавнить игрока
        _multiplayerManager._spawnPoints.GetPoint(spawnIndex, out Vector3 position, out Vector3 rotation);

        StartCoroutine(Hold());

        _player.transform.position = position;

        rotation.x = 0;
        rotation.z = 0;
        _player.transform.localEulerAngles = rotation;
      
        _player.SetInput(0, 0, 0);

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"pX", position.x},
            {"pY", 0},
            {"pZ", position.z},
            {"vX", 0},
            {"vY", 0},
            {"vZ", 0},
            {"rX", 0},
            {"rY", rotation.y},
        };

        _multiplayerManager.SendMessage("move", data);
    }

    private IEnumerator Hold()
    {
        _gameHold = true;

        yield return new WaitForSecondsRealtime(_restartDelay);

        _gameHold = false;
    }
}

//Информация о выстреле, которую будем передавать на сервер
[Serializable]
public struct ShootInfo
{
    public string key;

    //Откуда выстрелили
    public float pX;
    public float pY;
    public float pZ;

    //Направление, куда выстрелили
    public float dX;
    public float dY;
    public float dZ;
}


