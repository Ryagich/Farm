using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private ParticleSystem particleS;

    private Animator animator;
    private Vector3 moveDirection = Vector3.zero;

    private void Awake() => animator = GetComponent<Animator>();

    private void FixedUpdate()
    {
        moveDirection = new Vector3(_joystick.Horizontal,
                                    moveDirection.y,
                                    _joystick.Vertical);
        moveDirection = Quaternion.Euler(
            0, Camera.main.transform.rotation.eulerAngles.y, 0) * moveDirection;

        var isMoving = moveDirection.magnitude >= 0.181;
        animator.SetBool("IsMoving", isMoving);
        if (!isMoving)
        {
            particleS.enableEmission = false;
            return;
        }
        particleS.enableEmission = true;
        var angle = Mathf.Rad2Deg * Mathf.Atan2(moveDirection.x, moveDirection.z);
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}
