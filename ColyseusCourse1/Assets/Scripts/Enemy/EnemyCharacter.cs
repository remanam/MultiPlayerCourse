using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    private string _sessionID;
    [SerializeField] private Transform _head;
    [SerializeField] private Health _health;

    public Vector3 _targetPosition { get; private set; } = Vector3.zero;
    private float _velocityMagnitude = 0; // Длинна вектора velocity


    public void Init(string sessionID)
    {
        _sessionID = sessionID;
    }

    private void Start()
    {
        _targetPosition = transform.position;
    }

    private void Update()
    {
        if (_velocityMagnitude > .1f)
        {

            float maxDistance = _velocityMagnitude * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, maxDistance);
        }
        else
        {
            transform.position = _targetPosition;
        }
    }

    public void SetSpeed(float value) => speed = value;

    public void SetMovement(in Vector3 position, in Vector3 velocity, in float avarageInterval)
    {
        _targetPosition = position + (velocity * avarageInterval);
        _velocityMagnitude = velocity.magnitude;

        this.velocity = velocity;
    }

    public void SetRotateX(float value)
    {
        _head.transform.localEulerAngles = new Vector3(value, 0, 0);
    }

    public void SetRotateY(float value)
    {
        transform.localEulerAngles = new Vector3(0, value, 0);
    }
    
    public void SetMaxHP(int value)
    {
        maxHealth = value;
        _health.SetMax(value);
        _health.SetCurrent(value);
    }

    public void RestoreHP(int value)
    {
        _health.SetCurrent(value);
    }

    public void ApplyDamage(int damage)
    {
        _health.ApplyDamage(damage);

        Dictionary<string, object> data = new Dictionary<string, object>() 
        {
            {"id", _sessionID},
            {"value", damage},
        };

        MultiplayerManager.Instance.SendMessage("damage", data);
    }
}
