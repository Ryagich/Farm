using UnityEngine;

namespace Code.PlayerMovement
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerGravity : MonoBehaviour
    {
        [SerializeField] private CharacterController _controller;
        [SerializeField, Min(.0f)] private float _g = 10f;

        private float g;
        private void Update()
        {
            if (_controller.isGrounded)
            {
                g = 0;
                return;
            }
            g += _g;
            _controller.Move(-transform.up * (g * Time.deltaTime));
        }
    }
}
