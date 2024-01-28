using System.Collections;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private bool canFallThroughPlatform = false;

    private void Update()
    {
        // Check for the down arrow key press
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // Allow the player to fall through platforms
            canFallThroughPlatform = true;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            // Allow the player to fall through platforms
            canFallThroughPlatform = true;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        // Check if the player is colliding with a platform
        if (other.CompareTag("Player"))
        {
            // Check if the down arrow key is pressed and allow falling through
            if (canFallThroughPlatform && Input.GetKey(KeyCode.DownArrow))
            {
                // Disable the player's collider for a short time to fall through
                GetComponent<Collider2D>().enabled = false;

                // Use a Coroutine to enable the collider again after a delay
                StartCoroutine(EnableColliderAfterDelay());
            }
        }
    }

    private IEnumerator EnableColliderAfterDelay()
    {
        // Wait for a short time (adjust as needed)
        yield return new WaitForSeconds(0.2f);

        // Re-enable the player's collider
        GetComponent<Collider2D>().enabled = true;

        // Reset the ability to fall through platforms
        canFallThroughPlatform = false;
    }
}
