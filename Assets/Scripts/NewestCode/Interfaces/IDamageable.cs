
public interface IDamageable
{
    PlayerType PlayerType { get; set; }
    void TakeDamage(float damage);
    float GetMaxHealth();
    float GetCurrentHealth();
    void Death();
}
