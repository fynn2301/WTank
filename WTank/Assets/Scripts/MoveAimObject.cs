using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class MoveAimObject : MonoBehaviour
{

    public LayerMask layerMask;
    public GameObject blueCrossHair;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastAim();
        

    }

    void RaycastAim()
    {
        RaycastHit hit;
        if (Physics.Raycast(blueCrossHair.transform.position, blueCrossHair.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(blueCrossHair.transform.position, blueCrossHair.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        }
        else
        {
            Debug.DrawRay(blueCrossHair.transform.position, blueCrossHair.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        }
        SetAimObj(hit.point);
    }
    void SetAimObj(Vector3 pos)
    {
        transform.position = pos;
    }
}
