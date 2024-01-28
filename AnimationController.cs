using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private float idleAnimationLength = 1.25f;
    private bool isIdling = false;

    private Animator animator; // Animator component
    private Vector2 lastPosition; // Last recorded position
    private float timeSinceLastMove = 0f; // Time since character last moved
    private bool isPunching = false;
    private AudioSource audioSource;
    public AudioClip punch;
    public float walkTime = 0f;
    private bool punchRecovery = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lastPosition = transform.position;
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the GameObject");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isPunching)
        {
            animator.Play("New State");
            Debug.Log("punching");
            StartCoroutine(Punch());
        }
        else if (animator.GetBool("IsGrounded") && !isPunching && !punchRecovery && ( Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))) 
        {
            walkTime -= Time.deltaTime;
            timeSinceLastMove = 0;
            if (walkTime <= 0f)
            {
                animator.SetTrigger("walk");
                walkTime = 0.5f;
            }

        }
        else if (!isPunching && !punchRecovery && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.F)))
        {
            walkTime = 0f;
            timeSinceLastMove = 0;
            animator.Play("New State");
        }
        //idle logic
        else if (!isPunching)
        {
            if ((Vector2)transform.position != lastPosition)
            {
                // Character has moved
                lastPosition = transform.position; // Update last position
                timeSinceLastMove = 0f; // Reset timer
                if (animator != null)
                {
                    //animator.enabled = false; // Disable the animator when the character moves
                }
            }
            else
            {
                // Character has not moved
                if (timeSinceLastMove < 1.5f)
                {
                    walkTime = 0f;
                    animator.Play("New State");
                }
                //Add time
                timeSinceLastMove += Time.deltaTime;
                //Idle animation logic
                if (timeSinceLastMove > 1.5f && !isIdling)
                {
                    isIdling = true;
                    // More than one second has passed since last move
                    if (animator != null)
                    {
                        //animator.enabled = true;
                    }
                    animator.SetTrigger("idle"); // Trigger idle animation
                }
                else if (timeSinceLastMove > 1.5f + idleAnimationLength)
                {

                    timeSinceLastMove = 1.5f;
                    isIdling = false;
                }
            }
        }
        
    }

    private IEnumerator Punch()
    {
        walkTime = 2f;
        timeSinceLastMove = 0f;
        isIdling = false;
        Debug.Log("set idling false");
        if (!animator.enabled)
        {
            //animator.enabled = true;
        }
        Debug.Log("punch now");
        isPunching = true;
        audioSource.PlayOneShot(punch, 0.5f);
        animator.SetTrigger("punch");
        yield return new WaitForSeconds(0.333f);

        Debug.Log("punched");
        isPunching = false;
        punchRecovery = true;
        yield return new WaitForSeconds(0.2f);
        punchRecovery = false;
        //animator.enabled = false;

    }
}
