using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : Gun
{
    public void Shoot(Vector3 position, Vector3 velocity)
    {
        Instantiate(_bulletPrefab, position, Quaternion.identity).Init(velocity);
        Debug.Log(position.x + " " + position.y + " " + position.z);
        Debug.Log(velocity.x + " " + velocity.y + " " + velocity.z);
        shoot?.Invoke();
    }
}
