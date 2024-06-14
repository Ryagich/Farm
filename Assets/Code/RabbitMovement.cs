using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RabbitMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [Header("Idle Time")]
    [SerializeField, Min(.0f)] private float _min = .5f;
    [SerializeField, Min(.0f)] private float _max = 1.5f;

    private Animator animator;
    private BoxCollider collider;
    private MoveTo moveTo;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        moveTo = GetComponent<MoveTo>();
        moveTo.GotToPlace += () => StartCoroutine(Wait());
    }

    public void Init(BoxCollider collider)
    {
        this.collider = collider;
        target.SetParent(null);
    }

    public void FindNextPlace()
        =>target.position = new(Random.Range(collider.bounds.min.x,
                                           collider.bounds.max.x),
                                           0.9f,
                                Random.Range(collider.bounds.min.z,
                                           collider.bounds.max.z));

    public void MoveToTarget()
    {
        transform.LookAt(target);
        moveTo.Move(target, false);
        animator.SetBool("IsMoving", true);
    }

    private IEnumerator Wait()
    {
        animator.SetBool("IsMoving", false);
        yield return new WaitForSeconds(Random.Range(_min, _max));
        FindNextPlace();
        MoveToTarget();
    }
}
