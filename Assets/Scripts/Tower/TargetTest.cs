using Common;
using Common.Interfaces;
using UnityEngine;

namespace Tower
{
    public class TargetTest : MonoBehaviour, IDamageable
    {
        public int CurrentHealth { get; }
        public int MaximumHealth { get; }

        [SerializeField] private int _maxHp;

        private int _curentHP;
    
        private void Start()
        {
            _curentHP = _maxHp;
            Debug.Log(gameObject.name+" hp:"+_curentHP);
        }
        private void Death()
        {
            Destroy(gameObject);
        }
    
        public void TakeDamage(int damage)
        {
            _curentHP -= damage;
            Debug.Log(gameObject.name+" hp:"+_curentHP);
            if(_curentHP <= 0)
            {
                GameEvents.OnEnemyDeath.Invoke(this);
                Death();
            }
        }
    
    }
}
