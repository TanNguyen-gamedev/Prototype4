using UnityEngine;
using UnityEngine.InputSystem;

public class RotateCamera : MonoBehaviour
{
    [SerializeField] private float _turnRate = 120f;
    private InputSystem_Actions _playerInput;
    private Rigidbody _rb;
    private Vector2 _moveInput;

    private void Awake()
    {
        _playerInput = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Player.Move.performed += OnMove;
        _playerInput.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        _playerInput.Player.Move.performed -= OnMove;
        _playerInput.Player.Move.canceled -= OnMove;

        _playerInput.Disable();
    }

    private void OnMove(InputAction.CallbackContext callback)
    {
        _moveInput = callback.ReadValue<Vector2>();
    }


    private void FixedUpdate()
    {
        RotatePlatform();
    }

    private void RotatePlatform()
    {
        transform.Rotate(0, _moveInput.x * _turnRate * Time.fixedDeltaTime, 0);
    }

}
