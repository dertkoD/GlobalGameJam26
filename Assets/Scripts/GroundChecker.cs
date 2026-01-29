using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Ground filter")]
    [SerializeField] private LayerMask groundMask = ~0;
    [Tooltip("Допуск по углу нормали (90° = строго вверх). Пример: 10° => 80..100.")]
    [Range(0f, 45f)]
    [SerializeField] private float normalAngleTolerance = 10f;

    [Header("Airborne gating")]
    [Tooltip("Upward velocity that immediately marks jump as airborne before contacts separate")]
    [SerializeField] private float leaveGroundVelocity = 0.05f;

    public bool IsGrounded { get; private set; }

    void Reset()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public bool Evaluate(bool isJumping)
    {
        if (!rb) return false;

        if (isJumping && rb.linearVelocity.y > leaveGroundVelocity)
            return IsGrounded = false;

        var filter = new ContactFilter2D
        {
            useTriggers    = false,
            useLayerMask   = true,
            layerMask      = groundMask,
            useNormalAngle = true,
            minNormalAngle = 90f - normalAngleTolerance,
            maxNormalAngle = 90f + normalAngleTolerance
        };

        return IsGrounded = rb.IsTouching(filter);
    }
}
