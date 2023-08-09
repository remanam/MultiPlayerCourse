using Colyseus.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyCharacter _enemyCharacter;
    private List<float> _lastFiveIntervals = new List<float> { 0, 0, 0, 0, 0 };
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
        float interval = Time.time - _lastRecieveTime; // ����� ���������� � ���������� ���������� �������
        _lastRecieveTime = Time.time; // ��������� ��������� ���������� �������

        _lastFiveIntervals.Add(interval);
        _lastFiveIntervals.Remove(0); // � ����� ��������, � � ������ �������
    }

    public void OnChanged(List<DataChange> changes)
    {
        SaveRecieveTime(); // Time.deltatime �� ��� ���������� �� �������


        Vector3 position = _enemyCharacter._targetPosition;
        Vector3 velocity = Vector3.zero;

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
