using UnityEngine;

public class EnergyBallController : MonoBehaviour
{
    public AudioClip explosion; // Assign this in the Unity Editor
    private AudioSource audioSource;
    private bool isFadingOut = false;
    private float fadeOutDuration = 1.5f;
    private float fadeOutTimer = 0f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Ensure there's an AudioSource component and set it up
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Add AudioSource if not already attached
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Check for position limits
        if (!isFadingOut && (transform.position.x > 15f || transform.position.x < -15f))
        {
            isFadingOut = true;
        }

        // Handle audio fade-out
        if (isFadingOut)
        {
            fadeOutTimer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(1f, 0f, fadeOutTimer / fadeOutDuration);

            if (fadeOutTimer >= fadeOutDuration)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isFadingOut)
        {
            // Play the explosion sound
            audioSource.PlayOneShot(explosion);
            spriteRenderer.enabled = false;
            // Destroy the object after a short delay to allow the audio to play
            Destroy(gameObject, explosion.length);
        }
    }
}
