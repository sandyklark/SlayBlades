using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BattleCharacter))]
public class DeathTrigger : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private SpriteRenderer[] _innerSprites;
    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _innerSprites = GetComponentsInChildren<SpriteRenderer>();
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
    }
}
