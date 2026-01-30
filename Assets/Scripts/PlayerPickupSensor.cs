using UnityEngine;

public class PlayerPickupSensor : MonoBehaviour
{
    [SerializeField] private PlayerBrain brain;

    private void Reset()
    {
        brain = GetComponentInParent<PlayerBrain>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var pickup = other.GetComponent<MaskPickup>();
        if (pickup && brain) brain.SetNearbyPickup(pickup);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var pickup = other.GetComponent<MaskPickup>();
        if (pickup && brain) brain.ClearNearbyPickup(pickup);
    }
}
