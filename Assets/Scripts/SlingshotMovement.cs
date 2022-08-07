using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SlingshotInput))]
public class SlingshotMovement : MonoBehaviour
{
    private SlingshotInput _input;
    private Rigidbody2D _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _input = GetComponent<SlingshotInput>();
        _input.OnRelease += HandleRelease;
    }

    private void HandleRelease(Vector2 direction)
    {
        _rigid.AddForce(direction * 4f, ForceMode2D.Impulse);
        _rigid.AddTorque(direction.magnitude * 1.5f, ForceMode2D.Impulse);
    }
}
