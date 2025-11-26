using UnityEngine;
 // If using XR grabs

public class BassDrumHit : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hitSound;  // Bass thump

    [Header("Hit Settings")]
    public float minHitVelocity = 0.5f;  // Sensitive for foot pedal feel

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab; // Optional: skip sounds while grabbed

    private void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (grab != null && grab.isSelected) return; // Skip if grabbed

        float velocity = collision.relativeVelocity.magnitude;
        if (velocity < minHitVelocity) return;

        // Bass: Lower pitch variation
        audioSource.pitch = 0.6f + (velocity * 0.05f);  // Deep tones
        audioSource.volume = Mathf.Clamp01(velocity * 0.2f);

        audioSource.PlayOneShot(hitSound);
    }
}