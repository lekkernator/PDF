using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class Player2Controller : MonoBehaviour
{
    public float playerNumber = 0f;
    public float playerPlacardSpawnDifferential = 210f;
    public AudioClip landingSound; // Sound effect to play when landing on a platform or "Ground."
    private AudioSource audioSource;
    private bool canPlaySound = true; // Boolean to control when the sound can play.
    public AudioClip footstepSound; // Sound effect to play for footsteps.
    private float footstepTimer = 0f; // Timer to control the timing of footstep sounds.
    private float footstepInterval = 0.18f; // The interval at which footstep sounds can play (3 times per second).
    public AudioClip deathSound; // Sound effect to play upon death.
    private SpriteRenderer spriteRenderer; // Sprite Renderer for the character.
    private int deathCount = 0; // Variable to keep track of the number of deaths.
    public GameObject gameOverBannerPrefab; // Assign this in the Unity editor.
    private bool respawning = false; // Boolean to track if character is respawning.
    public GameObject gameOverPrefab; // Assign the gameOverUi prefab in the Unity editor
    private  GameObject gameOverUiPrefab; // Assign the gameOverUi prefab in the Unity editor

    public GameObject healthTrackerPrefab; // Assign the gameOverUi prefab in the Unity editor
    private TextMeshProUGUI HealthTracker; // Assign this in the Unity editor
    public float playerHealth = 0f; // Example starting health

    private RectTransform rectTransform;
    public float basePosition = -258.9f;
    private float twoDigits = -10f;
    private float threeDigits = -10f;

    private Collider2D characterCollider;
    private Rigidbody2D rb;
    public AudioClip gameOverSound;
    private bool canFall;
    private BoxCollider2D damageReceiver;

    public GameObject energyBallPrefab; // Assign this in the Unity Editor
    private float energyBallSpawnTimer = 0f;
    private const float EnergyBallSpawnCooldown = 0.5f;
    public SpriteRenderer[] spritesToDisable; // Assign this array in the Unity Editor
    private int currentSpriteIndex = 0;



    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component.
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(true); // Get the Sprite Renderer component from child objects.
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on child GameObjects");
        }
        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();

        characterCollider = colliders[0]; // Get the player's collider
        damageReceiver = GetColliderFromTaggedChild("Player2SubLayer") as BoxCollider2D; // Get the player's damage receiver collider
        if (damageReceiver == null)
        {
            Debug.LogError("Damage receiver collider not found on child GameObjects");
        }

        (gameOverUiPrefab = Instantiate(gameOverPrefab, transform.position, Quaternion.identity)).SetActive(false); // Spawn to continue message
        var temptracker = Instantiate(healthTrackerPrefab, transform.position, Quaternion.identity);
        temptracker.SetActive(true);
        HealthTracker = temptracker.GetComponentInChildren<TextMeshProUGUI>(); // Spawn to continue message

        rectTransform = HealthTracker.GetComponent<RectTransform>();

        basePosition += playerNumber * playerPlacardSpawnDifferential;
        twoDigits += basePosition;
        threeDigits += twoDigits;
    }
    void Update()
    {
        energyBallSpawnTimer += Time.deltaTime;

        if (!respawning && Input.GetKeyDown(KeyCode.Quote) && energyBallSpawnTimer >= EnergyBallSpawnCooldown)
        {
            ShootEnergyBall();
            energyBallSpawnTimer = 0f; // Reset the timer after spawning an energy ball
        }
        if (respawning)
        {
            playerHealth = 0; // Reset player health when respawning
        }

        // Update the HealthTracker text
        if (HealthTracker != null)
        {
            HealthTracker.text = $"{playerHealth}%";
            UpdatePositionByDigitCount(playerHealth);
        }
        // Check if the down arrow key is pressed and allow falling through
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            canFall = true;
        }
        else
        {
            canFall = false;
        }

        if(rb.position.y < -2.4 && !characterCollider.enabled)
        {
            // Re-enable the player's collider
            characterCollider.enabled = true;
        }

        // Check if the player is moving horizontally and not vertically.
        if (Mathf.Abs(rb.velocity.x) > 0.1f && Mathf.Abs(rb.velocity.y) < 0.1f)
        {
            // Calculate the absolute velocity as a value between 0 and 5.
            float absoluteVelocity = Mathf.Clamp(Mathf.Abs(rb.velocity.x), 0f, 5f);

            // Calculate the new footstep interval proportionately to the velocity.
            footstepInterval = 0.18f * (5f / absoluteVelocity); // Adjust the multiplier as needed.

            // Update the footstep timer.
            footstepTimer += Time.deltaTime;

            // Check if enough time has passed to play the footstep sound.
            if ((footstepTimer >= footstepInterval) && rb.constraints != (RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY))
            {
                // Play the footstep sound.
                audioSource.PlayOneShot(footstepSound, 0.25f);
                footstepTimer = 0f; // Reset the timer.
            }
        }

        // Check if the character is outside the defined boundaries.
        if ((Mathf.Abs(transform.position.x) > 15 || Mathf.Abs(transform.position.y) > 9) && !respawning)
        {
            death(); // Call the death method.
        }
    }
    private void ShootEnergyBall()
    {
        if (energyBallPrefab == null)
        {
            Debug.LogError("EnergyBall prefab is not assigned in the inspector");
            return;
        }

        // Calculate spawn position
        Vector3 spawnPosition = transform.position + new Vector3(spriteRenderer.flipX ? -1 : 1, 0, 0);

        // Instantiate the energy ball
        GameObject energyBall = Instantiate(energyBallPrefab, spawnPosition, Quaternion.identity);

        // Get the Rigidbody2D component of the energy ball
        Rigidbody2D energyBallRb = energyBall.GetComponent<Rigidbody2D>();
        if (energyBallRb != null)
        {
            // Set velocity
            energyBallRb.velocity = new Vector2(spriteRenderer.flipX ? -5 : 5, 0);
        }

        // Rotate the sprite renderer of the energy ball
        SpriteRenderer energyBallSprite = energyBall.GetComponent<SpriteRenderer>();
        if (energyBallSprite != null)
        {
            StartCoroutine(RotateSprite(energyBallSprite));
        }
    }

    private IEnumerator RotateSprite(SpriteRenderer spriteRenderer)
    {
        float rotationSpeed = 360f * 4; // 4 full rotations per second

        while (spriteRenderer != null)
        {
            spriteRenderer.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private Collider2D GetColliderFromTaggedChild(string childTag)
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag(childTag))
            {
                return child.GetComponent<Collider2D>();
            }
        }
        return null; // Return null if no matching child is found
    }

    void UpdatePositionByDigitCount(float percentage)
    {
        if (percentage < 10)
        {
            // Update the X position
            rectTransform.anchoredPosition = new Vector2(basePosition, rectTransform.anchoredPosition.y);
        }
        else if (percentage < 100)
        {
            // Update the X position
            rectTransform.anchoredPosition = new Vector2(twoDigits, rectTransform.anchoredPosition.y);
        }
        else
        {
            // Update the X position
            rectTransform.anchoredPosition = new Vector2(threeDigits, rectTransform.anchoredPosition.y);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if ( collision.gameObject.tag != "Platform" && collision.gameObject.tag != "Ground" && collision.gameObject.tag != "Player1SubLayer")
        {
            Debug.Log("Player: " + playerNumber + ". Tag: " + collision.gameObject.tag);
            Debug.Log("gettriggered");
            playerHealth += 3; // Increment health by 3 when the damage receiver collider collides
            ApplyKnockbackEffect(collision);
        }
        if (( collision.gameObject.tag == "Player")|| (collision.gameObject.tag == "Player1SubLayer" ))
        {
            Debug.Log("knockback achieved");
            ApplyKnockbackEffect(collision);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (canPlaySound && (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("Ground")) && rb.velocity.y < 0f)
        {
            // Play the landing sound.
            audioSource.PlayOneShot(landingSound);
            canPlaySound = false; // Set the boolean to false to prevent repeated sound.
        }
    }

    private void ApplyKnockbackEffect(Collider2D collision)
    {
        // Define minimum knockback force
        float minKnockbackForce = 2f; // Adjust this value as needed

        // Calculate knockback multiplier
        float knockbackMultiplier = Mathf.Lerp(1f, 6f, playerHealth / 200f);

        // Determine the direction of the knockback based on the positions of the player and the collider, only considering the X-axis
        float direction = transform.position.x - collision.transform.position.x;
        direction = Mathf.Sign(direction); // This will be 1 if to the right, -1 if to the left

        // Create the knockback force vector, only affecting the X-axis
        Vector2 knockbackForce = new Vector2(direction * Mathf.Max(minKnockbackForce, knockbackMultiplier), 0);
        Debug.Log(knockbackForce);

        // Apply the force
        rb.AddForce(knockbackForce, ForceMode2D.Impulse);
    }


    void OnCollisionStay2D(Collision2D other)
    {
        TogglePlatformCollision(other.collider);
    }

    private void TogglePlatformCollision(Collider2D other)
    {
        // Check if the player is colliding with a platform
        if (other.CompareTag("Platform"))
        {
            // Check if the down arrow key is pressed and allow falling through
            if (canFall)
            {
                // Disable the player's collider for a short time to fall through
                characterCollider.enabled = false;

                // Use a Coroutine to enable the collider again after a delay
                StartCoroutine(EnableColliderAfterDelay());
            }
        }
    }

    private IEnumerator EnableColliderAfterDelay()
    {
        // Wait for a short time (adjust as needed)
        yield return new WaitForSeconds(0.23f);

        // Re-enable the player's collider
        characterCollider.enabled = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("got here");
        if ( (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("Ground")))
        {

            // If the player is no longer touching an object with the relevant tag, reset the boolean.
            canPlaySound = true;
        }
    }

    // Method to handle the death of the player.
    void death()
    {
        Debug.Log("died");
        // Set Respawning flag on actual death.
        respawning = true;

        rb.position = new Vector3(0, 8.99f, 0);

        spriteRenderer.enabled = false; // Make the character invisible.
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        if (currentSpriteIndex < spritesToDisable.Length)
        {
            spritesToDisable[currentSpriteIndex].enabled = false;
            currentSpriteIndex++;
        }
        if (deathCount >= 3)
        {
            HandleGameOver(); // Handle the game over scenario.
        }
        else
        {
            StartCoroutine(MoveCharacter(new Vector3(0, 0, 0)));
        }
    }

    void HandleGameOver()
    {
        gameOverUiPrefab.SetActive(true);
        // Instantiate the game over banner at position (0,0,0).
        Instantiate(gameOverBannerPrefab, new Vector3(0, 0.4f, -5), Quaternion.identity);
        gameOverBannerPrefab.GetComponent<Renderer>().enabled = true;
        audioSource.PlayOneShot(gameOverSound);
        // Slow down time.
        Time.timeScale = 0;
    }

    // Coroutine to move the character.
    IEnumerator MoveCharacter(Vector3 target)
    {
        Debug.Log("respawning");
        audioSource.PlayOneShot(deathSound);
        deathCount++; // Increment the death count.
        yield return new WaitForSeconds(2f); // Wait for 2 seconds.

        // Make the character visible again.
        spriteRenderer.enabled = true;

        float elapsedTime = 0;
        Vector3 startPosition = rb.position;
        while (elapsedTime < 1.5f)
        {
            float progress = elapsedTime / 1.5f;
            rb.position = Vector3.Lerp(startPosition, target, Mathf.Pow(progress,0.41f) ); // Slower approach to the target.
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.position = target;

        yield return new WaitForSeconds(2f);

        // Reset the respawning flag.
        respawning = false;

        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
