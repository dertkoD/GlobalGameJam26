using UnityEngine;

public class MaskPickup : MonoBehaviour
{
    [SerializeField] private MaskEnum maskId = MaskEnum.Strike;

    [Header("Pickup")]
    [SerializeField] private bool destroyOnPickup = true;

    private bool _playerInRange;
    private PlayerMaskController _playerMasks;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var masks = other.GetComponentInParent<PlayerMaskController>();
        if (!masks) return;

        _playerInRange = true;
        _playerMasks = masks;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var masks = other.GetComponentInParent<PlayerMaskController>();
        if (!masks) return;

        if (_playerMasks == masks)
        {
            _playerInRange = false;
            _playerMasks = null;
        }
    }

    public void TryPickup()
    {
        if (!_playerInRange || !_playerMasks) return;

        _playerMasks.UnlockMask(maskId);

        if (destroyOnPickup) Destroy(gameObject);
        else gameObject.SetActive(false);
    }
}
