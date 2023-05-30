using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float distance = 20;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float _yOffset;
    [SerializeField] private float _smoothing;

    private Vector3 offset;

    private void Start() =>
        offset = transform.rotation * Vector3.back * distance;

    private void Update()
    {
        Vector3 targetCamPos = (target.position + offset + cameraOffset).WithY(_yOffset);
        transform.position = Vector3.Lerp(transform.position,
                                          targetCamPos,
                                          _smoothing * Time.deltaTime);
    }
}
