using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [Header("Movement & Aim")]
    [HideInInspector]
    public Vector2 move;
    [HideInInspector]
    public Vector2 look;
    [HideInInspector]
    public bool sprint;
    [HideInInspector]
    public bool jump;
    [HideInInspector]
    public bool crouch;

    [Header("Shooting")]
    [HideInInspector]
    public bool shoot;
    [HideInInspector]
    public bool reload;

    public bool cursorLocked;
    public bool cursorInputForLook;

    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }
    public void OnLook(InputValue value)
    {
        if (cursorInputForLook)
        {
            LookInput(value.Get<Vector2>());
        }
    }
    public void OnSprint(InputValue value)
    {
        SprintInput(value.isPressed);
    }
    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }
    public void OnCrouch(InputValue value)
    {
        CrouchInput(value.isPressed);
    }
    public void OnShoot(InputValue value)
    {
        ShootInput(value.isPressed);
    }
    public void OnReload(InputValue value)
    {
        ReloadInput(value.isPressed);
    }


    public void MoveInput(Vector2 newMovementDirection)
    {
        move = newMovementDirection;
    }
    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }
    public void SprintInput(bool newSprint)
    {
        sprint = newSprint;
    }
    public void JumpInput(bool newJump)
    {
        jump = newJump;
    }
    public void CrouchInput(bool newCrouch)
    {
        crouch = newCrouch;
    }
    public void ShootInput(bool newShoot)
    {
        shoot = newShoot;
    }
    public void ReloadInput(bool newReload)
    {
        reload = newReload;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }

}