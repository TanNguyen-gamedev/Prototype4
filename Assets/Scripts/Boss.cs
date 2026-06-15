using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public enum SkillType
    {   
        Agile,
        SpawnMinions,
        SmashDown
    }

    [SerializeField] private GameObject _minionPrefab;
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private float _skillInterval = 10f; 
    [SerializeField] private int _minionPerSpawn = 3;


    [Header("Smash Down setting")]
    [SerializeField] private float _explosionForce = 500f;
    [SerializeField] private float _explosionRange = 5f;
    [SerializeField] private float _uplift = 2f;
    [SerializeField] private float _dashForce = 500f;
    private Dictionary<SkillType, Coroutine> _activeCoroutines = new();
    private Coroutine _missileCoroutine;
    private float _skillTimer;


    protected override void Update()
    {
        base.Update();
        _skillTimer += Time.deltaTime;
        if(_skillTimer >= _skillInterval)
        {
            ExecuteSkill();
            _skillTimer = 0;
        }
    }

    private void ExecuteSkill()
    {
        int randomSkill = Random.Range(0,3);

        ActiveSkill((SkillType)randomSkill, 6f);
    }

    public void ActiveSkill(SkillType skillType, float duration)
        {
            Debug.Log("Skill actived:" + skillType);
            if(_activeCoroutines.TryGetValue(skillType, out Coroutine existing))
            {
                StopCoroutine(existing);
            }
            _activeCoroutines[skillType] = StartCoroutine(SkillCountDown(skillType, duration));
        }

        private IEnumerator SkillCountDown(SkillType skillType, float duration)
        {
            AddEffect(skillType);
            yield return new WaitForSeconds(duration);
            RemoveEffect(skillType);
            _activeCoroutines.Remove(skillType);
        }

        private void AddEffect(SkillType skillType)
        {
            switch (skillType)
            {
                case SkillType.Agile:
                {
                    Vector3 direction = (_player.transform.position - transform.position).normalized;
                    _rb.AddForce(direction * _dashForce, ForceMode.Impulse);
                }
                break;

                case SkillType.SpawnMinions:
                {
                    _missileCoroutine = StartCoroutine(SpawnMininons());
                }
                break;

                case SkillType.SmashDown:
                {
                   
                }
                break;
            }
        }

        private void RemoveEffect(SkillType skillType)
        {
            switch (skillType)
            {
                case SkillType.Agile:
                {
                   
                    
                }
                break;

                case SkillType.SpawnMinions:
                {
                    if(_missileCoroutine != null)
                    {
                        StopCoroutine(_missileCoroutine);
                        _missileCoroutine = null;
                    }
                   
                }
                break;

                case SkillType.SmashDown:
                {
                   
                }
                break;
            }
        }

        private IEnumerator SpawnMininons()
        {
            while(true)
            {
                for(int i = 0; i < _minionPerSpawn; i++)
                {
                    Vector3 direction = (_player.transform.position - transform.position).normalized;
                    float offset = 1.1f;
                    Vector3 spawnPosition = _player.transform.position + (direction * offset);                
                    Instantiate(_minionPrefab,
                    spawnPosition,
                    Quaternion.LookRotation(direction)
                    );

                }
                yield return new WaitForSeconds(_spawnInterval);
            }
        }

       
        public void ApplySmashExplosion()
        {
            if(_player.TryGetComponent<Rigidbody>(out var rb))
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
