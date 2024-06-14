using UnityEngine;
using UnityEngine.Serialization;

public class CameraMovement : MonoBehaviour
{
    [FormerlySerializedAs("target")] [SerializeField] private Transform _target;
    [FormerlySerializedAs("distance")] [SerializeField] private float _distance = 20;
    [FormerlySerializedAs("cameraOffset")] [SerializeField] private Vector3 _cameraOffset;
    [SerializeField] private float _yOffset;
    [SerializeField] private float _smoothing;

    private void Update()
    {
        var offset = transform.rotation * Vector3.back * _distance;
        var targetCamPos = (_target.position + offset + _cameraOffset).WithY(_yOffset);
        transform.position = Vector3.Lerp(transform.position,
                                          targetCamPos,
                                          _smoothing * Time.deltaTime);
    }
}
