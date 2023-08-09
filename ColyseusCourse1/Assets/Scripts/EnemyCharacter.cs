using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{

    public Vector3 _targetPosition { get; private set; } = Vector3.zero;
    private float _velocityMagnitude = 0; // Длинна вектора velocity

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
}
