using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    //public ParticleSystem ShootEffect;
    public AudioSource audioSource;
    public Rigidbody Bullet;
    public Transform FirePoint;
    public float FireRate;

    private float delay = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        delay -= Time.deltaTime;

        if(Input.GetKey(KeyCode.Mouse0))
        {
            
            Shoot();
            
        }
    }

    //Fire bullet
    void Shoot()
    {
        if (delay <= 0)
        {
            audioSource.Play();
            Rigidbody rigidbodyInstance;
            rigidbodyInstance = Instantiate(Bullet, FirePoint.position, FirePoint.rotation) as Rigidbody;
            rigidbodyInstance.AddForce(FirePoint.forward * 1000);
            delay = FireRate;
        }
    }
}
