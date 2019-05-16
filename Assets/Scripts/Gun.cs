using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Gun : MonoBehaviour
{
    public float damage;
    public float fireRate;
    public float Range;
    public float Force;
    public Transform RayOrigin;
    public Transform Firepoint;

    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);
    private AudioSource gunAudio;
    public LineRenderer laserLine;
    private float nextFire;

    // Start is called before the first frame update
    void Start()
    {
        gunAudio = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shoot()
    {
        if(Time.time>nextFire)
        {
            //effect
            StartCoroutine(ShotEffect());
            //fire delay
            nextFire = Time.time + fireRate;
            //ray cast
            RaycastHit hit;
            //set laser position to player.orgin
            laserLine.SetPosition(0, Firepoint.position);
            //check if ray hit
            if(Physics.Raycast(RayOrigin.position, RayOrigin.TransformDirection(Vector3.forward), out hit, Range))
            {
                //set laser end point to hit point
                laserLine.SetPosition(1, hit.point);
                //hit enemy
                if (hit.collider.tag=="Enemy")
                {
                    hit.collider.GetComponent<Enemy>().takeDamage(damage);
                    StartCoroutine(ForceEffect(hit.collider.GetComponent<NavMeshAgent>()));
                }

                //add force
                if(hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * Force);
                }
            }
            else//else just draw the laser
            {
                laserLine.SetPosition(1, Firepoint.position + RayOrigin.TransformDirection(Vector3.forward) * Range);
            }

        }
        
    }

    //effect
    private IEnumerator ShotEffect()
    {
        gunAudio.Play();
        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false;
    }

    private IEnumerator ForceEffect(NavMeshAgent agent)
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(0.5f);
        if (agent.isActiveAndEnabled)
        {
            agent.isStopped = false;
        }
        
    }

}
