using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public ParticleSystem ShootEffect;
    public GameObject Bullet;
    public GameObject FirePoint;
    public float FireRate;

    private float delay = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (ShootEffect = GetComponent<ParticleSystem>())
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Fire point position: "+FirePoint.transform.position);

        delay -= Time.deltaTime;

        if(Input.GetButton("Fire1"))
        {
            Shoot();
        }
    }

    //Fire bullet
    void Shoot()
    {
        if (delay <= 0)
        {
            Instantiate(Bullet, FirePoint.transform.position, FirePoint.transform.rotation);
            delay = FireRate;
        }
    }
}
