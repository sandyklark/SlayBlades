using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BladeBehaviour : MonoBehaviour
{
    public GameObject blade;

    private Rigidbody2D _rigid;
    private float _initialScale;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _initialScale = blade.transform.localScale.y;
    }

    private void Update()
    {
        var spin = _rigid.angularVelocity;
        var scale = blade.transform.localScale;

        if (spin > 1000f)
        {
            scale.y = _initialScale;
        }
        else
        {
            var newY = Mathf.Lerp(scale.y, 0f, Time.deltaTime * 2f);
            scale.y = newY;
        }

        blade.transform.localScale = scale;
    }
}
