using UnityEngine;

namespace Code.PlayerAnimation
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _movingName = "IsMoving";
        
        public void OnVelocityChanged(Vector2 velocity)
        {
            _animator.SetBool("IsMoving", velocity is not {x: 0, y: 0});
        }
    }
}
