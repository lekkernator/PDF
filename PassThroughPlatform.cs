using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassThroughPlatform : MonoBehaviour
{
    private bool isPassingThrough = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the character enters the platform from below or sideways
        if (other.CompareTag("Platform") && !isPassingThrough)
        {
            // Ignore collisions with the platform
            Physics2D.IgnoreCollision(other, GetComponent<Collider2D>(), true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Re-enable collisions when leaving the platform
        if (other.CompareTag("Platform"))
        {
            Physics2D.IgnoreCollision(other, GetComponent<Collider2D>(), false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Character is on top of the platform
        if (collision.gameObject.CompareTag("Platform"))
        {
            isPassingThrough = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Character is no longer on top of the platform
        if (collision.gameObject.CompareTag("Platform"))
        {
            isPassingThrough = true;
        }
    }
}
