using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Code.PlayerMovement
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Vector2> _velocityChanged;
        [SerializeField] private InputActionReference _moveInput;
        [SerializeField] private CharacterController _controller;
        [SerializeField, Min(.0f)] private float _speed = 5f;

        private Vector2 velocity;

        private void Awake()
        {
            _moveInput.action.performed += OnTryMove;
            _moveInput.action.canceled += OnTryMove;
        }

        private void OnTryMove(InputAction.CallbackContext context)
        {
            var newVelocity = context.ReadValue<Vector2>();
            if (GameStateController.Instance.GameState == GameStates.Redactor)
            {
                newVelocity = Vector2.zero;
            }
            //Почему то не работает
            //if (!_controller.isGrounded) { newVelocity = Vector2.zero; }
            velocity = newVelocity;
            GameStateController.Instance.IsMoving = velocity is not { x: 0, y: 0 };
            _velocityChanged?.Invoke(velocity);
        }

        private void Update()
        {
            if (velocity is { x: 0, y: 0 })
            {
                return;
            }
            // TODO: передавать ссылку на камеру
            var moveDirection = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0) *
                                new Vector3(velocity.x, 0, velocity.y);
            var angle = Mathf.Rad2Deg * Mathf.Atan2(moveDirection.x, moveDirection.z);
            var t = transform;
            t.rotation = Quaternion.Euler(0, angle, 0);
            _controller.Move(t.forward * (_speed * Time.deltaTime));
        }
    }
}