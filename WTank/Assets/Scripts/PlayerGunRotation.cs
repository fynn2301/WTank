using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunRotation : MonoBehaviour
{
    public GameObject aimObject;

    public Quaternion rotation;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RotateGunTube(aimObject);
    }

    void RotateGunTube(GameObject aimObj)
    {
        Vector3 aimPos = aimObj.transform.position;
        Vector3 gunPos = transform.position;

        float yRotation = Mathf.Atan((aimPos.z - gunPos.z) / (aimPos.x - gunPos.x));
        Vector3 oldRot = transform.eulerAngles;
        oldRot.y = -yRotation * 180 / Mathf.PI;
        if (aimPos.x < gunPos.x)
        {
            oldRot.y += 180;
        }
        
        transform.eulerAngles = oldRot;
        
        //gun.transform.eulerAngles =
    }
}
