using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    public Transform targetObject;
    public Vector3 offset;
    private Vector3 cameraPosition;
    private bool mapView;

    public float followSpeed;
    public float rotationSpeed;

    public int mapSize;
    public int mapZOffset;

    Camera cameraComponent;

    void Start()
    {
        cameraComponent = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (mapView)
            {
                mapView = false;
            }
            else
            {
                mapView = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (!mapView)
        {
            // Reset the camera far clipping plane
            cameraComponent.farClipPlane = 1000;

            var targetPosition = targetObject.TransformPoint(offset);
            cameraPosition = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            transform.position = cameraPosition;

            var direction = targetObject.position - transform.position;
            var rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
        else if (mapView)
        {
            // Able the camera to see further to show the full game world on the map
            cameraComponent.farClipPlane = 2500;

            var cameraPosition = Vector3.up * mapSize + new Vector3(0, 0, mapZOffset);
            cameraPosition.x += 500;
            transform.position = cameraPosition;
            var rotation = new Vector3(90, 0, 0);
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}
