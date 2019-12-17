using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletImpact : MonoBehaviour
{
    public GameObject ExplosionEffectObj;
    public GameObject DeadCrossObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Bullet")
        {
            TankWasHit();
            Debug.Log("Died");
        }
    }

    private void TankWasHit()
    {

        MeshRenderer[] tankMeshes = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer meshRend in tankMeshes)
        {
            meshRend.enabled = false;
        }
        GetComponent<BoxCollider>().enabled = false;
        ExplosionEffectObj.GetComponent<ParticleSystem>().Play();
        ExplosionEffectObj.GetComponent<ParticleSystem>().Stop();
        DeadCrossObj.GetComponent<SpriteRenderer>().enabled = true;
        DeadCrossObj.transform.eulerAngles = new Vector3(90, 0, 0);
        GetComponentInParent<PlayerMovement>().alive = false;
    }
}
