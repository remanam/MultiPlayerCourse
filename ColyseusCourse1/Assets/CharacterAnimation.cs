using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private const string GROUNDED = "Grounded";
    private const string SPEED = "Speed";

    [SerializeField] private Animator _animator;
    [SerializeField] private CheckGrounded _checkGrounded;
    [SerializeField] private Character _character;


    // Update is called once per frame
    void Update()
    {
        Vector3 localVelocity = _character.transform.InverseTransformVector(_character.velocity);
        float speed = localVelocity.magnitude / _character.speed;
        float sign = Mathf.Sign(localVelocity.z);

        _animator.SetFloat(SPEED, speed * sign);
        _animator.SetBool(GROUNDED, _checkGrounded.IsGrounded == true);
    }
}
