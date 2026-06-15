using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Transform _leftTransform;
    [SerializeField] private Transform _rightTransform;
    [SerializeField] private float _spawnInterval;
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private GameObject[] _powerUpPrefabs;
    private List<Enemy> _activeEnemies = new List<Enemy>();
    private float _spawnTimer;
    private int _currentWave = 1;

    private void Awake()
    {
        Enemy.OnDeath += OnEnemyDeath;   
    }

    private void OnDestroy()
    {
        Enemy.OnDeath -= OnEnemyDeath;
    }

    private void Update()
    {
 
        HandleSpawn();
    }

    private void HandleSpawn()
    {
        if(_activeEnemies.Count == 0)
        {
            _spawnTimer += Time.deltaTime;
            if(_spawnTimer > _spawnInterval)
            {
                for(int i = 0; i < _currentWave; i++)
                {
                    SpawnEnemy();
                }
                SpawnPowerUp();
                _currentWave++;
                _spawnTimer = 0f;
            }
           
        }
    }

    private void SpawnEnemy()
    {
        _activeEnemies.Add(Instantiate(
            _enemyPrefabs[Random.Range(0,_enemyPrefabs.Length)],
            GenerateSpawnPosition(),
            transform.rotation
        ).GetComponent<Enemy>());

    }

    private void SpawnPowerUp()
    {
        Instantiate(_powerUpPrefabs[Random.Range(0, _powerUpPrefabs.Length)],
        GenerateSpawnPosition(),
        transform.rotation
        );
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        _activeEnemies.Remove(enemy);
    }

    private Vector3 GenerateSpawnPosition()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(_leftTransform.position.x,_rightTransform.position.x),
            0,
            Random.Range(_leftTransform.position.z,_rightTransform.position.z)
        );
        return spawnPosition;
    }

}
