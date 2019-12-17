using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public GameObject standardBulletSmokeObj;
    public GameObject destroyExplosionObj;

    public float bulletSpeed = 2;
    public int numBounces = 1;
    private bool oritate = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = GetComponent<Rigidbody>().velocity;
        if (oritate)
        {
            GetComponent<Rigidbody>().freezeRotation = false;
            transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);
            GetComponent<Rigidbody>().freezeRotation = true;
            oritate = false;
        }

    }
    public void OnCollisionEnter(Collision collision)
    {
        
        ObjectWasHit(collision);
        if (numBounces > 0 && collision.transform.tag == "Wall")
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            transform.position += collision.contacts[0].normal / 30;
            Vector3 outVec = Vector3.Reflect(transform.forward, collision.contacts[0].normal);
                
            Debug.Log(outVec.normalized * bulletSpeed);
            rigidbody.velocity = outVec.normalized * bulletSpeed;
            oritate = true;
            numBounces--;
        }
        else
        {
            DestroyBullet();
        }
    }

    private void ObjectWasHit(Collision collison)
    {

    }

    private void DestroyBullet()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        standardBulletSmokeObj.GetComponent<ParticleSystem>().Stop();
        Invoke("Die", 3);
        destroyExplosionObj.GetComponent<ParticleSystem>().Play();
        destroyExplosionObj.GetComponent<ParticleSystem>().Stop();

    }
    private void Die()
    {

        Destroy(gameObject);
    }
}
