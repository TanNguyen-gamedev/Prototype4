using System.Collections;
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
    [SerializeField] private PowerUpManager _powerUpManager;


    private bool _isSmashing = false;

    public void SetSmashing(bool value) => _isSmashing = value; 

    private InputSystem_Actions _playerInput;
    private Rigidbody _rb;
    private Vector2 _moveInput;
    private bool _hasKnockback = false;


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
        if(other.TryGetComponent<PowerUp>(out var powerUp))
        {
            _powerUpManager.ActivePowerUp(powerUp.PowerType, powerUp.Duration);
            Destroy(powerUp.gameObject);
            _powerUpIndicator.gameObject.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && _hasKnockback)
        {
               Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            if(enemyRb)
            {
                Vector3 awayFromPlayer = collision.transform.position - transform.position;
                awayFromPlayer.y = 0f;

                enemyRb.AddForce(awayFromPlayer.normalized * _powerUpStrength, ForceMode.Impulse);
            }
        }


        if (_isSmashing && collision.gameObject.CompareTag("Ground"))
        {
            _isSmashing = false;
            _powerUpManager.ApplySmashExplosion();
        }
    }

    public void SetKnockback(bool hasKnockback)
    {
        _hasKnockback = hasKnockback;
    }

    public void TurnOffTheIndicator()
    {
        _powerUpIndicator.SetActive(false);
    }

    public IEnumerator ApplySmash()
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
        _rb.AddForce(Vector3.down * _jumpForce * 3f, ForceMode.Impulse);
        SetSmashing(true);
    }


}
