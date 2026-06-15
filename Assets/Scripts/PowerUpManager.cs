    using System.Collections;
    using System.Collections.Generic;
    using Unity.VisualScripting;
    using UnityEngine;

    public class PowerUpManager : MonoBehaviour
    {
        [SerializeField] private GameObject _misslePrefab;
        [SerializeField] private float _fireInterval = 0.5f;
        [SerializeField] private Transform _focalPoint; 
        [Header("Smash Down setting")]
        [SerializeField] private float _explosionForce = 500f;
        [SerializeField] private float _explosionRange = 5f;
        [SerializeField] private float _uplift = 2f;
        private Dictionary<PowerUpType, Coroutine> _activeCoroutines = new();
        private Coroutine _missileCoroutine;
        private PlayerController _playerController;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
        }
        public void ActivePowerUp(PowerUpType powerType, float duration)
        {
            if(_activeCoroutines.TryGetValue(powerType, out Coroutine existing))
            {
                StopCoroutine(existing);
            }
            _activeCoroutines[powerType] = StartCoroutine(PowerUpCountDown(powerType, duration));
        }

        private IEnumerator PowerUpCountDown(PowerUpType powerType, float duration)
        {
            AddEffect(powerType);
            yield return new WaitForSeconds(duration);
            _activeCoroutines.Remove(powerType);
            RemoveEffect(powerType);
        }

        private void AddEffect(PowerUpType powerType)
        {
            switch (powerType)
            {
                case PowerUpType.Knockback:
                {
                    _playerController.SetKnockback(true);
                }
                break;

                case PowerUpType.HomingMissle:
                {
                    _missileCoroutine = StartCoroutine(SpawnMissiles());
                }
                break;

                case PowerUpType.SmashDown:
                {
                    StartCoroutine(_playerController.ApplySmash());
                }
                break;
            }
        }

        private void RemoveEffect(PowerUpType powerType)
        {
            switch (powerType)
            {
                case PowerUpType.Knockback:
                {
                    _playerController.SetKnockback(false);
                    if(_activeCoroutines.Count == 0)
                    {
                        _playerController.TurnOffTheIndicator();  
                    }
                    
                }
                break;

                case PowerUpType.HomingMissle:
                {
                    if(_missileCoroutine != null)
                    {
                        StopCoroutine(_missileCoroutine);
                        _missileCoroutine = null;
                    }
                    if(_activeCoroutines.Count == 0)
                    {
                        _playerController.TurnOffTheIndicator();  
                    }
                }
                break;

                case PowerUpType.SmashDown:
                {
                    if(_activeCoroutines.Count == 0)
                    {
                        _playerController.TurnOffTheIndicator();  
                    }
                }
                break;
            }
        }

        private IEnumerator SpawnMissiles()
        {
            while(true)
            {
                Enemy[] enemies = GetEnemies();
                for(int i = 0; i < enemies.Length; i++)
                {
                    Vector3 direction = (enemies[i].transform.position - _playerController.transform.position).normalized;
                    float offset = 1.1f;
                    Vector3 spawnPosition = _playerController.transform.position + (direction * offset);                
                    Projectile projectile = Instantiate(_misslePrefab,
                    spawnPosition,
                    Quaternion.LookRotation(direction)
                    ).GetComponent<Projectile>();
                    projectile.SetTarget(enemies[i].gameObject.transform);
                }
                yield return new WaitForSeconds(_fireInterval);
            }
        }

        private Enemy[] GetEnemies()
        {
            Collider[] colliders = new Collider[100];
            int count = Physics.OverlapSphereNonAlloc(transform.position, _explosionRange * 2, colliders);
            List<Enemy> enemies = new List<Enemy>();
            for(int i = 0; i < count; i++)
            {
                if(colliders[i].TryGetComponent<Enemy>(out Enemy enemy))
                {
                    if(enemy)
                    {
                        enemies.Add(enemy);
                    }
                }
            }
            return enemies.ToArray();
        }
        public void ApplySmashExplosion()
        {
            Debug.Log("SmashDown");
            Enemy[] enemies = GetEnemies();
            foreach(var enemy in enemies)
            {   
                if(enemy.TryGetComponent<Rigidbody>(out var rb))
                {
                    rb.AddExplosionForce(
                    _explosionForce,
                    transform.position,
                    _explosionRange,
                    _uplift,
                    ForceMode.Impulse          
                    );
                }
            }
           
        }
    }
