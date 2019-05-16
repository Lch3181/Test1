using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    static Animator animator;

    private CharacterController controller;
    private Gun gun;
    public GameObject Mesh;
    private Vector3 moveDirection = Vector3.zero;
    public float health;
    public Slider healthBar;
    public float maxHealth;
    public float moveSpeed;
    private bool Dead;
    public bool ControllerEnable;
    private float rotation;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        health = maxHealth;
        gun = GetComponentInChildren<Gun>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Dead)
        {

            Rotation();

            Movement();

            UI();
        }
    }

    //Rotation
    void Rotation()
    {
        if (!ControllerEnable)
        {
            //ray from the mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //ray target ( ground )
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            //math
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                //get distance
                Vector3 target = ray.GetPoint(distance);
                //get direction
                Vector3 direction = target - transform.position;
                //get rotation
                rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                //apply
                Mesh.transform.rotation = Quaternion.Euler(0, rotation, 0);
            }
        }
        else if (ControllerEnable && Input.GetAxis("Horizontal1") != 0 || Input.GetAxis("Vertical1") != 0)
        {
            //get rotation
            rotation = Mathf.Atan2(Input.GetAxis("Horizontal1"), Input.GetAxis("Vertical1")) * Mathf.Rad2Deg;
            //apply
            Mesh.transform.rotation = Quaternion.Euler(0, rotation, 0);
        }



    }

    //Player movement
    void Movement()
    {

        if (health <= 0)
        {
            GetComponentInChildren<LineRenderer>().enabled = false;
            controller.height = 0.6f;
            Dead = !Dead;
            animator.Play("Death from the front");

        }
        else if (health > 0)
        {

            // is the controller on the ground?
            if (controller.isGrounded)
            {
                //Feed moveDirection with input.
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);
                //Multiply it by speed.
                moveDirection *= moveSpeed;

            }
            //Applying gravity to the controller
            moveDirection.y -= 20f * Time.deltaTime;
            //Making the character move
            if (Input.GetAxis("Run") >= 0.1)//run
            {
                controller.Move(moveDirection * 5f * Time.deltaTime);
            }
            else//walk
            {
                controller.Move(moveDirection * Time.deltaTime);
            }

            //shoot
            if (Input.GetAxis("Fire1") >= 0.1f && animator.GetCurrentAnimatorStateInfo(0).IsName("Stand Fire") || animator.GetCurrentAnimatorStateInfo(0).IsName("Walk Fire") || animator.GetCurrentAnimatorStateInfo(0).IsName("Run Fire"))
            {
                gun.Shoot();
            }

            //convert degree to rad
            float rad = rotation * Mathf.Deg2Rad;

            //get new x,0,z after rotation
            float x = (controller.velocity.x * Mathf.Cos(rad) - controller.velocity.z * Mathf.Sin(rad));
            float z = (controller.velocity.x * Mathf.Sin(rad) + controller.velocity.z * Mathf.Cos(rad));
            Vector3 v3 = new Vector3(x, 0, z);

            //Animation
            animator.SetFloat("inputH", x / v3.magnitude >= 0.1 || x / v3.magnitude <= 0.1 ? x / v3.magnitude : 0);
            animator.SetFloat("inputV", z / v3.magnitude >= 0.1 || z / v3.magnitude <= 0.1 ? z / v3.magnitude : 0);
            animator.SetBool("run", Input.GetAxis("Run") >= 0.1);
            animator.SetBool("Fire", Input.GetAxis("Fire1") >= 0.1);
        }
    }

    //UI
    void UI()
    {
        healthBar.value = healthbar();
    }
    float healthbar()
    {
        return health / maxHealth;
    }

    //take damage
    public void takeDamage(float dmg)
    {
        health -= dmg;
    }

}
