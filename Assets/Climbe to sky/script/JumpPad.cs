using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpForce = 30f; // jump height in meters

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();

            if (playerRb != null)
            {
                // Zero out current Y velocity to avoid stacking forces
                Vector3 velocity = playerRb.linearVelocity;
                velocity.y = 0;
                playerRb.linearVelocity = velocity;

                // Apply jump force
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            }
        }
    }
}