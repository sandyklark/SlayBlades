using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BattleCharacter))]
public class DeathTrigger : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private SpriteRenderer[] _innerSprites;
    private Collider2D _collider;
    private Collider2D[] _innerColliders;

    private void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _innerSprites = GetComponentsInChildren<SpriteRenderer>();
        _innerColliders = GetComponentsInChildren<Collider2D>();
        var character = GetComponent<BattleCharacter>();
        character.OnDeath += HandleDeath;
    }

    private void HandleDeath()
    {
        _sprite.enabled = false;
        _collider.enabled = false;

        foreach (var sprite in _innerSprites)
        {
            sprite.enabled = false;
        }

        foreach (var col in _innerColliders)
        {
            col.enabled = false;
        }
    }
}
