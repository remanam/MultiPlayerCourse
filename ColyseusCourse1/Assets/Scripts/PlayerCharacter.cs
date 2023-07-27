using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    private float _inputV;
    private float _inputH;

    public void SetInput(float h, float v)
    {
        _inputH = h;
        _inputV = v;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 direction = new Vector3(_inputH, 0, _inputV).normalized;
        transform.position += direction * Time.deltaTime * speed;

        Debug.Log("Moving");
    }
}
