using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private string attackTriggerName = "AttackTrigger";

    [Header("Hitbox (child object)")]
    [SerializeField] private AttackHitbox hitbox;

    [Header("Timing")]
    [SerializeField] private float attackCooldown = 0.15f;

    private int _attackTriggerHash;
    private float _cooldownTimer;

    private void Reset()
    {
        animator = GetComponentInChildren<Animator>();
        hitbox = GetComponentInChildren<AttackHitbox>(true);
    }

    private void Awake()
    {
        _attackTriggerHash = Animator.StringToHash(attackTriggerName);
        if (hitbox) hitbox.SetActive(false);
    }

    private void Update()
    {
        if (_cooldownTimer > 0f)
            _cooldownTimer -= Time.deltaTime;
    }

    public void RequestAttack()
    {
        if (_cooldownTimer > 0f) return;
        _cooldownTimer = attackCooldown;

        if (animator)
            animator.SetTrigger(_attackTriggerHash);
    }
    
    public void EnableHitbox()
    {
        if (hitbox) hitbox.SetActive(true);
    }

    public void DisableHitbox()
    {
        if (hitbox) hitbox.SetActive(false);
    }
}
