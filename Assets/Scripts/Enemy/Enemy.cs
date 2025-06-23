using System;
using Common;
using Common.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class Enemy : MonoBehaviour, ISpawnable, IDamageable {
        //Two first properties inherited from IDamageable
        public int CurrentHealth { get; }
        public int MaximumHealth { get; }
        [SerializeField] private int _breachDamage;
        [SerializeField] private float _attackInterval;
        public Transform BreachLocation;

        [SerializeField] private float _breachMargin;
        private bool _hasReached;
        private float _attinter;
        
        //elias
        
        private NavMeshAgent _navMeshAgent;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void Spawn() {
            // GetComponent<NavMeshAgent>().SetDestination(BreachLocation.position);
        }

        public void TakeDamage(int damage) {
            if (CurrentHealth <= 0) {
                GameEvents.OnEnemyDeath?.Invoke(this);
                Die();
            }
        }

        public void Die() {
            Destroy(gameObject);
        }

        private void Update() {
            if (_attinter <= 0) {
                AttemptAttack();
                _attinter = _attackInterval;
            }
            _attinter -= Time.deltaTime;

            //Walk up to the breach point using NavMeshAgent

            /*if (Vector3.Distance(transform.position, BreachLocation.position) <= _breachMargin) {
                _hasReached = true;
            }*/
            
            //elias

            if (Mathf.Approximately(transform.position.x, _navMeshAgent.destination.x) && Mathf.Approximately(transform.position.z, _navMeshAgent.destination.z))
            {
                _hasReached = true;
            }
            
        }

        private void AttemptAttack() {
            if (_hasReached) {
                Debug.Log("Attacking");
                // TODO: Convert the manual set to a request to the GameManager to reduces the lives
                GameData.PlayerLives -= _breachDamage;
                Destroy(this.gameObject);
                GameEvents.OnEnemyBreached?.Invoke();
            }
        }

        private void OnEnable()
        {
            // Spawn();
        }
    }
}
