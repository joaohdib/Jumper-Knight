using UnityEngine;

public interface IDamageable
{
    // every object that implements this must have these functions

    void TakeDamage(int damage);
    void ApplyKnockback(Vector2 direction, float force, float duration);
}
