using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBrain : MonoBehaviour
{
    [Header("Actions (New Input System)")]
    [SerializeField] private InputActionReference moveAction; 
    [SerializeField] private InputActionReference jumpAction; 

    [Header("Gameplay")]
    [SerializeField] private PlayerMover mover;
    [SerializeField] private PlayerJumping jumping;

    private float _moveX;

    private void Reset()
    {
        mover = GetComponent<PlayerMover>();
        jumping = GetComponent<PlayerJumping>();
    }

    private void OnEnable()
    {
        if (moveAction?.action != null)
        {
            moveAction.action.Enable();
            moveAction.action.performed += OnMove;
            moveAction.action.canceled += OnMove;
        }

        if (jumpAction?.action != null)
        {
            jumpAction.action.Enable();
            jumpAction.action.performed += OnJumpPressed;
            jumpAction.action.canceled += OnJumpReleased;
        }
    }

    private void OnDisable()
    {
        if (moveAction?.action != null)
        {
            moveAction.action.performed -= OnMove;
            moveAction.action.canceled -= OnMove;
            moveAction.action.Disable();
        }

        if (jumpAction?.action != null)
        {
            jumpAction.action.performed -= OnJumpPressed;
            jumpAction.action.canceled -= OnJumpReleased;
            jumpAction.action.Disable();
        }
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 v = ctx.ReadValue<Vector2>();
        _moveX = Mathf.Clamp(v.x, -1f, 1f);
        if (mover) mover.SetMoveInput(_moveX);
    }

    private void OnJumpPressed(InputAction.CallbackContext ctx)
    {
        if (!jumping) return;
        jumping.RequestJump();         
        jumping.SetJumpHeld(true);
    }

    private void OnJumpReleased(InputAction.CallbackContext ctx)
    {
        if (!jumping) return;
        jumping.SetJumpHeld(false);
        jumping.NotifyJumpReleased(); 
    }
}
