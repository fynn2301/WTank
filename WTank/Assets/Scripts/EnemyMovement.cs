using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{

    public Vector3 driveDestination;
    public float rotateSpeed;

    private NavMeshPath path;
    private float elapsed = 0.0f;
    private bool isRotating = false;

    private Vector3 tempRotation;
    private float destinationYRotation;
    void Start()
    {
        path = new NavMeshPath();
        elapsed = 0.0f;
    }

    void Update()
    {
        // Update the way to the goal every second.
        elapsed += Time.deltaTime;
        if (elapsed > 1.0f)
        {
            elapsed -= 1.0f;
            

        }
        NavMesh.CalculatePath(transform.position, driveDestination, NavMesh.AllAreas, path);
        //GetComponent<NavMeshAgent>().SetDestination(driveDestination);
        for (int i = 0; i < path.corners.Length - 1; i++)
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        
            
    }

    private void Drive()
    {

    }

    protected void RotateTank()
    {
        isRotating = true;
        Vector3 tmpRot = transform.eulerAngles;

        if (tmpRot.y > 360)
        {
            transform.eulerAngles = new Vector3(tmpRot.x, tmpRot.y - 360, tmpRot.z);
        }
        else if (tmpRot.y <= 0)
        {
            transform.eulerAngles = new Vector3(tmpRot.x, tmpRot.y + 360, tmpRot.z);
        }
        float tmpYRot = transform.eulerAngles.y;
        float destYRot = GetDestinationRotation();

        // rotate

        float rotDif = RotDif(destYRot, tmpYRot);
        if (Mathf.Abs(rotDif) < rotateSpeed)
        {

            tmpRot.y = destYRot;
            isRotating = false;
        }
        else
        {
            if (rotDif < 0)
            {
                tmpRot.y -= rotateSpeed * 50 * Time.deltaTime;
            }
            else if (rotDif >= 0)
            {
                tmpRot.y += rotateSpeed * 50 * Time.deltaTime;
            }
            else
            {
                isRotating = false;
                return;
            }
        }

    }

    protected float RotDif(float destRot, float tmpRot)
    {

        float altTmpRot = tmpRot;


        if (altTmpRot <= 180)
        {
            altTmpRot += 180;
        }
        else
        {
            altTmpRot -= 180;
        }
        float tmp0Pos;
        float tmp0Neg;
        float tmp1Pos;
        float tmp1Neg;
        // tmp0
        if (tmpRot >= destRot)
        {
            tmp0Neg = destRot - tmpRot;
            tmp0Pos = (360 - tmpRot) + destRot;
        }
        else
        {
            tmp0Neg = -tmpRot - (360 - destRot);
            tmp0Pos = destRot - tmpRot;
        }
        if (altTmpRot >= destRot)
        {
            tmp1Neg = destRot - altTmpRot;
            tmp1Pos = (altTmpRot) + destRot;
        }
        else
        {
            tmp1Neg = -altTmpRot - (360 - destRot);
            tmp1Pos = destRot - altTmpRot;
        }
        float[] floats = { tmp0Pos, tmp0Neg, tmp1Pos, tmp1Neg };
        return GetFloatWithSmallestAbs(floats);
    }

    protected float GetFloatWithSmallestAbs(float[] floats)
    {
        for (int i = 0; i < floats.Length; i++)
        {
            for (int j = 0; j < floats.Length; j++)
            {
                if (j == i && i == floats.Length - 1) return floats[i];
                if (j == i) continue;

                if (Mathf.Abs(floats[i]) <= Mathf.Abs(floats[j]))
                {
                    if (j == floats.Length - 1) return floats[i];
                    continue;
                }
                else
                {
                    break;
                }
            }
        }
        Debug.Log("algorithm not working");
        return 0;
    }

    private float GetDestinationRotation()
    {
        if (path.corners.Length < 2)
        {
            return 0;
        }
        Vector3 direction = path.corners[1] - path.corners[0];
        float destYRot = Mathf.Atan(direction.y / direction.x) * (180 / Mathf.PI);
        if (direction.x < 0)
        {
            destYRot += 180;
        }
        if (destYRot <= 0)
        {
            destYRot = 360 + destYRot;
        }
        destYRot = 360 - destYRot;
        destYRot = Mathf.RoundToInt(destYRot);
        return destYRot;
    }
}
