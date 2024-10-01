using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    [SerializeField] int _health;

    public void DealtDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            _health = 0;
            GameManager.Instance.PlayerDead();
            UIManager.Instance.UpdatePlayerHealth(_health);
        }

        UIManager.Instance.UpdatePlayerHealth(_health);
    }
}
