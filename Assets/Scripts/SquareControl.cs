using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareControl : MonoBehaviour
{
    public bool isMouseDown;

    private void onGui()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log($"Mouse Down on {this.name}, {this.transform.position.x}, {this.transform.position.y}");
            this.isMouseDown = true;
        }

    }
}
