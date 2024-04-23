using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveByClick : MonoBehaviour
{

    public UnityEvent OnClick; 

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Check if the mouse click collided with this object
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the raycast hit this object
                // Click collision occurred with this object
                Debug.Log("Clicked on object: " + gameObject.name);
                transform.position = hit.point;
                if (OnClick != null) OnClick.Invoke();
            }
        }

    }
}
