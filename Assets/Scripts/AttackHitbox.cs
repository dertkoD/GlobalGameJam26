using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [Header("Hit settings")]
    [SerializeField] private LayerMask hittableMask = ~0;
    [SerializeField] private int damage = 1;

    private BoxCollider2D _col;

    private void Awake()
    {
        _col = GetComponent<BoxCollider2D>();
        _col.isTrigger = true;
        SetActive(false);
    }

    public void SetActive(bool active)
    {
        if (_col) _col.enabled = active;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & hittableMask) == 0)
            return;

        var breakable = other.GetComponent<BreakableWall>();
        if (breakable)
        {
            breakable.Hit(damage);
            Debug.Log("Hit");
        }
    }
}
