using UnityEngine;

public class Enemy : MonoBehaviour
{
   [SerializeField] private float _speed;
   [SerializeField] private GameObject _player;

   private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Chase Player
        Vector3 direction = (_player.transform.position - transform.position).normalized;
        _rb.AddForce(direction * _speed * Time.deltaTime, ForceMode.Impulse);
    }
}
