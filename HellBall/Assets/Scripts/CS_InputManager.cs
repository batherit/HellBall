using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_InputManager : MonoBehaviour {

    private bool isLeftDown;
    private bool isRightDown;
    private bool isJumpDown;
    private bool isDiveDown;
    [HideInInspector] public CS_Joystick joystick;
  

    private void Awake()
    {
        isLeftDown = false;
        isRightDown = false;
        isJumpDown = false;
        isDiveDown = false;
        joystick = FindObjectOfType<CS_Joystick>();
    }

    public bool IsStickDragging() { return joystick.IsStickDragging(); }
    public Vector2 GetStickDirection() { return joystick.GetStickDirection(); }
    public bool IsOutsideOfReloadRadius() { return joystick.IsOutsideOfReloadRadius(); }

    public void LeftDown() { isLeftDown = true; }
    public void LeftUp() { isLeftDown = false; }
    public bool IsLeftDown() { return isLeftDown; }

    public void RightDown() { isRightDown = true; }
    public void RightUp() { isRightDown = false; }
    public bool IsRightDown() { return isRightDown; }

    public void JumpDown() { isJumpDown = true; }
    public void JumpUp() { isJumpDown = false; }
    public bool IsJumpDown() { return isJumpDown; }

    public void DiveDown() { isDiveDown = true; }
    public void DiveUp() { isDiveDown = false; }
    public bool IsDiveDown() { return isDiveDown; }
}
