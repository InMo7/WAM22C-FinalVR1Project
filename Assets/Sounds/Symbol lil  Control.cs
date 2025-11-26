using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Cymbal2Hit : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hitSound;  // Ride cymbal

    [Header("Hit Settings")]
    public float minHitVelocity = 1.0f;
    public float cymbalRadius = 0.4f;  // Edge detection

    private XRGrabInteractable grab;

    private void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        grab = GetComponent<XRGrabInteractable>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (grab != null && grab.isSelected) return;

        Vector3 hitPoint = collision.contacts[0].point - transform.position;
        float distFromCenter = hitPoint.magnitude / cymbalRadius;
        float velocity = collision.relativeVelocity.magnitude;

        if (velocity < minHitVelocity) return;

        // Ride: Bell (center) higher pitch than wash (edge)
        float pitch = (distFromCenter > 0.7f ? 0.9f : 1.4f) + (velocity * 0.1f);
        audioSource.pitch = pitch;
        audioSource.volume = Mathf.Clamp01(velocity * 0.2f);

        audioSource.PlayOneShot(hitSound);
    }
}