using System;
using Unity.Netcode;
using UnityEngine;

public class SlingshotInput : NetworkBehaviour
{
    public Action<Vector2> OnRelease;

    private Vector2 _output;
    private LineRenderer _line;

    private void Awake()
    {
        _line = gameObject.GetComponent<LineRenderer>();
    }

    private void OnMouseDrag()
    {
        if(!IsOwner) return;
        if (Camera.main == null) return;

        var position = transform.position;
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = position.z;
        _output = position - mousePos;
        Debug.DrawLine(position, Camera.main.ScreenToWorldPoint(Input.mousePosition));

        _line.positionCount = 2;
        _line.SetPositions(new [] {
            position,
            mousePos
        });
    }

    private void OnMouseUp()
    {
        if (IsOwner)
        {
            FireSlingshotServerRpc(_output);
        }

        _line.positionCount = 0;
    }

    [ServerRpc]
    private void FireSlingshotServerRpc(Vector2 direction, ServerRpcParams serverRpcParams = default)
    {
        OnRelease?.Invoke(direction);
    }
}

