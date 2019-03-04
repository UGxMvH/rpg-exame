using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;

    public Transform target;
    [Range(0.1f, 1f)]
    public float smoothness;

    // Update is called once per frame
    void Update()
    {
        // Get target location
        Vector3 targetPos = target.position;
        targetPos.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothness);
    }
}
