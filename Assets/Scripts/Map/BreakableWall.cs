using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [SerializeField] private int hp = 1;

    public void Hit(int damage)
    {
        hp -= damage;
        if (hp <= 0)
            Destroy(gameObject);
    }
}
