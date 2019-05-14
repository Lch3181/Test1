using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    static Animator animator;

    private CharacterController controller;
    public GameObject Mesh;
    private Vector3 moveDirection = Vector3.zero;
    private float inputH, inputV;
    private bool LeftShift;
    public float moveSpeed;
    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //get key input
        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");
        LeftShift = Input.GetKey(KeyCode.LeftShift);

        Rotation();

        Movement();
    }

    //Rotation
    void Rotation()
    {
        //ray from the mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //ray target ( ground )
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        //math
        float distance;
        if(plane.Raycast(ray, out distance))
        {
            //get distance
            Vector3 target = ray.GetPoint(distance);
            //get direction
            Vector3 direction = target - transform.position;
            //get rotation
            float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            //apply
            transform.rotation = Quaternion.Euler(0, rotation, 0);
            //Mesh.transform.rotation = Quaternion.Euler(0, rotation, 0);
        }


    }

    //Player movement
    void Movement()
    {
        
        // is the controller on the ground?
        if (controller.isGrounded)
        {
            //Feed moveDirection with input.
            moveDirection = new Vector3(inputH, 0, inputV);
            moveDirection = transform.TransformDirection(moveDirection);
            //Multiply it by speed.
            moveDirection *= moveSpeed;
            //Jumping
            //if (Input.GetButton("Jump"))
            //    moveDirection.y = moveSpeed;

        }
        //Applying gravity to the controller
        moveDirection.y -= 20f * Time.deltaTime;
        //Making the character move
        if(LeftShift)//run
        {
            controller.Move(moveDirection * 5f * Time.deltaTime);
        }
        else//walk
        {
            controller.Move(moveDirection * Time.deltaTime);
        }
        

        //Animation
        animator.SetFloat("inputH", inputH);
        animator.SetFloat("inputV", inputV);
        animator.SetBool("run", LeftShift);
        animator.SetBool("Fire", Input.GetButton("Fire1"));
    }

}
