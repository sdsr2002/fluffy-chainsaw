using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    Vector2 _displaySize;
    Vector2 _touchPosition;
    Camera mainCamera;

    private void Awake()
    {
        _displaySize = new Vector2(Screen.width,Screen.height); 
    }

    /// <summary>
    /// Will take your view position and convert it to world position (- Camera Z position)
    /// </summary>
    /// <param name="ScreenPosition">View Position</param>
    /// <returns>World Position</returns>
    private Vector3 ViewToWorldPosition(Vector2 ScreenPosition)
    {
        return Camera.main.ScreenToWorldPoint(ScreenPosition) - Camera.main.transform.position.z * Vector3.forward;
    }
    private void Update()
    {
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            return;
        }
        _touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Debug.Log(ViewToWorldPosition(_touchPosition));

        UpdateLineRender();
    }

    private void UpdateLineRender()
    {

    }
}
