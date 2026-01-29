using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 8f;
    public float accel = 80f;
    public float decel = 100f;

    [Header("Refs")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GroundChecker groundChecker;

    [Header("Optional")]
    [Range(0f, 1f)] public float airControl = 1f;

    [Header("Flip")]
    [Tooltip("Минимальный ввод, чтобы считать смену направления (защита от дрожания стика).")]
    [SerializeField] private float flipDeadzone = 0.01f;

    private float inputX;
    private bool facingRight = true;

    private void Reset()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!groundChecker) groundChecker = GetComponent<GroundChecker>();
    }

    private void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    public void SetMoveInput(float x)
    {
        inputX = Mathf.Clamp(x, -1f, 1f);

        // Флип по вводу (можно переключить на флип по velocity, если нужно)
        if (Mathf.Abs(inputX) > flipDeadzone)
        {
            bool wantRight = inputX > 0f;
            if (wantRight != facingRight)
                Flip();
        }
    }

    private void FixedUpdate()
    {
        if (!rb || !groundChecker) return;

        bool grounded = groundChecker.Evaluate(false);

        float targetX = inputX * moveSpeed;
        float rate = Mathf.Abs(targetX) > 0.01f ? accel : decel;
        if (!grounded) rate *= airControl;

        float newX = Mathf.MoveTowards(rb.linearVelocity.x, targetX, rate * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 s = transform.localScale;
        s.x *= -1f;
        transform.localScale = s;
    }
}
