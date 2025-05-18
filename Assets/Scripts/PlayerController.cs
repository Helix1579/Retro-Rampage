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
    public bool IsFacingRight => m_FacingRight;

    
    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
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
        m_FacingRight = !m_FacingRight;
        transform.Rotate(0f, 180f, 0f);

        // Optional: Flip weapon manually if needed
        Transform weapon = transform.Find("Weapon");
        if (weapon != null)
        {
            Vector3 scale = weapon.localScale;
            scale.x *= -1;
            weapon.localScale = scale;
        }
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
    
    public void ResetPlayer()
    {
        transform.position = startPosition;
        gameObject.SetActive(true);

        if (TryGetComponent(out BaseHealth health))
        {
            health.ResetHealth();
        }
        
        PlayerShooter shooter = FindObjectOfType<PlayerShooter>();
        if (shooter != null)
        {
            shooter.ResetWeapons();
        }
        // Add any other reset logic here: ammo, cooldowns, animations, etc.
    }

}