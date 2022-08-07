using System;
using Effects;
using GameplaySingletons;
using UnityEngine;

public enum Team { BUNNY_PUNK, BEAR_CORE }

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class BattleCharacter : MonoBehaviour
{
    public int health = 100;
    public Team team;
    public ParticleSystem blood;

    private SpriteRenderer _sprite;
    private int _currentHealth;
    private Color _initialColor;
    private bool _flashFlag;
    private Rigidbody2D _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _initialColor = _sprite.color;
        _currentHealth = health;
    }

    public void Damage(int amount, Vector2 direction)
    {
        _currentHealth -= amount;
        if (_currentHealth < 0) _currentHealth = 0;
        _sprite.color = Color.red;
        _flashFlag = !_flashFlag;

        DamageTextEmitter.Instance.Emit(transform.position, amount, Color.white);
        CameraShake.instance.Shake(amount);
        _rigid.AddForce(direction * 10f, ForceMode2D.Impulse);

        if(blood != null) blood.Emit(3 * amount);
    }

    private void Update()
    {
        _sprite.color = Color.Lerp(_sprite.color, _initialColor, Time.deltaTime * 8f);
        if(_currentHealth == 0) _sprite.color = Color.black;
    }
}
