using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float health;
    public float damage;
    private float velocity;
    public Transform Target;
    private NavMeshAgent agent;
    static Animator animator;
    private bool Dead = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && !Dead)
        {
            GetComponent<CapsuleCollider>().height = 0.3f;
            agent.height = 0;
            agent.enabled = false;
            animator.Play("Dead");
            Dead = !Dead;
        }
        else if(health>0)
        {
            //set to chase target
            agent.SetDestination(Target.position);
            //face the target
            FaceTarget();
            //get velocity
            velocity = agent.velocity.magnitude / agent.speed;

            //Animation
            animator.SetFloat("Speed", velocity);
            animator.SetBool("Attack", agent.remainingDistance < 2);
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (Target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
    }

    public void takeDamage(float dmg)
    {
        health -= dmg;
    }

}
