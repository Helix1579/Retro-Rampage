using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Audio")] public AudioClip jumpClip;
    public AudioClip runClip;

    public AudioSource audioSource;
    private bool isRunningSoundPlaying = false;

    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private float moveInput;
    private bool isGrounded;
    private bool m_FacingRight = true;
    private Vector3 startPosition;

    public float speed;
    public float jumpForce;
    public Rigidbody2D RigidBody;
    [SerializeField] private Animator animator;
    public bool IsFacingRight => m_FacingRight;

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

        animator.SetBool("isRunning", moveInput != 0);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            RigidBody.AddForce(new Vector2(RigidBody.linearVelocity.x, jumpForce));
            animator.SetBool("isJumping", true);

            if (jumpClip != null)
            {
                audioSource.PlayOneShot(jumpClip);
            }
        }

        // ✅ Running sound
        if (Mathf.Abs(moveInput) > 0.1f && isGrounded)
        {
            if (!isRunningSoundPlaying && runClip != null)
            {
                audioSource.clip = runClip;
                audioSource.loop = true;
                audioSource.Play();
                isRunningSoundPlaying = true;
            }
        }
        else
        {
            if (isRunningSoundPlaying)
            {
                audioSource.Stop();
                isRunningSoundPlaying = false;
            }
        }


    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        transform.Rotate(0f, 180f, 0f);

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
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
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
    }
}