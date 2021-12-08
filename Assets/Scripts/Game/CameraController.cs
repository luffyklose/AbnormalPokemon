using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform targetTransform;

    public float smoothing;

    public Vector2 maxPosition;
    public Vector2 minPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //If the camera isn't on the same position(x and y axis), it will change position to player's position.
        if (transform.position != targetTransform.position)
        {
            Vector3 targetPosition =
                new Vector3(targetTransform.position.x, targetTransform.position.y, transform.position.z);
            targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        }
        /*transform.position = new Vector3(targetTransform.transform.position.x, targetTransform.transform.position.y,
            targetTransform.transform.position.z - 10.0f);*/
    }
}
