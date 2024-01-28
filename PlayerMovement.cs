using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 8.0f;
    public int maxAerialJumps = 1;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public AudioClip jumpSound; // Reference to the jump sound.
    private bool punching = false;
    private Rigidbody2D rb;
    private int jumpsRemaining;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource; // Reference to the AudioSource component.
    private bool punchRecovery = false;
    public bool isGrounded = false;

    private bool canPlaySound = true; // Boolean to control when the sound can play.

    public Animator animator; // Assign this in the Unity Editor

    private BoxCollider2D attackCollider;
    private const float frameDuration = 1f / 60f; // For 60 FPS


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpsRemaining = maxAerialJumps;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(true); // Get the Sprite Renderer component from child objects.
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on child GameObjects");
        }
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component.
        // Update the Animator's parameter
        if (animator != null)
        {
            animator.SetBool("IsGrounded", isGrounded);
        }
        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();

        attackCollider = colliders[1]; // Get the player's collider
        attackCollider.enabled = false;
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) && !canPlaySound;

            // Update the Animator's parameter
            if (animator != null)
            {
                animator.SetBool("IsGrounded", isGrounded);
            }

            // Check for punch input
            if (isGrounded && !punching && !punchRecovery)
            {
                if (Input.GetKeyDown(KeyCode.F ))
                {
                    StartCoroutine(punch());
                }
            }

            // Handle movement and jumping
            if ((!punching && !punchRecovery) || ((punching || punchRecovery) && !isGrounded))
            {
                float horizontalInput = 0;
                if (Input.GetKey(KeyCode.A))
                {
                    horizontalInput = -1f;
                    Vector2 movement = new Vector2(horizontalInput, 0);
                    rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    horizontalInput = 1f; 
                    Vector2 movement = new Vector2(horizontalInput, 0);
                    rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
                }
                else if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) 
                {

                    Vector2 movement = new Vector2(horizontalInput, 0); 
                    rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y); 
                }

                if (horizontalInput > 0 && rb.constraints != (RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY))
                {
                    spriteRenderer.flipX = false;
                }
                else if (horizontalInput < 0 && rb.constraints != (RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY))
                {
                    spriteRenderer.flipX = true;
                }

                if (isGrounded)
                {
                    jumpsRemaining = maxAerialJumps;
                }

                // Check for jump input and remaining jumps
                if (Input.GetKeyDown(KeyCode.W) && rb.constraints != (RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY))
                {
                    if (isGrounded || jumpsRemaining > 0)
                    {
                        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                        if (!isGrounded)
                        {
                            jumpsRemaining--;
                        }

                        // Play the jump sound.
                        if (jumpSound != null)
                        {
                            audioSource.PlayOneShot(jumpSound, 0.6f);
                        }
                    }
                }
            }
            else
            {
                Vector2 movement = new Vector2(0, 0);
            }
        }
    }

    private IEnumerator punch()
    {
        bool wasXPositionFrozen = (rb.constraints & RigidbodyConstraints2D.FreezePositionX) != 0;

        if (isGrounded && !wasXPositionFrozen)
        {
            // Freeze the Rigidbody's X position
            rb.constraints |= RigidbodyConstraints2D.FreezePositionX;
        }
        punching = true;
        attackCollider.enabled = true;

        Vector2 originalSize = attackCollider.size;
        Vector2 originalOffset = attackCollider.offset;

        Vector2 targetOffset = spriteRenderer.flipX ? new Vector2(-0.95f, attackCollider.offset.y) : new Vector2(0.95f, attackCollider.offset.y);
        Vector2 targetSize = new Vector2(7.5f, attackCollider.size.y);

        // Expand the collider over 10 frames
        for (int i = 0; i < 10; i++)
        {
            float lerpFactor = (float)i / 10;
            attackCollider.size = Vector2.Lerp(originalSize, targetSize, lerpFactor);
            yield return new WaitForSeconds(frameDuration);
        }

        // Ensure the collider reaches the target size and offset
        attackCollider.size = targetSize;
        attackCollider.offset = targetOffset;

        // Wait for 6 additional frames
        yield return new WaitForSeconds(6 * frameDuration);

        // Disable the collider and reset its size and offset
        attackCollider.enabled = false;
        attackCollider.size = originalSize;
        attackCollider.offset = originalOffset;
        if (isGrounded && !wasXPositionFrozen)
        {
            // Unfreeze the Rigidbody's X position after the punch
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        }
        yield return new WaitForSeconds(4 * frameDuration);
        punching = false;
        punchRecovery = true;
        yield return new WaitForSeconds(0.2f); // Recovery time after the punch
        punchRecovery = false;
    }


    void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("got here");
        if ((collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("Ground")))
        {

            // If the player is no longer touching an object with the relevant tag, reset the boolean.
            canPlaySound = true;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && rb.velocity.y < 0f)
        {
            canPlaySound = false;
        }
        if (canPlaySound && collision.gameObject.CompareTag("Platform") && rb.velocity.y < 0f)
        {
            canPlaySound = false; // Set the boolean to false to prevent repeated sound.
        }
    }

}
