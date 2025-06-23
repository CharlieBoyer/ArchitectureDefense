namespace Common.Interfaces
{
    public interface IDamageable
    {
        int CurrentHealth { get; }
        int MaximumHealth { get; }
        public void TakeDamage(int damage);
    }
}
