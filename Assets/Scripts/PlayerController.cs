using System.Collections;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private GameObject _focalPoint;
    [SerializeField] private float _gravityModifier = 2f;
    [SerializeField] private float _powerUpStrength = 15f;
    [SerializeField] private GameObject _powerUpIndicator;
    private InputSystem_Actions _playerInput;
    private Rigidbody _rb;
    private Vector2 _moveInput;
    private Coroutine _powerUpTimeOut;

    private bool _hasPowerUp;


    private void Awake()
    {
        _playerInput = new InputSystem_Actions();
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Player.Move.performed += OnMove;
        _playerInput.Player.Move.canceled += OnMove;
        _playerInput.Player.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        _playerInput.Player.Move.performed -= OnMove;
        _playerInput.Player.Move.canceled -= OnMove;
        _playerInput.Player.Jump.performed -= OnJump;
        _playerInput.Disable();
    }

    private void OnMove(InputAction.CallbackContext callback)
    {
        _moveInput = callback.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext callback)
    {
        if(callback.performed)
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }
    private void Update()
    {
        _powerUpIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
    }

    private void FixedUpdate()
    {
        MovePlayer();
        if(_rb.linearVelocity.y < 0)
        {
            _rb.AddForce(Physics.gravity * _gravityModifier, ForceMode.Acceleration);
        }
    }

    private void MovePlayer()
    {
        Vector3 direction = _moveInput.y * _focalPoint.transform.forward;
        _rb.AddForce(direction * Time.deltaTime * _speed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PowerUp"))
        {
            _hasPowerUp = true;
            Destroy(other.gameObject);
            _powerUpIndicator.gameObject.SetActive(true);
            if(_powerUpTimeOut == null)
            {
                _powerUpTimeOut = StartCoroutine(PowerUpCountdown());
            }
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && _hasPowerUp)
        {
            Rigidbody enemyRidgebody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            enemyRidgebody.AddForce(awayFromPlayer * _powerUpStrength, ForceMode.Impulse);
        }
    }

    private IEnumerator PowerUpCountdown()
    {
        yield return new WaitForSeconds(7);
        
        _powerUpIndicator.gameObject.SetActive(false);
        _hasPowerUp = false;
        _powerUpTimeOut = null;
    }

}
