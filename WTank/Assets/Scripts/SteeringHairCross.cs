using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SteeringHairCross : MonoBehaviour
{
    public float resetTimeAim = 0.2f;

    // filter
    protected Queue<Vector3> filterDataQueue = new Queue<Vector3>();
    public int filterLength = 1;
    public int moveSpeed = 1;

    // reset parameters
    Vector3 relativAccelPoint;
    bool accActiv = true;

    // Update is called once per frame
    void Start()
    {
        
        for (int i = 0; i < filterLength; i++) filterDataQueue.Enqueue(Input.acceleration); //filling the queue to requered length
        relativAccelPoint = Vector3.zero;
        //lastPos = Input.acceleration;
    }

    void Update()
    {
        if (accActiv)
        {
            MoveCrossHair();
        }
    }

    void MoveCrossHair()
    {
        Vector3 oldPos = Vector3.zero;


        // get acceleration factor and edit it
        Vector3 tempAccel = LowPassAccelerometer() - relativAccelPoint;

        Vector3 accelDelta = tempAccel;

        accelDelta.x *= 2.5f;
        transform.localPosition = oldPos + accelDelta * moveSpeed;

        Vector3 oldAimPos = transform.localPosition;
        oldAimPos.z = 1;
        if (oldAimPos.x <= -6.3f)
        {
            oldAimPos.x = -6.3f;
        }
        else if (oldAimPos.x >= 6.3f)
        {
            oldAimPos.x = 6.3f;
        }

        if (oldAimPos.y <= -3.5f)
        {
            oldAimPos.y = -3.5f;
        }
        else if (oldAimPos.y >= 3.4f)
        {
            oldAimPos.y = 3.4f;
        }
        transform.localPosition = oldAimPos;

    }

    public void ResetCenter()
    {
        accActiv = false;

        Invoke("ActivateAcc", resetTimeAim);
        StartCoroutine(MoveLocalOverSeconds(gameObject, Vector3.zero, resetTimeAim));
        
    }
    public IEnumerator MoveLocalOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.localPosition;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.localPosition = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.localPosition = end;
    }
    private void ActivateAcc()
    {
        relativAccelPoint = LowPassAccelerometer();
        accActiv = true;
    }
  
    public Vector3 LowPassAccelerometer()
    {
        if (filterLength <= 0) return Input.acceleration;
        filterDataQueue.Enqueue(Input.acceleration);
        filterDataQueue.Dequeue();

        Vector3 vFiltered = Vector3.zero;
        foreach (Vector3 v in filterDataQueue) vFiltered += v;
        vFiltered /= filterLength;
        return vFiltered;
    }
}