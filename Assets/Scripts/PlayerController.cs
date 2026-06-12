using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private GameObject _focalPoint;
    [SerializeField] private float _gravityModifier = 2f;
    private InputSystem_Actions _playerInput;
    private Rigidbody _rb;
    private Vector2 _moveInput;


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


}
