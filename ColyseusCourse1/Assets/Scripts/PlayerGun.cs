using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _bulletPoint;
    [SerializeField] private float _bulletSpeed;

    [SerializeField] private float _shootDelay;
    private float _lastShootTime;

    public Action shoot;

    public void Shoot()
    {
        if (_lastShootTime - Time.time > _shootDelay) return;

        _lastShootTime = Time.time;

        var bullet = Instantiate(_bulletPrefab, _bulletPoint.position, _bulletPoint.rotation);

        bullet.Init(_bulletPoint.forward, _bulletSpeed); // ѕул€ должна полететь вперед от направлени€ головы
        shoot?.Invoke();
    }
}
