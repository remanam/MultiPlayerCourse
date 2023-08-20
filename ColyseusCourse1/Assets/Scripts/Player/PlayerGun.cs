using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : Gun
{
    [SerializeField] private int _damage;
    [SerializeField] private Transform _bulletPoint;
    [SerializeField] private float _bulletSpeed;

    [SerializeField] private float _shootDelay;
    private float _lastShootTime;



    public bool TryShoot(out ShootInfo info)
    {
        info = new ShootInfo();

        if (_lastShootTime - Time.time > _shootDelay) return false;


        Vector3 position = _bulletPoint.position;
        Vector3 velocity = _bulletPoint.forward * _bulletSpeed;

        _lastShootTime = Time.time;

        var bullet = Instantiate(_bulletPrefab, _bulletPoint.position, _bulletPoint.rotation);
        bullet.Init(velocity, _damage); //Включаем физику пули

        shoot?.Invoke();

        info.pX = position.x;
        info.pY = position.y;
        info.pZ = position.z;
        info.dX = velocity.x;
        info.dY = velocity.y;
        info.dZ = velocity.z;

        //Debug.Log(position.x + " " + position.y + " " + position.z);

        return true;
    }
}
