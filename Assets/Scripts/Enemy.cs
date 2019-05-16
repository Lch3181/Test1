using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private float health;
    public float maxHealth;
    public GameObject GUI;
    private Slider healthBar;
    public float damage;
    protected float velocity;
    private Transform Target;
    private NavMeshAgent agent;
    protected static Animator animator;
    private bool Dead = false;
    public float attackRate;
    private float attackCD;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Target = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        health = maxHealth;
        attackCD = attackRate;
        healthBar = GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (health <= 0 && !Dead)
        {
            Die();
            
        }
        else if(health>0)
        {
            //set to chase target
            if(agent.isActiveAndEnabled)
                agent.SetDestination(Target.position);

            //get velocity
            velocity = agent.velocity.magnitude / agent.speed;

            //Animation
            Animation();
                
           
        }
        //UI
        UI();
    }

    //Die
    void Die()
    {
        GetComponent<CapsuleCollider>().height = 0.3f;
        //agent.height = 0;
        agent.enabled = false;
        animator.Play("Dead");
        Dead = !Dead;
    }

    //UI
    void UI()
    {
        //health bar
        if (healthBar.value <= 0.99 && healthBar.value > 0)
            GUI.SetActive(true);
        else
            GUI.SetActive(false);

        healthBar.value = health / maxHealth;
    }

    //face the target
    void FaceTarget()
    {
        Vector3 direction = (Target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
    }

    //for other script to access take damage
    public void takeDamage(float dmg)
    {
        health -= dmg;
    }

    //animation
    protected virtual void Animation()
    {
        //set argument values
        animator.SetFloat("Speed", velocity);
        animator.SetBool("Attack", InRange() && attackCD <= 0);
        //attack
        attackCD -= Time.deltaTime;
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("attack")) //stop moving when attacking
        {
            agent.velocity = Vector3.zero;
        }
        else
        {
            //loop at target
            FaceTarget();
        }
    }

    //Animation event
    protected virtual void AttackEnd()
    {
        if(InRange())
        {
            Target.GetComponent<PlayerControl>().takeDamage(damage);
            attackCD = attackRate;
        }
        
    }
    //check if target still in range
    protected bool InRange()
    {
        if (agent.isActiveAndEnabled)
            return agent.remainingDistance <= agent.stoppingDistance;
        else
            return false;
    }

}
