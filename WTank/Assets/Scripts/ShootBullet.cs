using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    //settings
    public int magazinSize = 5;
    public float reloadTimeForMagazin = 1.5f;
    public float reloadTimeForShot = 0.03f;
    public int shots;
    private float relodadMagazinTimeStamp;
    private float relodadShotTimeStamp;

    public GameObject bulletPrefab;
    // bullet settings

    // Start is called before the first frame update
    void Start()
    {
        shots = magazinSize;
        relodadMagazinTimeStamp = Time.time;
        relodadShotTimeStamp = Time.time - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (shots < magazinSize)
        {
            if (relodadMagazinTimeStamp + reloadTimeForMagazin < Time.time)
            {
                shots++;
                relodadMagazinTimeStamp = Time.time;
            }
        }
        else
        {
            relodadMagazinTimeStamp = Time.time;
        }
    }

    public void ShootingBullet()
    {
        if (shots > 0)
        {
            if (relodadShotTimeStamp + reloadTimeForShot < Time.time)
            {
                var shotBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                shotBullet.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward) * bulletPrefab.GetComponent<BulletManager>().bulletSpeed;
                shots--;
                relodadShotTimeStamp = Time.time;
            } 
            
        }
        
    }
}
