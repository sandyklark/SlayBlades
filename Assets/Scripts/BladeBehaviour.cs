using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

[RequireComponent(typeof(NetworkRigidbody))]
public class BladeBehaviour : NetworkBehaviour
{
    public GameObject blade;

    private float _initialScale;
    private Quaternion _lastRotation;

    private void Awake()
    {
        _initialScale = blade.transform.localScale.y;
    }

    private void Update()
    {
        var rotation = transform.rotation;
        var spin = Quaternion.Angle(rotation, _lastRotation);
        var scale = blade.transform.localScale;

        _lastRotation = rotation;

        if (spin > 12f)
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
