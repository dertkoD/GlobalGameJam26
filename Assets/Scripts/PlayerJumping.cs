using UnityEngine;

public class PlayerJumping : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GroundChecker groundChecker;

    [Header("Buffer / Coyote")]
    [SerializeField] private float jumpBufferTime = 0.12f;
    [SerializeField] private float coyoteTime     = 0.08f;

    [Header("Jump shape")]
    [SerializeField] private float minJumpVelocity = 8.5f;
    [SerializeField] private float maxJumpVelocity = 14.0f;
    [SerializeField] private float maxHoldTime     = 0.18f;

    [Range(0.1f, 0.95f)]
    [SerializeField] private float jumpCutMultiplier = 0.5f;

    private float _buffer;
    private float _coyote;
    private bool  _isJumping;

    private bool  _holdingJump;
    private float _holdTimer;

    private bool _wantsJumpCut;

    public bool IsGrounded => groundChecker ? groundChecker.IsGrounded : false;

    void Reset()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!groundChecker) groundChecker = GetComponent<GroundChecker>();
    }

    void Update()
    {
        if (!rb || !groundChecker) return;

        bool grounded = groundChecker.Evaluate(_isJumping);

        _coyote = grounded ? coyoteTime : Mathf.Max(0f, _coyote - Time.deltaTime);
        _buffer = Mathf.Max(0f, _buffer - Time.deltaTime);

        if (grounded && _isJumping)
        {
            _isJumping = false;
            _holdTimer = 0f;
        }
    }

    void FixedUpdate()
    {
        if (!rb || !groundChecker) return;

        bool grounded = groundChecker.IsGrounded;

        if (_buffer > 0f && (_coyote > 0f || grounded))
        {
            DoJumpStart();
        }

        if (_isJumping && !grounded && _holdingJump && _holdTimer < maxHoldTime && rb.linearVelocity.y > 0f)
        {
            _holdTimer += Time.fixedDeltaTime;

            float t = Mathf.Clamp01(_holdTimer / maxHoldTime);
            float targetVy = Mathf.Lerp(minJumpVelocity, maxJumpVelocity, t);

            var v = rb.linearVelocity;
            if (v.y < targetVy) v.y = targetVy;
            rb.linearVelocity = v;
        }

        if (_wantsJumpCut)
        {
            _wantsJumpCut = false;

            if (!grounded && rb.linearVelocity.y > 0f)
            {
                var v = rb.linearVelocity;
                v.y *= jumpCutMultiplier;
                rb.linearVelocity = v;
            }
        }
    }

    private void DoJumpStart()
    {
        var v = rb.linearVelocity;
        v.y = minJumpVelocity;
        rb.linearVelocity = v;

        _isJumping = true;
        _buffer = 0f;
        _coyote = 0f;

        _holdTimer = 0f;
    }

    public void RequestJump() => _buffer = jumpBufferTime;
    public void NotifyJumpReleased() => _wantsJumpCut = true;
    public void SetJumpHeld(bool held) => _holdingJump = held;
}
