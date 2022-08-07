using System;
using UnityEngine;

public class SlingshotInput : MonoBehaviour
{
    public Action<Vector2> OnRelease;

    private Vector2 _output;

    private void OnMouseDrag()
    {
        if (Camera.main == null) return;

        var position = transform.position;
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _output = position - mousePos;
        Debug.DrawLine(position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    private void OnMouseUp()
    {
        OnRelease?.Invoke(_output);
    }
}
