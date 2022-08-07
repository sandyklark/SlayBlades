using System;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public int damage;
    private BattleCharacter _character;
    private bool _belongsToCharacter;

    private void Awake()
    {
        _character = transform.parent.GetComponent<BattleCharacter>();
        _belongsToCharacter = _character != null;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent<BattleCharacter>(out var otherCharacter)) return;
        if (_belongsToCharacter && _character.team == otherCharacter.team) return;
        var direction = (Vector2)col.transform.position - col.ClosestPoint(transform.position);
        otherCharacter.Damage(damage, direction.normalized);
    }
}
