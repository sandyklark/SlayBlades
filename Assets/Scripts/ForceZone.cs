using System.Runtime.CompilerServices;
using UnityEngine;

public enum ForceZoneMode { ATTRACT, REPEL }

[RequireComponent(typeof(Collider2D))]
public class ForceZone : MonoBehaviour
{
    public ForceZoneMode mode;
    public float strength = 10f;

    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(!other.TryGetComponent<Rigidbody2D>(out var rigid)) return;

        var direction = transform.position - other.transform.position;

        if (mode == ForceZoneMode.ATTRACT && direction.magnitude < 0.1f) return;

        if (mode == ForceZoneMode.REPEL)
        {
            direction = -direction;
        }

        rigid.AddForce(direction.normalized * strength, ForceMode2D.Force);
    }
}
