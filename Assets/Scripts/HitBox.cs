using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HitBox : MonoBehaviour
{
    public Action<BattleCharacter> OnHit;

    public int damage;
    private BattleCharacter _character;
    private bool _belongsToCharacter;

    private void Awake()
    {
        _character = transform.GetComponentInParent<BattleCharacter>();
        _belongsToCharacter = _character != null;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent<BattleCharacter>(out var otherCharacter)) return;
        if (_belongsToCharacter && _character.team == otherCharacter.team) return;
        var direction = (Vector2)col.transform.position - col.ClosestPoint(transform.position);
        var dmg = Random.Range(damage / 2, damage);
        otherCharacter.Damage(dmg, direction.normalized);
        OnHit?.Invoke(otherCharacter);
    }
}
