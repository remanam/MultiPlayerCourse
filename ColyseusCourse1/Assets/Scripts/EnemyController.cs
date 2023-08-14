using Colyseus.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyCharacter _enemyCharacter;
    private List<float> _lastFiveIntervals = new List<float> { 0, 0, 0, 0, 0 };

    private Player _player;

    public void Init(Player player)
    {
        _player = player;
        _enemyCharacter.SetSpeed(player.speed);

        player.OnChange += OnChange;
    }

    public void OnDestroy()
    {
        _player.OnChange -= OnChange;

        Destroy(gameObject);
    }

    private float AvarageTimeInterval
    {
        get
        {
            float summ = 0;
            int count = _lastFiveIntervals.Count;
            int intervalsCount = count;

            for (int i = 0; i < intervalsCount; i++)
            {
                summ += _lastFiveIntervals[i];
            }
            return summ / intervalsCount;
        }
    }


    private float _lastRecieveTime = 0f;

    private void SaveRecieveTime()
    {
        float interval = Time.time - _lastRecieveTime; // ¬рем€ пройденное с последнего обновлени€ времени
        _lastRecieveTime = Time.time; // ќбновл€ем последнее обновление времени

        _lastFiveIntervals.Add(interval);
        _lastFiveIntervals.Remove(0); // ¬ конец добавили, а в начале удалили
    }

    public void OnChange(List<DataChange> changes)
    {
        SaveRecieveTime(); // Time.deltatime но дл€ обновлений от сервера


        Vector3 position = _enemyCharacter._targetPosition;
        Vector3 velocity = _enemyCharacter.velocity;

        foreach (var dataChange in changes)
        {
            switch (dataChange.Field)
            {
                case "pX":
                    position.x = (float)dataChange.Value;
                    break;
                case "pY":
                    position.y = (float)dataChange.Value;
                    break;
                case "pZ":
                    position.z = (float)dataChange.Value;
                    break;
                case "vX":
                    velocity.x = (float)dataChange.Value;
                    break;
                case "vY":
                    velocity.y = (float)dataChange.Value;
                    break;
                case "vZ":
                    velocity.z = (float)dataChange.Value;
                    break;
                case "rX":
                    _enemyCharacter.SetRotateX((float)dataChange.Value);
                    break;
                case "rY":
                    _enemyCharacter.SetRotateY((float)dataChange.Value);
                    break;
                default:
                    Debug.LogWarning("Field " + dataChange.Field + "is not handled!");
                    break;
            }
        }

        _enemyCharacter.SetMovement(position, velocity, AvarageTimeInterval);
    }


    internal void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}
