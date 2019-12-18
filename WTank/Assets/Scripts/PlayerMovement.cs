using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{

    // player info
    public InformtionFromMenu passedInfoFroMenu;
    public bool alive = true;
    public string color;

    //filter
    protected Queue<Vector3> filterDataQueue = new Queue<Vector3>();
    public int filterLength = 5;

    // settings
    public float rotateSpeed = 1f;
    public float driveSpeed = 1;
    public int rotSteps = 5;

    // gun
    public GameObject gun;
    
    // joystick
    public GameObject joyStick;
    public Vector2 direction;

    // steering otherplayer
    public GameObject gameManager;

    // private
    private bool isRotating = false;

    //path print
    public GameObject PathSprite;
    public float printPathTimeDelta = 0.5f;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < filterLength; i++) filterDataQueue.Enqueue(Input.acceleration);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(color);
        Debug.Log(passedInfoFroMenu.GetPlayerIndex());
        if (color == "Blue" && passedInfoFroMenu.GetPlayerIndex() == 1)
        {
            // this player tank
            direction = LowPassJoystick();
            gameManager.GetComponent<GameScene>().SetDirectionOfThisPlayer(direction);
        }
        else
        {
            // other player tank. need to receive jostick info
            direction = gameManager.GetComponent<GameScene>().directionOtherPlayer;
        }

        
        if (direction.magnitude > 0 && alive)
        {
            Debug.Log("should drive");
            RotateTank();
            DriveTank();

        }
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
        float destYRot = GetJoystickRotation();

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

        // inheritate gun
        Vector3 rot = gun.transform.eulerAngles;
        transform.eulerAngles = tmpRot;
        gun.transform.eulerAngles = rot;

    }

    protected void DriveTank()
    {
        if (isRotating)
        {
            return;
        }
        GetComponent<NavMeshAgent>().Move(transform.TransformVector(new Vector3(driveSpeed * 3 * Time.deltaTime, 0, 0)));
    }

    private float GetJoystickRotation()
    {
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

        destYRot /= rotSteps;
        destYRot = Mathf.RoundToInt(destYRot);
        destYRot *= rotSteps;
        return destYRot;
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
        } else
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

    protected Vector3 LowPassJoystick()
    {
        if (filterLength <= 0) return joyStick.GetComponent<VariableJoystick>().Direction;
        filterDataQueue.Enqueue(joyStick.GetComponent<VariableJoystick>().Direction);
        filterDataQueue.Dequeue();

        Vector3 vFiltered = Vector3.zero;
        foreach (Vector3 v in filterDataQueue) vFiltered += v;
        vFiltered /= filterLength;
        return vFiltered;
    }
}
