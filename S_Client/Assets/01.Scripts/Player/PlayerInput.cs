using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action<Vector3> movementInput;
    public event Action<Vector2> mouseMove;
    public event Action jumpKeyPress;

    private float rotateX = 0; 
    private float rotateY = 0; 

    private void Update() {
        MovementKeyPress();
        MouseMoveInput();
        JumpKeyInput();
    }

    private void JumpKeyInput()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            jumpKeyPress?.Invoke();
    }

    private void MouseMoveInput()
    {
        rotateX += Input.GetAxis("Mouse X");
        rotateX = (rotateX > 360) ? rotateX % 360 : rotateX; 
        rotateY += Input.GetAxis("Mouse Y");
        mouseMove?.Invoke(new Vector2(rotateX,rotateY));
    }

    private void MovementKeyPress()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(inputX,0,inputY);

        movementInput?.Invoke(dir.normalized);
    }
    
}
