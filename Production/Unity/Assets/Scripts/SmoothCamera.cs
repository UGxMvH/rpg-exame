using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;

    public Transform target;
    [Range(0.1f, 1f)]
    public float smoothness = 0.5f;
    public Vector2 offset;

    // Update is called once per frame
    void Update()
    {
        if (!target) return;

        // Get target location
        Vector3 targetPos = target.position;
        targetPos.z = transform.position.z;
        targetPos.x += offset.x;
        targetPos.y += offset.y;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothness);
    }
}
