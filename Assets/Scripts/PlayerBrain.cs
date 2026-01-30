using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBrain : MonoBehaviour
{
    [Header("Actions (New Input System)")]
    [SerializeField] private InputActionReference moveAction;       
    [SerializeField] private InputActionReference jumpAction;       
    [SerializeField] private InputActionReference attackAction;     
    [SerializeField] private InputActionReference interactAction;   // E
    [SerializeField] private InputActionReference equipStrikeAction;// (опционально) 1

    [Header("Gameplay")]
    [SerializeField] private PlayerMover mover;
    [SerializeField] private PlayerJumping jumping;
    [SerializeField] private PlayerMaskController masks;
    [SerializeField] private PlayerAttack attack;

    private MaskPickup _nearbyPickup;

    private void Reset()
    {
        mover = GetComponent<PlayerMover>();
        jumping = GetComponent<PlayerJumping>();
        masks = GetComponent<PlayerMaskController>();
        attack = GetComponent<PlayerAttack>();
    }

    private void OnEnable()
    {
        EnableAndBind(moveAction, OnMove, OnMove);
        EnableAndBind(jumpAction, OnJumpPressed, OnJumpReleased);
        EnableAndBind(attackAction, OnAttack, null);
        EnableAndBind(interactAction, OnInteract, null);

        if (equipStrikeAction?.action != null)
        {
            equipStrikeAction.action.Enable();
            equipStrikeAction.action.performed += OnEquipStrike;
        }
    }

    private void OnDisable()
    {
        DisableAndUnbind(moveAction, OnMove, OnMove);
        DisableAndUnbind(jumpAction, OnJumpPressed, OnJumpReleased);
        DisableAndUnbind(attackAction, OnAttack, null);
        DisableAndUnbind(interactAction, OnInteract, null);

        if (equipStrikeAction?.action != null)
        {
            equipStrikeAction.action.performed -= OnEquipStrike;
            equipStrikeAction.action.Disable();
        }
    }

    private void EnableAndBind(InputActionReference a,
        System.Action<InputAction.CallbackContext> performed,
        System.Action<InputAction.CallbackContext> canceled)
    {
        if (a?.action == null) return;
        a.action.Enable();
        a.action.performed += performed;
        if (canceled != null) a.action.canceled += canceled;
    }

    private void DisableAndUnbind(InputActionReference a,
        System.Action<InputAction.CallbackContext> performed,
        System.Action<InputAction.CallbackContext> canceled)
    {
        if (a?.action == null) return;
        a.action.performed -= performed;
        if (canceled != null) a.action.canceled -= canceled;
        a.action.Disable();
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        float x = Mathf.Clamp(ctx.ReadValue<Vector2>().x, -1f, 1f);
        if (mover) mover.SetMoveInput(x);
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

    private void OnAttack(InputAction.CallbackContext ctx)
    {
        if (attack && attack.enabled)
            attack.RequestAttack();
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (_nearbyPickup)
            _nearbyPickup.TryPickup();
    }

    private void OnEquipStrike(InputAction.CallbackContext ctx)
    {
        if (masks) masks.Equip(MaskEnum.Strike);
    }

    public void SetNearbyPickup(MaskPickup pickup) => _nearbyPickup = pickup;
    public void ClearNearbyPickup(MaskPickup pickup)
    {
        if (_nearbyPickup == pickup) _nearbyPickup = null;
    }
}
