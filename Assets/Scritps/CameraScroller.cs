using UnityEngine;
using System.Collections;

public class CameraScroller : MonoBehaviour
{
    public float minX = 0.0f, maxX = 50.0f, minY = 0.0f, maxY = 50.0f;
    public float scrollSpeed = 50.0f;
    public float scrollPercentage = 0.05f;

    // Use this for initialization
    void Start()
    {
    
    }
    
    // Update is called once per frame
    void Update()
    {
        Vector3 viewPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector3 nodePos = Vector3.zero;

        if (viewPoint.x < scrollPercentage)
            nodePos.x -= 1.0f;

        if (viewPoint.x > 1.0f - scrollPercentage)
            nodePos.x += 1.0f;

        if (viewPoint.y < scrollPercentage)
            nodePos.z -= 1.0f;

        if (viewPoint.y > 1.0f - scrollPercentage)
            nodePos.z += 1.0f;

        Vector3 boundPos = transform.position + nodePos.normalized * Time.deltaTime * scrollSpeed;

        if (boundPos.x < minX)
            boundPos.x = minX;

        if (boundPos.x > maxX)
            boundPos.x = maxX;

        if (boundPos.z < minY)
            boundPos.z = minY;

        if (boundPos.z > maxY)
            boundPos.z = maxY;

        transform.position = boundPos;
    }
}
