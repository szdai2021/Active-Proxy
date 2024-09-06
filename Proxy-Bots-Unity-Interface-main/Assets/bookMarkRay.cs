using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bookMarkRay : MonoBehaviour
{
    private Transform rayOrigin; // The object from which the ray will originate
    private Vector3 rayDirection = Vector3.forward; // The direction in which the ray will be cast
    public float rayDistance = 100f; // Distance of the raycast
    public string targetName = "ZONE_DASHBOARD"; // The name of the object to detect

    public bool rayHitDetected = false;

    private void Start()
    {
        rayOrigin = this.transform;
        rayDirection = Vector3.up;
    }

    void Update()
    {
        // Create a ray from the rayOrigin in the specified direction
        Ray ray = new Ray(rayOrigin.position, rayDirection);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            print(hit.collider.gameObject.name);
            // Check if the ray hit an object with the specific name
            if (hit.collider.gameObject.name == targetName)
            {
                rayHitDetected = true;
            }
            else
            {
                rayHitDetected = false;
            }
        }
        else
        {
            rayHitDetected = false;
        }
    }
}
