using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Transform _leftTransform;
    [SerializeField] private Transform _rightTransform;
    [SerializeField] private float _spawnInterval;
    [SerializeField] private GameObject[] _enemyPrefabs;
    private float _spawnTimer;

    private void Update()
    {
        _spawnTimer += Time.deltaTime;
        HandleSpawn();
    }

    private void HandleSpawn()
    {
        if(_spawnTimer > _spawnInterval)
        {
            SpawnEnemy();
            _spawnTimer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        Instantiate(
            _enemyPrefabs[Random.Range(0,_enemyPrefabs.Length)],
            GenerateSpawnPosition(),
            transform.rotation
        );
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
