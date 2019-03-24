using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SmoothCamera : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;

    public bool smooth = true;
    public Transform target;
    [Range(0.1f, 1f)]
    public float smoothness = 0.5f;
    public Vector2 offset;

    private void Start()
    {
        PixelPerfectCamera pixelCam = GetComponent<PixelPerfectCamera>();

        if (pixelCam)
        {
            pixelCam.refResolutionX = Screen.width;
            pixelCam.refResolutionY = Screen.height;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!target) return;

        if (!smooth)
        {
            Vector3 pos = target.position;
            pos.z = transform.position.z;
            pos.x += offset.x;
            pos.y += offset.y;

            transform.position = pos;
            return;
        }

        // Get target location
        Vector3 targetPos = target.position;
        targetPos.z = transform.position.z;
        targetPos.x += offset.x;
        targetPos.y += offset.y;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothness);
    }
}
