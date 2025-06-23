using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Interfaces;
using UnityEngine;

namespace Tower
{
    [RequireComponent(typeof(SphereCollider))]
    public class Tower : MonoBehaviour, IUpgradable
    {
        [SerializeField] private TowerSO _towerData;
        [SerializeField] private Rigidbody _turretRb;
        [SerializeField] private Transform _headCanon;
        [SerializeField] private ParticleSystem _fx;

        int IUpgradable.SellValue
        {
            get => _sellValue;
            set => _sellValue = value;
        }

        int IUpgradable.UpgradeCost
        {
            get => _upgradeCost;
            set => _upgradeCost = value;
        }

        private int _damage;
        private float _fireRate;
        private float _range;
        private float _cooldownFireRate;
        private SphereCollider _foesDetector;
        private List<GameObject> _targetList = new List<GameObject>();
        private GameObject _myTarget = null;
        private int _sellValue;
        private int _upgradeCost;

        private void OnEnable()
        {
            GameEvents.OnEnemyDeath += UpdateDeadTarget;
        }
        private void OnDisable()
        {
            GameEvents.OnEnemyDeath -= UpdateDeadTarget;
        }
        private void Start()
        {
            _foesDetector = GetComponent<SphereCollider>();
            InitiateTowerStats();
            InitiateTowerComponents();
        }
        private void Update()
        {
            if(_myTarget)
                TurretRotationUpdate();
            AutoAttack();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                _targetList.Add(other.gameObject);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (_targetList.Count == 0)
            {
                if (other.TryGetComponent(out IDamageable damageable))
                {
                    _targetList.Add(other.gameObject);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                _targetList.Remove(other.gameObject);
            }
        }
        private void InitiateTowerStats()
        {
            _damage = _towerData.Damage;
            _fireRate = _towerData.FireRate;
            _range = _towerData.Range;
        }
        private void InitiateTowerComponents()
        {
            _sellValue = _towerData.SellValue;
            _upgradeCost = _towerData.Cost;
            _foesDetector.isTrigger = true;
            _foesDetector.radius = _range;
        }
        private void AutoAttack()
        {
            if (_cooldownFireRate < _fireRate)
            {
                _cooldownFireRate += Time.deltaTime;
            }
            else
            {
                TargetSelector();
                if (_myTarget)
                {
                    Shot();
                    _myTarget.GetComponent<IDamageable>().TakeDamage(_damage);
                    _cooldownFireRate = 0;
                }
            }
        }
        private void Shot()
        {
            _fx.Play();
        }
        private void TurretRotationUpdate()
        {
            Quaternion rotation = Quaternion.LookRotation(_myTarget.transform.position - _turretRb.transform.position, Vector3.up);
            _turretRb.MoveRotation(rotation);
        }
        private void TargetSelector()
        {
            if (_targetList.Count == 0)
                _myTarget = null;
            else if (_targetList.Count == 1)
            {
                _myTarget = _targetList[0];
            }
            else
            {
                foreach (GameObject target in _targetList)
                {
                    if (Vector3.Distance(transform.position, _myTarget.transform.position) >
                        Vector3.Distance(transform.position, target.transform.position))
                    {
                        _myTarget = target;
                    }
                }
            }
        }
        private void UpdateDeadTarget(IDamageable target)
        {
            foreach (GameObject targetInList in _targetList.ToList())
            {
                if(targetInList.GetComponent<IDamageable>() == target)
                    _targetList.Remove(targetInList);
            }
        }



        public void UpgradeTower()
        {
            if (_towerData.UpgradedTower)
            {
                Instantiate(_towerData.UpgradedTower.TowerPrefab,transform.position,transform.rotation);
                Destroy(gameObject);
            }
        }
        public int SellTower()
        {
            return _towerData.SellValue;
        }
    }
}
