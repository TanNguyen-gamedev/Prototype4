using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _turnRate = 300f;
    [SerializeField] private float _impactStrength = 100f;
    private Transform _targetTransform;
    private Rigidbody _rb;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        // Ensure the model is rotated correctly so it lies horizontally
        transform.GetChild(0).localRotation = Quaternion.Euler(90f, 0f, 0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_targetTransform == null)
        {
            _rb.linearVelocity = transform.forward * _speed;
            return;
        } 

        //Calculate the direction to the target
        Vector3 direction = (_targetTransform.position - transform.position).normalized;

        Quaternion deltaRotation = Quaternion.LookRotation(direction);
        _rb.MoveRotation(Quaternion.Slerp(
            transform.rotation, 
            deltaRotation,
            _turnRate * Time.fixedDeltaTime)
        );

        _rb.linearVelocity = transform.forward * _speed;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            if(enemyRb)
            {
                Vector3 awayFromProjectile = collision.transform.position - transform.position;
                awayFromProjectile.y = 0f;

                enemyRb.AddForce(awayFromProjectile.normalized * _impactStrength, ForceMode.Impulse);
            }
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform target)
    {
        _targetTransform = target;
        Destroy(gameObject, 5f);
    }
}
