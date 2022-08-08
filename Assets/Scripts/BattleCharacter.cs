using System;
using Effects;
using GameplaySingletons;
using UnityEngine;

public enum Team { BUNNY_PUNK, BEAR_CORE }

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class BattleCharacter : MonoBehaviour
{
    public Action OnDeath;

    public int health = 100;
    public Team team;
    public ParticleSystem blood;

    private SpriteRenderer _sprite;
    private int _currentHealth;
    private Color _initialColor;
    private bool _flashFlag;
    private Rigidbody2D _rigid;
    private bool _wrongTeamDebuff;
    private bool _isDead;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _initialColor = _sprite.color;
        _currentHealth = health;
    }

    public void Damage(int amount, Vector2 direction)
    {
        if (_isDead)
        {
            CameraShake.instance.Shake(amount / 2f);
            _rigid.AddForce(direction * amount / 4f, ForceMode2D.Impulse);
            return;
        }

        if (_wrongTeamDebuff) amount *= 3;

        _currentHealth -= amount;
        if (_currentHealth < 0) _currentHealth = 0;
        _sprite.color = Color.red;
        _flashFlag = !_flashFlag;

        DamageTextEmitter.Instance.Emit(transform.position, amount, Color.white);
        CameraShake.instance.Shake(amount);
        _rigid.AddForce(direction * amount, ForceMode2D.Impulse);

        if(blood != null) blood.Emit(3 * amount);
    }

    private void Update()
    {
        if (_isDead) return;

        _sprite.color = Color.Lerp(_sprite.color, _initialColor, Time.deltaTime * 8f);
        if (_currentHealth != 0) return;

        _isDead = true;
        CameraShake.instance.Shake(20);
        if(blood != null) blood.Emit(30);
        _sprite.color = Color.black;
        OnDeath?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent<TeamAlignment>(out var otherTeam))
        {
            _wrongTeamDebuff = otherTeam.team != team;
        }
    }
}
