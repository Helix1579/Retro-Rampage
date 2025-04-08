using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    public float speed;
    public float jumpForce;
    private float moveInput;
    private bool isGrounded;
    public Rigidbody2D RigidBody;
    [SerializeField] private Animator animator;
    
    private bool m_FacingRight = true;
    
    private void Start()
    {

    }
    private void Update()
    {
        moveInput = Input.GetAxis("Horizontal");
        RigidBody.linearVelocity = new Vector2(moveInput * speed, RigidBody.linearVelocity.y);
            if (moveInput > 0 && !m_FacingRight)
            {
                Flip();
            }
            else if (moveInput < 0 && m_FacingRight)
            {
                Flip();
            }
        
        animator.SetBool("isRunning", moveInput != 0 );

        if (Input.GetButtonDown("Jump") && isGrounded == false)
        {
            RigidBody.AddForce(new Vector2(RigidBody.linearVelocity.x, jumpForce));
            animator.SetBool("isJumping", true);
        }
        

    }
    
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        transform.Rotate(0f, 180f, 0f);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("isJumping", false);
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}