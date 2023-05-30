using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineAnimate : MonoBehaviour
{
    [SerializeField] private float speed, min, max;
    [SerializeField] private Vector3 movement, scale;
    [SerializeField] private bool randomizeTime = false;

    private float t = 0;
    private Vector3 startPos, startScale;

    private void Start()
    {
        if (randomizeTime)
            t = Random.Range(0, Mathf.PI * 2);
        startPos = transform.localPosition;
        startScale = transform.localScale;
    }

    private void Update()   
    {
        t = (t + Time.deltaTime * speed) % (Mathf.PI * 2);
        var k = min + (Mathf.Sin(t) / 2 + 0.5f) * (max - min);
        transform.localPosition = startPos + movement * k;
        transform.localScale = new Vector3(
                                    Mathf.Lerp(startScale.x, k, scale.x),
                                    Mathf.Lerp(startScale.y, k, scale.y),
                                    Mathf.Lerp(startScale.z, k, scale.z));
    }
}
