using UnityEngine;
using System.Collections;
using System;

public class MoveCamera : MonoBehaviour
{

    void Update()
    {
        if (_State == State.None && Input.GetMouseButtonDown(1)) InitDrag();

        if (_State == State.Dragging && Input.GetMouseButton(1)) MoveTheCamera();

        if (_State == State.Dragging && Input.GetMouseButtonUp(1)) FinishDrag();
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

    public static void MoveTo(Transform player)
    {
        Camera.main.transform.LookAt(player);
        //Camera.main.transform.eulerAngles = new Vector3(0, 0, 0);
        /*
        Vector3 actualPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        Vector3 dragDelta = actualPos - moveToPos;

        //if (Math.Abs(dragDelta.x) < 0.00001f && Math.Abs(dragDelta.y) < 0.00001f) return;

        Camera.main.transform.Translate(-dragDelta);
        */
    }

    public enum State
    {
        None,
        Dragging
    }
}