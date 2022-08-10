using System;
using Effects;
using GameplaySingletons;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Team { BUNNY_PUNK, BEAR_CORE }

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class BattleCharacter : NetworkBehaviour
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

    public void Damage(int amount, Vector2 direction)
    {
        if (_isDead)
        {
            CameraShake.instance.Shake(amount / 2f);
            _rigid.AddForce(direction * amount / 4f, ForceMode2D.Impulse);
            return;
        }

        if (_wrongTeamDebuff) amount = Mathf.CeilToInt(amount * 1.5f);

        _currentHealth -= amount;
        if (_currentHealth < 0) _currentHealth = 0;
        _sprite.color = Color.red;
        _flashFlag = !_flashFlag;

        DamageTextEmitter.Instance.Emit(transform.position, amount, Color.white);
        CameraShake.instance.Shake(amount);
        _rigid.AddForce(direction * amount, ForceMode2D.Impulse);

        if(blood != null) blood.Emit(3 * amount);
    }

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _initialColor = _sprite.color;
        _currentHealth = health;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        var isPlayer1 = OwnerClientId == 0;

        InitPlayerClientRpc(isPlayer1);
    }

    [ClientRpc]
    private void InitPlayerClientRpc(bool isPlayer1)
    {
        Debug.Log(isPlayer1);
        transform.position = new Vector3(isPlayer1 ? -5 : 5, 0, 0);
        _initialColor = isPlayer1 ? Color.green : Color.cyan;
        team = isPlayer1 ? Team.BUNNY_PUNK : Team.BEAR_CORE;
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
