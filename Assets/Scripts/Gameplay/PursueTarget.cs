using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PursueTarget : MonoBehaviour
{
    public float chaseSeconds;
    public float cooldownSeconds;
    public float chaseSpeed = 10f;
    public bool jumpBackAfterChase;

    private float _currentChaseSeconds;
    private float _currentCooldownSeconds;
    private Rigidbody2D _rigid;
    private Transform _target;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        var hitBox = GetComponentInChildren<HitBox>();
        if (hitBox != null) hitBox.OnHit += HandleHit;
    }

    private void HandleHit(BattleCharacter character)
    {
        if (_currentChaseSeconds != 0 || _currentCooldownSeconds != 0) return;
        _currentChaseSeconds = chaseSeconds;
        _target = character.transform;
    }

    private void Update()
    {
        if (_currentCooldownSeconds == 0 && _target != null && _currentChaseSeconds > 0)
        {
            var direction = _target.transform.position - transform.position;
            _rigid.AddForce(direction * chaseSpeed, ForceMode2D.Force);
            _currentChaseSeconds -= Time.deltaTime;

            if (_currentChaseSeconds <= 0)
            {
                _currentCooldownSeconds = cooldownSeconds;
                if (jumpBackAfterChase)
                {
                    _rigid.AddForce(-direction * chaseSpeed, ForceMode2D.Impulse);
                    _rigid.AddTorque(chaseSpeed, ForceMode2D.Impulse);
                }
            }
        }
        else
        {
            _currentCooldownSeconds -= Time.deltaTime;
        }

        if (_currentChaseSeconds < 0) _currentChaseSeconds = 0;
        if (_currentCooldownSeconds < 0) _currentCooldownSeconds = 0;
    }
}
