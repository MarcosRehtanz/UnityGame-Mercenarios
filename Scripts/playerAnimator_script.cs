using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimator_script : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Animator animator;

    [SerializeField] private bool grounded;
    [SerializeField] private float velocityY;
    [SerializeField] private Vector2 move;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = controller.isGrounded;
        velocityY = controller.velocity.y;
        move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); 

        if (velocityY > 0)
        {
            animator.SetBool("Jump", true);
        } else if (grounded && velocityY<0)
        {
            animator.SetBool("Jump", false);
        }

        if (move != Vector2.zero && velocityY > -3)
        {
            animator.SetBool("Walk", true);
        } else
        {
            animator.SetBool("Walk", false);
        }

    }
}
