using System;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   [SerializeField] private float _speed;
   [SerializeField] private GameObject _player;
   [SerializeField] private float _deathHeght = -1f;
   private bool _isDead = false;

    public static event Action<Enemy> OnDeath;
   private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GameObject.Find("Player");
    }

    private void FixedUpdate()
    {
        // Chase Player
        Vector3 direction = (_player.transform.position - transform.position).normalized;
        _rb.AddForce(direction * _speed * Time.deltaTime, ForceMode.Impulse);
    }

    private void Update()
    {
        if(transform.position.y < _deathHeght && !_isDead)
        {
            OnDeath?.Invoke(this);
            _isDead = true;
            Destroy(gameObject, 1f);
        }
    }
}
