using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RayTest : MonoBehaviour
{
    [SerializeField]private Vector3 originalPos;
    [SerializeField]private float length;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(originalPos, Vector3.up);
        RaycastHit hitInfo;
        /* if (Physics.Raycast(ray, out hitInfo, 5f,1<<LayerMask.NameToLayer("Ground")))
        {
            Debug.Log(hitInfo.collider.name);
        }
        Debug.DrawLine(ray.origin,ray.direction*5f,Color.red);
        ray = new Ray(originalPos, Vector3.down);*/
        if (Physics.Raycast(ray, out hitInfo, length,1<<LayerMask.NameToLayer("Ground")))
        {
            Debug.Log(hitInfo.collider.name);
        }
        Debug.DrawLine(ray.origin,ray.origin+ray.direction*length,Color.white);
        
    }
}
