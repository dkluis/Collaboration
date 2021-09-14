using UnityEngine;
using System.Collections;
using System;

public class MoveCamera : MonoBehaviour
{

    void Update()
    {
        if (_State == State.None && Input.GetMouseButtonDown(0)) InitDrag();

        if (_State == State.Dragging && Input.GetMouseButton(0)) MoveTheCamera();

        if (_State == State.Dragging && Input.GetMouseButtonUp(0)) FinishDrag();
    }

    #region Calculations
    public State _State = State.None;
    public Vector3 _DragStartPos;

    private void InitDrag()
    {
        _DragStartPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

        _State = State.Dragging;
    }

    private void MoveTheCamera()
    {
        Vector3 actualPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        Vector3 dragDelta = actualPos - _DragStartPos;

        if (Math.Abs(dragDelta.x) < 0.00001f && Math.Abs(dragDelta.y) < 0.00001f) return;

        Camera.main.transform.Translate(-dragDelta);
    }

    private void FinishDrag()
    {
        _State = State.None;
    }
    #endregion

    public enum State
    {
        None,
        Dragging
    }
}
