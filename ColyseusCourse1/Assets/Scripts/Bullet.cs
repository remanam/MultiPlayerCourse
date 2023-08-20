using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifetime = 3f;
    [SerializeField] Rigidbody _rb;

    private int _damage;

    public void Init(Vector3 velocity, int damage = 0)
    {
        _rb.velocity = velocity;
        _damage = damage; // ���� ������ �������� ����, ������� �������� player. �� ��� ��������� ���� ������ �������� 0, ����� �� ���� ����� �����
        StartCoroutine(DelayedDestroy());
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSecondsRealtime(_lifetime);
        Destroy();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out EnemyCharacter enemy))
        {
            enemy.ApplyDamage(_damage);
        }

        Destroy();
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
