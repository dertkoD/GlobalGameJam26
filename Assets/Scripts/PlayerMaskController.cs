using UnityEngine;

public class PlayerMaskController : MonoBehaviour
{
    [Header("State")]
    [SerializeField] private MaskEnum equippedMask = MaskEnum.None;

    [Header("Unlocked (pseudo-inventory)")]
    [SerializeField] private bool hasStrikeMask = false;

    [Header("Mechanics")]
    [SerializeField] private PlayerAttack strikeAttack;

    public MaskEnum EquippedMask => equippedMask;
    public bool HasStrikeMask => hasStrikeMask;

    private void Reset()
    {
        if (!strikeAttack) strikeAttack = GetComponent<PlayerAttack>();
    }

    private void Awake()
    {
        ApplyEquippedMask();
    }

    public void UnlockMask(MaskEnum mask)
    {
        if (mask == MaskEnum.Strike) hasStrikeMask = true;

        Equip(mask);
    }

    public void Equip(MaskEnum mask)
    {
        if (mask == MaskEnum.Strike && !hasStrikeMask) return;

        equippedMask = mask;
        ApplyEquippedMask();
    }

    public void Unequip()
    {
        equippedMask = MaskEnum.None;
        ApplyEquippedMask();
    }

    private void ApplyEquippedMask()
    {
        if (strikeAttack) strikeAttack.enabled = (equippedMask == MaskEnum.Strike);
    }
}
